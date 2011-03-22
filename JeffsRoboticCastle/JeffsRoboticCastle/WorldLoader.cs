using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;


class WorldLoader
{
// Public
    // Constructor
    public WorldLoader(Canvas newCanvas, double[] screenPosition, double[] screenSize)
    {
        // setup a canvas to put the world on, for the purpose of cropping
        this.gameCanvas = newCanvas;
        this.worldCanvas = new Canvas();
        this.worldCanvas.Width = screenSize[0];
        this.worldCanvas.Height = screenSize[1];
        this.worldCanvas.RenderTransform = new TranslateTransform(screenPosition[0], screenPosition[1]);
        this.worldCanvas.ClipToBounds = true;
        this.gameCanvas.Children.Add(worldCanvas);

        // setup machinery to make it fast to run
        this.blockSize = new double[2];
        blockSize[0] = screenSize[0];
        blockSize[1] = screenSize[1];
        double[] activeDimensions = new double[2];
        activeDimensions[0] = blockSize[0] * 2;
        activeDimensions[1] = blockSize[0] * 2;
        double[] existentDimensions = new double[2];
        existentDimensions[0] = activeDimensions[0] * 2;
        existentDimensions[1] = activeDimensions[1] * 2;
        this.worldDimensions = new double[2];
        worldDimensions[0] = 30000;
        worldDimensions[1] = 2000;
        //this.dimensionsOfRealityBubble = Math.Max(screenSize[0], screenSize[1]) * 2;
        // make the world
        this.world = new World(worldCanvas, screenSize, existentDimensions);
        // Inform the world who it needs to tell whenever something moves
        this.world.setLoader(this);
        // Setup to hold the unspawned objects
        this.searcher = new WorldSearcher(2, blockSize, worldDimensions);

        // calculate the initial reality bubble
        WorldBox activeRegion = new WorldBox(0, activeDimensions[0], 0, activeDimensions[1]);
        WorldBox existentRegion = new WorldBox(0, existentDimensions[0], 0, existentDimensions[1]);
        //int i;
        //for (i = 0; i < 2; i++)
        //    existentRegion.enlarge(i, existentRegion.getSize(i));
        this.initialRealityBubble = new RealityBubble(activeRegion, existentRegion);
        this.realityBubble = this.calculateRealityBubble();
        this.objectsToUnspawn = new System.Collections.Generic.HashSet<GameObject>();
        this.objectsToDeactivate = new System.Collections.Generic.HashSet<GameObject>();
        this.loadedObjects = new System.Collections.Generic.HashSet<GameObject>();


        this.populate();

    }

    // put stuff in the world
    public void populate()
    {
        // #define DESIGN_HERE
        double[] location = new double[2];
        location[0] = 1000;
        location[1] = 400;
        this.addItem(new Enemy(location, 0, 0, 1));
        double x;
        double y;
#if false
        y = 600;
        for (x = -1000; x < 2000; x += 100)
        {
            location[0] = x;
            location[1] = y;
            this.addItem(new Platform(location, 0));
        }
        return;
#endif
        int i, type;
        int weaponType1;
        int weaponType2;
        Random generator = new Random();
        // add wallpaper
        for (x = 0; x < worldDimensions[0]; x += 1200)
        {
            for (y = 500; y < worldDimensions[1]; y += 1200)
            {
                location = new double[2]; location[0] = x; location[1] = y;
                this.addItem(new Painting(location, 1));
            }
        }
        x = 1000;
        while (x < worldDimensions[0])
        {
            // add an enemy
            x += 100 + 600 * generator.NextDouble();
            y = 600 * generator.NextDouble();
            weaponType1 = (int)(15 * generator.NextDouble());
            weaponType2 = (int)(15 * generator.NextDouble());
            location = new double[2]; location[0] = x; location[1] = y;
            type = (int)(generator.NextDouble() * 3);
            this.addItem(new Enemy(location, type, weaponType1, weaponType2));
            // give the enemy a painting to look at
            location = new double[2]; location[0] = x; location[1] = y;
            this.addItem(new Painting(location, 0));
        }

#if true
        for (i = 0; i < 300; i++)
        {
            x = generator.NextDouble() * worldDimensions[0];
            y = generator.NextDouble() * 500;
            location = new double[2]; location[0] = x; location[1] = y;
            type = (int)(generator.NextDouble() * 2);
            this.addItem(new Platform(location, type));
        }
#endif
        /*location = new double[2]; location[0] = 1400; location[1] = 300;
        this.addItem(new Enemy(location, 0));

        location = new double[2]; location[0] = 1500; location[1] = 300;
        this.addItem(new Enemy(location, 0));

        location = new double[2]; location[0] = 1600; location[1] = 300;
        this.addItem(new Enemy(location, 0));

        location = new double[2]; location[0] = 3100; location[1] = 300;
        this.addItem(new Enemy(location, 0));


        location = new double[2]; location[0] = 4000; location[1] = 300;
        this.addItem(new Enemy(location, 0));

        location = new double[2]; location[0] = 4300; location[1] = 300;
        this.addItem(new Enemy(location, 0));
        */

        /*location = new double[2]; location[0] = 1250; location[1] = 500;
        this.world.addCharacter(new Enemy(location, 0));
        */

        // make platforms
        /*location = new double[2]; location[0] = 700; location[1] = 60;
        this.world.addPlatform(new Platform(location, 0));
        location = new double[2]; location[0] = 800; location[1] = 120;
        this.world.addPlatform(new Platform(location, 0));
        location = new double[2]; location[0] = 700; location[1] = 220;
        this.world.addPlatform(new Platform(location, 0));
        */

        /*// back wall
        location = new double[2]; location[0] = 0; location[1] = 50;
        this.addItem(new Platform(location, 1));
        location = new double[2]; location[0] = 0; location[1] = 200;
        this.addItem(new Platform(location, 1));

        // steps
        location = new double[2]; location[0] = 400; location[1] = 100;
        this.addItem(new Platform(location, 0));
        location = new double[2]; location[0] = 500; location[1] = 250;
        this.addItem(new Platform(location, 0));

        // a shelter
        location = new double[2]; location[0] = 700; location[1] = 50;
        this.addItem(new Platform(location, 1));
        location = new double[2]; location[0] = 650; location[1] = 105;
        this.addItem(new Platform(location, 0));


        // far forward wall
        location = new double[2]; location[0] = 2000; location[1] = 70;
        this.addItem(new Platform(location, 1));


        // far backward wall
        location = new double[2]; location[0] = -2000; location[1] = 70;
        this.addItem(new Platform(location, 1));
        */
        // Paintings
        /*x = 0;
        for (i = 0; i < 20; i++)
        {
            x += 400 * generator.NextDouble() + 200;
            location = new double[2]; location[0] = x; location[1] = 200;
            this.addItem(new Painting(location, 0));
        }
        */

        /*int i, j;
        for (i = 100; i <= 700; i += 200)
        {
            for (j = i / 10 + 100; j <= i + 500; j += 100)
            {
                location = new double[2]; location[0] = i; location[1] = j;
                this.world.addPlatform(new Platform(location));
            }
        }*/
    }
    public void scrollTo(GameObject o)
    {
        // Now remove anything from the previous tick that fell off the edge of the world
        foreach (GameObject o2 in this.objectsToUnspawn)
        {
            this.world.unloadItem(o2);
        }
        // First, deactivate anything from the previous tick that is getting close to the edge of the world
        foreach (GameObject o2 in this.objectsToDeactivate)
        {
            this.world.deactivateItem(o2);
        }
        this.objectsToDeactivate.Clear();
        this.objectsToUnspawn.Clear();


        // Now move the world
        this.world.scrollTo(o);
        
        // Compute the new and old reality bubbles so we can load and unload as necessary
        RealityBubble newRealityBubble = this.calculateRealityBubble();
        WorldBox newExistenceRegion = newRealityBubble.getExistentRegion();
        WorldBox oldExistenceRegion = this.realityBubble.getExistentRegion();
        WorldBox newActiveRegion = newRealityBubble.getActiveRegion();
        WorldBox oldActiveRegion = this.realityBubble.getActiveRegion();
        // Find everything that is no longer active
        System.Collections.Generic.List<GameObject> previouslyActive = this.searcher.getObjectsOnlyInA(oldActiveRegion, newActiveRegion);
        foreach (GameObject o2 in previouslyActive)
        {
            this.world.deactivateItem(o2);
        }
        // Find everything that is no longer present
        System.Collections.Generic.List<GameObject> oldItems = this.searcher.getObjectsOnlyInA(oldExistenceRegion, newExistenceRegion);
        foreach (GameObject o2 in oldItems)
        {
            this.world.unloadItem(o2);
        }
        // Find everything that just became present
        System.Collections.Generic.List<GameObject> newItems = this.searcher.getObjectsOnlyInA(newExistenceRegion, oldExistenceRegion);
        foreach (GameObject o2 in newItems)
        {
            this.world.addDisabledItem(o2);
        }
        // Find everything that just became active
        System.Collections.Generic.List<GameObject> newlyActive = this.searcher.getObjectsOnlyInA(newActiveRegion, oldActiveRegion);
        foreach (GameObject o2 in newlyActive)
        {
            this.world.activateItem(o2);
        }
        this.realityBubble = newRealityBubble;
    }
    public void timerTick(double numSeconds)
    {
        // advance the world
        this.world.timerTick(numSeconds);
    }
    public void addItemAndDisableUnloading(GameObject o)
    {
        this.world.addItem(o);
    }
    public void addItem(GameObject o)
    {
        this.searcher.addItem(o);
        this.loadedObjects.Add(o);
        IndexBox existenceIndices = this.searcher.getIndexBoxFromWorldBox(this.realityBubble.getExistentRegion());
        IndexBox objectIndices = this.searcher.getIndexBoxFromWorldBox(o.getBoundingBox());
        if (existenceIndices.intersects(objectIndices))
        {
            IndexBox activeIndices = this.searcher.getIndexBoxFromWorldBox(this.realityBubble.getActiveRegion());
            if (activeIndices.intersects(objectIndices))
                this.world.addItem(o);
            else
                this.world.addDisabledItem(o);
        }
    }
    // This function gets called by the world when it wants to destroy an object
    public void removingObject(GameObject o)
    {
        if (loadedObjects.Contains(o))
        {
            this.searcher.removeItem(o);
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
#if false
        // Some objects are created by the world, not loaded from the landscape
        // If an object is created by the world then we don't control it here
        if (this.loadedObjects.Contains(o))
        {
            //this.searcher.removeItem(o);
            this.searcher.itemStartingMove(o);
        }
#else
        // If the object isn't in the searcher then we will never call the itemEndingMove and it won't actually change anything
        // It is slightly worse form to leave out the if statement here but it is slightly faster for runtime
        this.searcher.itemStartingMove(o);
#endif
    }
    // This function is called when an object just finished moving
    // This is to allow the world searcher to update properly and to let us unspawn anything that fell off the edge of the world
    public void objectEndingMove(GameObject o)
    {
        // Some objects are created by the world, not loaded from the landscape
        // If an object is created by the world then we don't control it here
        if (this.loadedObjects.Contains(o))
        {
            IndexBox existentIndices = this.searcher.getIndexBoxFromWorldBox(this.realityBubble.getExistentRegion());
            IndexBox objectIndices = this.searcher.getIndexBoxFromWorldBox(o.getBoundingBox());
            this.searcher.itemEndingMove(o);
            if (!existentIndices.intersects(objectIndices))
            {
                // flag it to remove later
                this.objectsToUnspawn.Add(o);
            }
            else
            {
                IndexBox activeIndices = this.searcher.getIndexBoxFromWorldBox(this.realityBubble.getActiveRegion());
                if (!objectIndices.intersects(activeIndices))
                {
                    // flag it to deactivate later
                    this.objectsToDeactivate.Add(o);
                }
            }
        }
    }
// Private
    // Using the current values of the world camera, calculate which parts of the world need to be part of reality
    private RealityBubble calculateRealityBubble()
    {
        // First create the smallest possible indexbox containing the camera
        WorldBox visibleBox = new WorldBox(this.world.getVisibleWorldBox());

        RealityBubble newBubble = new RealityBubble(this.initialRealityBubble);
        newBubble.centerOn(visibleBox);


        // Now increase the size a little for the region where time exists
        //activeBox.enlarge(0, activeBox.getSize(0));
        //activeBox.enlarge(1, activeBox.getSize(1));

        // Lastly, increase the size even more for the region where time doesn't exist
        //WorldBox existentBox = new WorldBox(activeBox);
        //existentBox.enlarge(0, activeBox.getSize(0) / 3);
        //existentBox.enlarge(1, activeBox.getSize(1) / 3);

        //RealityBubble newBubble = new RealityBubble(activeBox, existentBox);
        return newBubble;
    }
    Canvas gameCanvas;
    Canvas worldCanvas;
    World world;
    WorldSearcher searcher;
    RealityBubble initialRealityBubble;
    //double[] dimensionsOfRealityBubble;
    double[] blockSize;
    double[] worldDimensions;
    RealityBubble realityBubble;
    System.Collections.Generic.HashSet<GameObject> objectsToUnspawn;
    System.Collections.Generic.HashSet<GameObject> objectsToDeactivate;
    System.Collections.Generic.HashSet<GameObject> loadedObjects;
};