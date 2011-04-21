﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;


class WorldLoader
{
// Public
    // Constructor
    public WorldLoader(Canvas canvas, double[] screenSize, int levelNumber)
    {
        // setup a canvas to put the world on, for the purpose of cropping
        this.worldCanvas = canvas;
        /*this.worldCanvas = new Canvas();
        this.worldCanvas.Width = screenSize[0];
        this.worldCanvas.Height = screenSize[1];
        this.worldCanvas.RenderTransform = new TranslateTransform(screenPosition[0], screenPosition[1]);
        this.worldCanvas.ClipToBounds = true;
        this.gameCanvas.Children.Add(worldCanvas);
        */
        // setup machinery to make it fast to run
        this.blockSize = new double[2];
        blockSize[0] = screenSize[0] / 8;
        blockSize[1] = screenSize[1] / 8;
        double[] characterActiveDimensions = new double[2];
        characterActiveDimensions[0] = screenSize[0];
        characterActiveDimensions[1] = screenSize[1];
        double[] terrainActiveDimensions = new double[2];
        terrainActiveDimensions[0] = characterActiveDimensions[0] + blockSize[0] * 4;
        terrainActiveDimensions[1] = characterActiveDimensions[1] + blockSize[1] * 4;
        this.worldDimensions = new double[2];
        worldDimensions[0] = 4500 + 3000 * levelNumber;
        worldDimensions[1] = 800;
        //this.dimensionsOfRealityBubble = Math.Max(screenSize[0], screenSize[1]) * 2;
        // make the world
        this.world = new World(worldCanvas, screenSize, terrainActiveDimensions);
        // Inform the world who it needs to tell whenever something moves
        this.world.setLoader(this);
        // Setup to hold the unspawned objects
        this.terrainSearcher = new WorldSearcher(2, blockSize, worldDimensions);
        this.characterSearcher = new WorldSearcher(2, blockSize, worldDimensions);

        // calculate the initial reality bubbles 
        WorldBox terrainRegion = new WorldBox(0, terrainActiveDimensions[0], 0, terrainActiveDimensions[1]);
        WorldBox characterRegion = new WorldBox(0, characterActiveDimensions[0], 0, characterActiveDimensions[1]);
        //int i;
        //for (i = 0; i < 2; i++)
        //    existentRegion.enlarge(i, existentRegion.getSize(i));
        this.initialRealityBubble = new RealityBubble(characterRegion, terrainRegion);
        this.realityBubble = this.calculateRealityBubble();
        this.objectsToUnspawn = new System.Collections.Generic.HashSet<GameObject>();
        this.objectsToDeactivate = new System.Collections.Generic.HashSet<GameObject>();
        this.loadedObjects = new System.Collections.Generic.HashSet<GameObject>();


        this.populate(levelNumber);

    }

    // put stuff in the world
    public void populate(int levelNumber)
    {
        // #define DESIGN_HERE
        double[] location = new double[2];
        location[0] = 1000;
        location[1] = 400;
        //this.addItem(new Enemy(location, 2, 3, 4));
        double x;
        double y;
        //return;
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
        Random generator = new Random();
        // add wallpaper
        for (x = 0; x < worldDimensions[0]; x += 1200)
        {
            for (y = 500; y < worldDimensions[1]; y += 1200)
            {
                location = new double[2]; location[0] = x; location[1] = y;
                this.addItem(new Painting(location, 1));
                location[1] = 0;
                this.addItem(new PickupItem(location));
            }
        }
        x = 1000;
#if true
        int numWeapons;
        while (x < worldDimensions[0])
        {
            // add an enemy
            // choose the enemy's location
            x += (800 / worldDimensions[1]) * (900 * generator.NextDouble() + 900) / (levelNumber + 1);
            y = worldDimensions[1] * generator.NextDouble();
            location = new double[2]; location[0] = x; location[1] = y;
            // choose the enemy's type
            type = generator.Next(5);
            // make the enemy
            Enemy tempEnemy = new Enemy(location, type);
            // give the enemy a bunch of weapons
            numWeapons = (int)((levelNumber + 1) / 2);
            for (i = 0; i < numWeapons; i++)
            {
                tempEnemy.addWeapon(new Weapon(generator.Next(16)));
            }
            this.addItem(tempEnemy);
            // give the enemy a painting to look at
            location = new double[2]; location[0] = x; location[1] = y;
            this.addItem(new Painting(location, 0));
        }
#endif
        // add an exit
        location[0] = worldDimensions[0] + 100;
        location[1] = 50;
        Portal exit = new Portal();
        exit.setCenter(location);
        this.addItem(exit);
#if true
        // add platforms
        double spacing = 25;
        for (i = 0; i < 4; i++)
        {
            if (generator.Next(2) == 0)
                spacing *= 2;
        }
        int count = (int)(worldDimensions[0] / spacing);
        for (i = 0; i < count; i++)
        {
            x = generator.NextDouble() * worldDimensions[0];
            y = worldDimensions[1] * generator.NextDouble();
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
        // Currently this is not being used
        /*
        foreach (GameObject o2 in this.objectsToDeactivate)
        {
            this.world.deactivateItem(o2);
        }*/
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
        System.Collections.Generic.List<GameObject> previouslyActive = this.characterSearcher.getObjectsOnlyInA(oldActiveRegion, newActiveRegion);
        foreach (GameObject o2 in previouslyActive)
        {
            //this.world.deactivateItem(o2);
            this.world.removeItem(o2);
        }
        // Find everything that is no longer present
        System.Collections.Generic.List<GameObject> oldItems = this.terrainSearcher.getObjectsOnlyInA(oldExistenceRegion, newExistenceRegion);
        foreach (GameObject o2 in oldItems)
        {
            //this.world.unloadItem(o2);
            this.world.removeItem(o2);
        }
        // Find everything that just became present
        System.Collections.Generic.List<GameObject> newItems = this.terrainSearcher.getObjectsOnlyInA(newExistenceRegion, oldExistenceRegion);
        foreach (GameObject o2 in newItems)
        {
            //this.world.addDisabledItem(o2);
            this.world.addItem(o2);
        }
        // Find everything that just became active
        System.Collections.Generic.List<GameObject> newlyActive = this.characterSearcher.getObjectsOnlyInA(newActiveRegion, oldActiveRegion);
        foreach (GameObject o2 in newlyActive)
        {
            //this.world.activateItem(o2);
            this.world.addItem(o2);
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
        if (o.isMovable())
            this.characterSearcher.itemStartingMove(o);
        else
            this.terrainSearcher.itemStartingMove(o);
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

            /*
            else
            {
                IndexBox activeIndices = this.searcher.getIndexBoxFromWorldBox(this.realityBubble.getActiveRegion());
                if (!objectIndices.intersects(activeIndices))
                {
                    // flag it to deactivate later
                    this.objectsToDeactivate.Add(o);
                }
            }
            */
        }
    }
    // tells whether this character is touching any portans
    public bool characterTouchingPortal(Character character)
    {
        return this.world.characterTouchingPortal(character);
    }
    // performs cleanup on the world so it is essentially gone
    public void destroy()
    {
        this.world.destroy();
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
    //Canvas gameCanvas;
    Canvas worldCanvas;
    World world;
    WorldSearcher characterSearcher;
    WorldSearcher terrainSearcher;
    RealityBubble initialRealityBubble;
    //double[] dimensionsOfRealityBubble;
    double[] blockSize;
    double[] worldDimensions;
    RealityBubble realityBubble;
    System.Collections.Generic.HashSet<GameObject> objectsToUnspawn;
    System.Collections.Generic.HashSet<GameObject> objectsToDeactivate;
    System.Collections.Generic.HashSet<GameObject> loadedObjects;
};