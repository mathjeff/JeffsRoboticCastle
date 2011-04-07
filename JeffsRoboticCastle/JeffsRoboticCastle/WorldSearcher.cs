using System;
using System.Collections;
// The purpose of the WorldSearcher is to quickly find all objects inside an arbitrary region
class WorldSearcher
{
// public
    // constructor, which requires number of dimensions and some estimated sizes
    public WorldSearcher(int dim, double typicalObjectDiameter, double worldSize)
    {
        double[] diameters = new double[2];
        diameters[0] = diameters[1] = typicalObjectDiameter;
        double[] worldDimensions = new double[2];
        worldDimensions[0] = worldDimensions[1] = worldSize;
        this.initialize(dim, diameters, worldDimensions);
    }
    public WorldSearcher(int dim, double[] typicalObjectDiameter, double[] worldSize)
    {
        this.initialize(dim, typicalObjectDiameter, worldSize);
    }

    // Put the item into the world. The location of the item usually may not move until it is removed
    // There are two ways to move an object:
    // Option 1, which is slower, is to remove it, then move it and re-add it
    // Option 2, which only supports moving one object at a time, is to call itemStartingMove and then move it and call itemEndingMove
    public void addItem(GameObject item)
    {
        // add it to the set
        this.items.Add(item);
	    // figure out which boxes contain the bounding box for this item
	    IndexBox boundingBox = this.getIndexRange(item);
	    int i, j, x, y;
	    // for each box, put the item into it
	    for (i = boundingBox.getLowCoordinate(0); i <= boundingBox.getHighCoordinate(0); i++)
	    {
		    // wraparound
		    x = i % this.numBlocks[0];
		    if (x < 0)
			    x += this.numBlocks[0];
		    for (j = boundingBox.getLowCoordinate(1); j <= boundingBox.getHighCoordinate(1); j++)
		    {
			    // wraparound
			    y = j % this.numBlocks[1];
			    if (y < 0)
				    y += this.numBlocks[1];
			    // Now actually add it to this block
#if false
			    if (!(this.worldBlocks[x, y].Contains(item)))
				    this.worldBlocks[x, y].Add(item);
			    else
				    System.Diagnostics.Trace.WriteLine("error, WorldSearcher added an item that was already present");
#else
                if (!(this.worldBlocks[x, y].Contains(item)))
                    this.worldBlocks[x, y].Add(item);
#endif
		    }
	    }
    }

    // Removes an item from the world
    public void removeItem(GameObject item)
    {
        // figure out which boxes contain the bounding box for this item
        IndexBox boundingBox = this.getIndexRange(item);
        this.removeItem(item, boundingBox);
    }
    public void itemStartingMove(GameObject item)
    {
        // When something starts moving, just record where it started moving from
        this.movingItemStartLocation = this.getIndexBoxFromWorldBox(item.getBoundingBox());
    }
    public void itemEndingMove(GameObject item)
    {
        // When something stops moving, check whether it crossed any gridlines
        IndexBox newLocation = this.getIndexBoxFromWorldBox(item.getBoundingBox());
        if (!(newLocation.equals(this.movingItemStartLocation)))
        {
            // If we get here, then it did cross at least one gridline and we need to update it
            this.removeItem(item, this.movingItemStartLocation);
            this.addItem(item);
        }
    }

    /*void moveItem(GameObject item, double[] move)
    {
    }*/
    // Finds all objects in the rectangle
    public System.Collections.Generic.List<GameObject> getObjectsInWorldBox(WorldBox worldBox)
    {
	    System.Collections.Generic.List<GameObject> resultantList = new System.Collections.Generic.List<GameObject>();
	    IndexBox indexBox = this.getIndexBoxFromWorldBox(worldBox);
	    return this.getObjectsInIndexBox(indexBox);
    }
    // Snap the coordinates to multiples of the box size
    public IndexBox getIndexBoxFromWorldBox(WorldBox worldBox)
    {
	    int x1 = (int)(worldBox.getLowCoordinate(0) / this.blockSize[0]);
	    int x2 = (int)(worldBox.getHighCoordinate(0) / this.blockSize[0]);
	    int y1 = (int)(worldBox.getLowCoordinate(1) / this.blockSize[1]);
        int y2 = (int)(worldBox.getHighCoordinate(1) / this.blockSize[1]);
	    //Rectangle indexRect(x1, y1, x2 - x1, y2 - y1);
	    IndexBox indexBox = new IndexBox(x1, x2, y1, y2);
	    return indexBox;
    }
    public bool indexBoxContainsCoordinates(IndexBox indexBox, int[] coordinates)
    {
        int i;
        int difference;
        int modValue;
        for (i = 0; i < coordinates.Length; i++)
        {
            difference = coordinates[i] - indexBox.getLowCoordinate(i);
            modValue = difference % this.numBlocks[i];
            if (modValue < 0)
                modValue += this.numBlocks[i];
            if (modValue > indexBox.getHighCoordinate(i) - indexBox.getLowCoordinate(i))
                return false;
        }
        return true;
    }

    // given an item that wants to make the given move, find everything that may collide with it
    public System.Collections.Generic.List<GameObject> getCollisions(GameObject item, double[] move)
    {
        if (move == null)
            return this.getCollisions(item);
	    System.Collections.Generic.List<GameObject> intersections = new System.Collections.Generic.List<GameObject>();
	    // find the bounding box for this move
	    IndexBox boundingIndices = this.getIndexRange(item, move);
	    // find the objects inside this box
	    intersections = this.getObjectsInIndexBox(boundingIndices);
	    return intersections;
    }
    // given an item, find everything currently overlapping it
    public System.Collections.Generic.List<GameObject> getCollisions(GameObject item)
    {
        System.Collections.Generic.List<GameObject> intersections = new System.Collections.Generic.List<GameObject>();
        // find the bounding box for this move
        IndexBox boundingIndices = this.getIndexRange(item);
        // find the objects inside this box
        intersections = this.getObjectsInIndexBox(boundingIndices);
        return intersections;
    }
    public System.Collections.Generic.List<GameObject> getObjectsOnlyInA(WorldBox boxA, WorldBox boxB)
    {
        System.Collections.Generic.List<GameObject> results = new System.Collections.Generic.List<GameObject>();
        System.Collections.Generic.HashSet<GameObject> localItems;
        IndexBox box1 = this.getIndexBoxFromWorldBox(boxA);
        IndexBox box2 = this.getIndexBoxFromWorldBox(boxB);
        int[] coordinates = new int[2];
        int[] indices = new int[2];
        //int i;
        IndexBox itemIndexBox;
        //GameObject currentItem;
        // Check each box inside the larger indexbox
        for (coordinates[0] = box1.getLowCoordinate(0); coordinates[0] <= box1.getHighCoordinate(0); coordinates[0]++)
        {
            // wraparound
            indices[0] = coordinates[0] % this.numBlocks[0];
            if (indices[0] < 0)
                indices[0] += this.numBlocks[0];
            for (coordinates[1] = box1.getLowCoordinate(1); coordinates[1] <= box1.getHighCoordinate(1); coordinates[1]++)
            {
                // wraparound
                indices[1] = coordinates[1] % this.numBlocks[1];
                if (indices[1] < 0)
                    indices[1] += this.numBlocks[1];
                // Make sure that the current point isn't inside the smaller index box
                if (!(box2.contains(coordinates)))
                {
                    // Iterate over each item inside the box
                    localItems = this.worldBlocks[indices[0], indices[1]];
                    foreach (GameObject currentItem in localItems)
                    {
                        //currentItem = localItems[i];
                        itemIndexBox = this.getIndexBoxFromWorldBox(currentItem.getBoundingBox());
                        // Make sure that the item does intersect the larger box but not the smaller index box
                        if (itemIndexBox.intersects(box1) && !(itemIndexBox.intersects(box2)))
                        {
                            // Now add it to the resultant list if needed
                            if (!results.Contains(currentItem))
                            {
                                results.Add(currentItem);
                            }
                        }
                    }
                }
            }
        }
        return results;
    }

    /////////////////////////////////////////////////// Private member functions of WorldSearcher ///////////////////////////////////////////////////////////////////////

    // initializer
    void initialize(int dim, double[] typicalObjectDimensions, double[] worldSize)
    {
        if (dim != 2)
        {
            System.Diagnostics.Trace.WriteLine("The WorldSearcher is currently configured to handle only 2 dimensions");
            return;
        }
        // figure out how large our array will be to hold everything
        this.numDimensions = dim;
        this.blockSize = new double[dim];
        this.numBlocks = new int[dim];
        int i, j;
        for (i = 0; i < dim; i++)
        {
            this.numBlocks[i] = (int)(Math.Ceiling(worldSize[i] / typicalObjectDimensions[i]));
            this.blockSize[i] = worldSize[i] / this.numBlocks[i];
        }
        // allocate memory for the items in the world
        this.worldBlocks = new System.Collections.Generic.HashSet<GameObject>[numBlocks[0], numBlocks[1]];
        for (i = 0; i < worldBlocks.GetLength(0); i++)
        {
            for (j = 0; j < worldBlocks.GetLength(1); j++)
            {
                worldBlocks[i, j] = new System.Collections.Generic.HashSet<GameObject>();
            }
        }
        // Also create a set to hold the items in the world, to speed up the case where the world is sparse
        this.items = new System.Collections.Generic.HashSet<GameObject>();
    }

    // Get a hyper-box that bounds the item in all dimension
    private IndexBox getIndexRange(GameObject item)
    {
	    WorldBox boundingBox = item.getBoundingBox();
	    IndexBox indexBox = this.getIndexBoxFromWorldBox(boundingBox);
	    return indexBox;
    }
    // Get a hyper-box that bounds the item in all dimension at all points during the given move
    private IndexBox getIndexRange(GameObject item, double[] move)
    {
	    WorldBox boundingBox = item.getBoundingBoxForMove(move);
	    IndexBox indexBox = this.getIndexBoxFromWorldBox(boundingBox);
	    return indexBox;
    }
    // Return all objects that would overlap this set of boxes indicated by the index rectangle
    private System.Collections.Generic.List<GameObject> getObjectsInIndexBox(IndexBox indexBox)
    {
        if (indexBox.countNumBlocks() < this.items.Count)
        {
            return this.searchWorldForIndexBox(indexBox);
        }
        else
        {
            return this.searchItemsForIndexBox(indexBox);
        }
    }
    // Uses the worldBlocks array to speed up the search for objects inside the given index box. This is good when the box is small
    private System.Collections.Generic.List<GameObject> searchWorldForIndexBox(IndexBox indexBox)
    {
        System.Collections.Generic.List<GameObject> resultantList = new System.Collections.Generic.List<GameObject>();
        System.Collections.Generic.HashSet<GameObject> localItems;
        //GameObject tempItem;
        int x1 = (int)(indexBox.getLowCoordinate(0));
		int x2 = (int)(indexBox.getHighCoordinate(0));
		int y1 = (int)(indexBox.getLowCoordinate(1));
		int y2 = (int)(indexBox.getHighCoordinate(1));
	    int i, j, x, y;
	    // check each block inside the bounding box
	    for (i = x1; i <= x2; i++)
	    {
		    // wraparound
		    x = i % this.numBlocks[0];
		    if (x < 0)
			    x += this.numBlocks[0];
		    for (j = y1; j <= y2; j++)
		    {
			    // wraparound
			    y = j % this.numBlocks[1];
			    if (y < 0)
				    y += this.numBlocks[1];
			    // check each item in the block
			    localItems = this.worldBlocks[x, y];
			    foreach (GameObject currentItem in localItems)
			    {
				    //tempItem = localItems[k];
				    // make sure the item isn't there already
				    if (!resultantList.Contains(currentItem))
				    {
					    //Now we store it in the list of collisions
					    resultantList.Add(currentItem);
				    }
			    }
		    }
	    }	
	    return resultantList;
    }
    // Searches the items set for objects in the given box. This is good where there are not many objects in the world
    private System.Collections.Generic.List<GameObject> searchItemsForIndexBox(IndexBox indexBox)
    {
        System.Collections.Generic.List<GameObject> collisions = new System.Collections.Generic.List<GameObject>();
        foreach (GameObject candidate in this.items)
        {
            if (this.getIndexBoxFromWorldBox(candidate.getBoundingBox()).intersects(indexBox))
            {
                collisions.Add(candidate);
            }
        }
        return collisions;
    }

    // Removes an item from the world, which was previously found in the indexBox
    void removeItem(GameObject item, IndexBox boundingBox)
    {
        // remove it from the set
        this.items.Remove(item);
        int i, j, x, y;
        // iterate over each box
        for (i = boundingBox.getLowCoordinate(0); i <= boundingBox.getHighCoordinate(0); i++)
        {
            // wraparound
            x = i % this.numBlocks[0];
            if (x < 0)
                x += this.numBlocks[0];
            for (j = boundingBox.getLowCoordinate(1); j <= boundingBox.getHighCoordinate(1); j++)
            {
                // wraparound
                y = j % this.numBlocks[1];
                if (y < 0)
                    y += this.numBlocks[1];
                // Now actually remove it from this block
#if false
                if (this.worldBlocks[x, y].Contains(item))
                    this.worldBlocks[x, y].Remove(item);
                else
                    System.Diagnostics.Trace.WriteLine("error, WorldSearcher tried to remove an item that was not present");
#else
                if (this.worldBlocks[x, y].Contains(item))
                    this.worldBlocks[x, y].Remove(item);
#endif
            }
        }

    }

private
	int numDimensions;
	double[] blockSize;
	int[] numBlocks;
	System.Collections.Generic.HashSet<GameObject>[,] worldBlocks; // tells which items are present in each block
    System.Collections.Generic.HashSet<GameObject> items;          // gives all items known to this WorldSearcher
    IndexBox movingItemStartLocation;
    //GameObject movingItem;

};