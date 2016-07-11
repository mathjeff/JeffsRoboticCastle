#define SAVE_SCREENSHOTS


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

// The WorldLoader class implements the automatic loading and unloading of objects in the World based on LevelPlayer location
class WorldLoader
{
// Public
    // Constructor
    public WorldLoader(Size worldDimensions, Size realityBubbleSize)
    {
        // setup machinery to make it fast to run
        this.blockSize = new Size(realityBubbleSize.Width / 8, realityBubbleSize.Height / 4);
        Size characterActiveDimensions = new Size(realityBubbleSize.Width, realityBubbleSize.Height * 1.5);
        Size terrainActiveDimensions = new Size(characterActiveDimensions.Width + blockSize.Width * 2,
            characterActiveDimensions.Height + blockSize.Height * 2);
        this.worldDimensions = worldDimensions;
        // make the world
        this.world = new World(terrainActiveDimensions);
        // Inform the world who it needs to tell whenever something moves
        this.world.setLoader(this);
        // Setup to hold the unspawned objects
        this.terrainSearcher = new WorldSearcher(2, blockSize, worldDimensions);
        this.characterSearcher = new WorldSearcher(2, blockSize, worldDimensions);

        // calculate the initial reality bubbles 
        WorldBox terrainRegion = new WorldBox(0, terrainActiveDimensions.Width, 0, terrainActiveDimensions.Height);
        WorldBox characterRegion = new WorldBox(0, characterActiveDimensions.Width, 0, characterActiveDimensions.Height);
        
        this.realityBubble = this.initialRealityBubble = new RealityBubble(characterRegion, terrainRegion);
        this.objectsToUnspawn = new System.Collections.Generic.HashSet<GameObject>();
        this.objectsToDeactivate = new System.Collections.Generic.HashSet<GameObject>();
        this.loadedObjects = new System.Collections.Generic.HashSet<GameObject>();

        this.populate();

    }
    public void pause()
    {
        this.paused = true;
    }
    public void unPause()
    {
        this.paused = false;
    }
    public void togglePause()
    {
        this.paused = !(this.paused);
    }
    // Moves the reality bubble to be centered on the given box
    public void RecenterRealityBubble(WorldBox boxToCenterOn)
    {
        // First remove anything from the previous tick that fell off the edge of the world
        foreach (GameObject o2 in this.objectsToUnspawn)
        {
            this.world.unloadItem(o2);
        }
        this.objectsToUnspawn.Clear();

        // Determine the location of the new reality bubble
        RealityBubble newRealityBubble = new RealityBubble(this.initialRealityBubble);
        newRealityBubble.centerOn(boxToCenterOn);
        WorldBox newExistenceRegion = newRealityBubble.getExistentRegion();
        WorldBox oldExistenceRegion = this.realityBubble.getExistentRegion();
        WorldBox newActiveRegion = newRealityBubble.getActiveRegion();
        WorldBox oldActiveRegion = this.realityBubble.getActiveRegion();
        // Find everything that is no longer active
        System.Collections.Generic.List<GameObject> previouslyActive = this.characterSearcher.getObjectsOnlyInA(oldActiveRegion, newActiveRegion);
        foreach (GameObject o2 in previouslyActive)
        {
            this.world.removeItem(o2);
        }
        // Find everything that is no longer present
        System.Collections.Generic.List<GameObject> oldItems = this.terrainSearcher.getObjectsOnlyInA(oldExistenceRegion, newExistenceRegion);
        foreach (GameObject o2 in oldItems)
        {
            this.world.removeItem(o2);
        }
        // Find everything that just became present
        System.Collections.Generic.List<GameObject> newItems = this.terrainSearcher.getObjectsOnlyInA(newExistenceRegion, oldExistenceRegion);
        foreach (GameObject o2 in newItems)
        {
            this.world.addItem(o2);
        }
        // Find everything that just became active
        System.Collections.Generic.List<GameObject> newlyActive = this.characterSearcher.getObjectsOnlyInA(newActiveRegion, oldActiveRegion);
        foreach (GameObject o2 in newlyActive)
        {
            this.world.addItem(o2);
        }
        this.realityBubble = newRealityBubble;
    }
    public void timerTick(double numSeconds)
    {
        if (!this.paused)
        {
            // it's not really so awesome to save screenshots, and it does slow things down substantially
#if SAVE_SCREENSHOTS
            screenshotTimer += numSeconds;
            // count the number of explosions in the world so we can save screenshots with lots of explosions
            int numExplosions = world.getNumExplosions();
            // if it's been a while since the last screenshot, save another one once there's an explosion
            if (screenshotTimer >= 10)
            {
                this.bestNumExplosions = 0;
            }
            // players like to see screenshots with lots of explosions
            if (numExplosions > this.bestNumExplosions)
            {
                this.saveScreenshotInWorld();
                screenshotTimer = 0;
                this.bestNumExplosions = numExplosions;
            }
#endif
            // advance the world
            this.world.timerTick(numSeconds);
        }
    }
    public void addItemAndDisableUnloading(GameObject o)
    {
        this.world.addItem(o);
    }
    public void addItem(GameObject o)
    {
        // keep track of the fact that we have to manage this item
        this.loadedObjects.Add(o);
        // get the object's bounding box
        // decide whether to compare against the smaller character reality bubble or the larger terrain reality bubble
        if (o.isMovable())
        {
            this.characterSearcher.addItem(o);
            IndexBox activeIndices = this.characterSearcher.getIndexBoxFromWorldBox(this.realityBubble.getActiveRegion());
            IndexBox objectIndices = this.characterSearcher.getIndexBoxFromWorldBox(o.getBoundingBox());
            if (activeIndices.intersects(objectIndices))
            {
                this.world.addItem(o);
            }
        }
        else
        {
            this.terrainSearcher.addItem(o);
            IndexBox existenceIndices = this.terrainSearcher.getIndexBoxFromWorldBox(this.realityBubble.getExistentRegion());
            IndexBox objectIndices = this.terrainSearcher.getIndexBoxFromWorldBox(o.getBoundingBox());
            if (existenceIndices.intersects(objectIndices))
            {
                this.world.addItem(o);
            }
        }    
    }
    // This function gets called by the world when it wants to destroy an object
    public void removingObject(GameObject o)
    {
        if (loadedObjects.Contains(o))
        {
            if (o.isMovable())
                this.characterSearcher.removeItem(o);
            else
                this.terrainSearcher.removeItem(o);
            this.loadedObjects.Remove(o);
            if (this.objectsToUnspawn.Contains(o))
                this.objectsToUnspawn.Remove(o);
            if (this.objectsToDeactivate.Contains(o))
                this.objectsToDeactivate.Remove(o);
        }
    }
    // This function is called when an object is about to move
    // This is to allow the world searcher to update properly
    public void objectStartingMove(GameObject o)
    {
        // If the object isn't in the searcher then we will never call the itemEndingMove and it won't actually change anything
        // It is slightly worse form to leave out the if statement here but it is slightly faster for runtime
        if (o.isMovable())
            this.characterSearcher.itemStartingMove(o);
        else
            this.terrainSearcher.itemStartingMove(o);
    }
    // This function is called when an object just finished moving
    // This is to allow the world searcher to update properly and to let us unspawn anything that fell off the edge of the world
    public void objectEndingMove(GameObject o)
    {
        // Some objects are created by the world, not loaded from the landscape
        // If an object is created by the world then we don't control it here
        if (this.loadedObjects.Contains(o))
        {
            WorldSearcher searcher;
            IndexBox existentIndices;
            IndexBox objectIndices;
            if (o.isMovable())
            {
                searcher = this.characterSearcher;
                existentIndices = searcher.getIndexBoxFromWorldBox(this.realityBubble.getActiveRegion());
            }
            else
            {
                searcher = this.terrainSearcher;
                existentIndices = searcher.getIndexBoxFromWorldBox(this.realityBubble.getExistentRegion());
            }
            objectIndices = searcher.getIndexBoxFromWorldBox(o.getBoundingBox());
            searcher.itemEndingMove(o);
            if (!existentIndices.intersects(objectIndices))
            {
                // flag it to remove later
                this.objectsToUnspawn.Add(o);
            }

        }
    }
    // tells whether this character is touching any portans
    public bool characterTouchingPortal(Character character)
    {
        return this.world.characterTouchingPortal(character);
    }
    // return an image source with a screenshot of the world
    public ImageSource getScreenshot()
    {
        // TODO fix this
        double actualWidth = this.worldCanvas.ActualWidth;
        double actualHeight = this.worldCanvas.ActualHeight;

        double renderWidth = 300;
        double widthScale = renderWidth / actualWidth;
        double heightScale = widthScale;
        double renderHeight = actualHeight * heightScale;


        

        RenderTargetBitmap renderTarget = new RenderTargetBitmap((int) renderWidth, (int) renderHeight, 96, 96, PixelFormats.Pbgra32);
        VisualBrush sourceBrush = new VisualBrush(this.worldCanvas);

        DrawingVisual drawingVisual = new DrawingVisual();
        DrawingContext drawingContext = drawingVisual.RenderOpen();

        using (drawingContext)
        {
            drawingContext.PushTransform(new ScaleTransform(widthScale, heightScale));
            // draw the screenshot
            drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(actualWidth, actualHeight)));
            // draw a red border
            System.Windows.Media.Pen borderPen = new Pen(Brushes.Red, 2);
            Point topLeft = new Point(0, 0);
            Point topRight = new Point(0, actualHeight - 1);
            Point bottomLeft = new Point(actualWidth - 1, 0);
            Point bottomRight = new Point(actualWidth - 1, actualHeight - 1);
            drawingContext.DrawLine(borderPen, topLeft, topRight);
            drawingContext.DrawLine(borderPen, topRight, bottomRight);
            drawingContext.DrawLine(borderPen, bottomRight, bottomLeft);
            drawingContext.DrawLine(borderPen, bottomLeft, topLeft);
        }
        renderTarget.Render(drawingVisual);

        return renderTarget;
    }
    // get the current screenshot and save it in the world somewhere
    public void saveScreenshotInWorld()
    {
        // find everything that is slightly to the right of the screen
        RealityBubble newBubble = this.realityBubble;
        WorldBox realBox = new WorldBox(newBubble.getActiveRegion());
        realBox.shift(0, 3000);
        GameObject collisionBox = new GameObject();
        collisionBox.setShape(new GameRectangle(realBox.getSize(0), realBox.getSize(1)));
        collisionBox.setCenter(realBox.getCenter());
        List<GameObject> collisions = this.terrainSearcher.getCollisions(collisionBox);
        // find a painting
        Painting bestPainting = null;
        foreach (GameObject collision in collisions)
        {
            if (collision.isAPainting())
            {
                Painting tempPainting = (Painting)collision;
                if ((tempPainting.getShape().getWidth() <= 400) && (tempPainting.getShape().getWidth() > 200))
                {
                    bestPainting = tempPainting;
                    break;
                }
           }
        }
        if (bestPainting != null)
        {
            // TODO fix this
            // get the current screenshot
            // ImageSource source = this.getScreenshot();
            // put the screenshot in the painting
            // bestPainting.setBitmap(source);
        }
    }
    // performs cleanup on the world so it is essentially gone
    public void destroy()
    {
        this.world.destroy();
    }

    public void RegisterForUpdates(WorldScreen screen)
    {
        this.world.RegisterForUpdates(screen);
    }

// Private
    // put stuff in the world
    void populate()
    {
        double[] location = new double[2];
        location[0] = 1000;
        location[1] = 400;
        //this.addItem(new Enemy(location, 2, 3, 4));
        double x;
        double y;
        int i, type;
        Random generator = new Random();
        // add wallpaper
        for (x = 0; x < this.worldDimensions.Width; x += 1024)
        {
            for (y = 500; y < this.worldDimensions.Height; y += 1024)
            {
                location = new double[2]; location[0] = x; location[1] = y;
                this.addItem(new Painting(location, 1));
            }
        }
        // add paintings to display screenshots 
        for (x = 0; x < this.worldDimensions.Width; x += 500)
        {
            for (y = 0; y < this.worldDimensions.Height; y += 500)
            {
                location = new double[2]; location[0] = x; location[1] = y + generator.Next(500);
                this.addItem(new Painting(location, 0));
            }
        }
        x = 1000;

        
        
        // add an exit
        location[0] = worldDimensions.Width + 100;
        location[1] = 50;
        Portal exit = new Portal();
        exit.setCenter(location);
        this.addItem(exit);
        // add platforms
        double spacing = 30;
        for (i = 0; i < 4; i++)
        {
            if (generator.Next(2) == 0)
                spacing *= 2;
        }
        double roofAltitude = worldDimensions.Height * .75;
        int count = (int)(worldDimensions.Width / spacing);
        for (i = 0; i < count; i++)
        {
            x = generator.NextDouble() * worldDimensions.Width + 100;
            y = generator.NextDouble() * roofAltitude;
            location = new double[2]; location[0] = x; location[1] = y;
            type = (int)(generator.NextDouble() * 2);
            this.addItem(new Platform(location, type));
        }

        // create the walls of the world
        // top wall
        GameObject topWall;
        x = 0;
        while (x < worldDimensions.Width)
        {
            location = new double[2]; location[0] = x; location[1] = roofAltitude;
            topWall = new Platform(location, 0);
            this.addItem(topWall);
            x += topWall.getShape().getWidth();
        }

        // left wall
        GameObject leftWall;
        y = 0;
        while (y < worldDimensions.Height)
        {
            location = new double[2]; location[0] = 0; location[1] = y;
            leftWall = new Platform(location, 1);
            this.addItem(leftWall);
            y += leftWall.getShape().getHeight();
        }
        location = new double[2]; location[0] = 0; location[1] = y;
        leftWall = new Platform(location, 1);
        this.addItem(leftWall);
        // don't need to bother with the right wall, because the exit is there
    }

    Canvas worldCanvas;
    World world;
    WorldSearcher characterSearcher;
    WorldSearcher terrainSearcher;
    RealityBubble initialRealityBubble;
    Size blockSize;
    Size worldDimensions;
    RealityBubble realityBubble;
    System.Collections.Generic.HashSet<GameObject> objectsToUnspawn;
    System.Collections.Generic.HashSet<GameObject> objectsToDeactivate;
    System.Collections.Generic.HashSet<GameObject> loadedObjects;
    double screenshotTimer;
    int bestNumExplosions;
    bool paused;
};