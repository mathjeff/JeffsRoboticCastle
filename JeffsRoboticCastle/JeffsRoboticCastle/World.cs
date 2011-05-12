using System.Windows.Shapes;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

class World
{
// public
    // constructor
	public World(Canvas newCanvas, double[] screenSize, double[] sizeOfRealityBubble)
    {
        this.canvas = newCanvas;
        this.objects = new System.Collections.Generic.List<GameObject>();
        this.projectiles = new System.Collections.Generic.List<Projectile>();
        this.explosions = new System.Collections.Generic.List<Explosion>();
        this.characters = new System.Collections.Generic.List<Character>();

        this.dimensionsOfRealityBubble = sizeOfRealityBubble;
        // the first index into the WorldSearcher array is the team number
        // the second index into the WorldSearcher array is the type: {0 = Characters and platforms, 1 = projectiles, 2 = explosions, 3 = ghost}
        this.searchers = new WorldSearcher[3, 4];
        int i, j;
        double[] typicalDimensions = new double[2];
        typicalDimensions[0] = typicalDimensions[1] = 100;
        // projectiles are usually few in number, so use smaller boxes for optimizing projectiles
        double[] typicalProjectileDimensions = new double[2];
        typicalProjectileDimensions[0] = typicalProjectileDimensions[1] = 50;
        for (i = 0; i < 3; i++)
        {
            for (j = 0; j < 4; j++)
            {
                if (j == 1)
                {
                    // projectiles are usually few in number,
                    // so the only case where the gridlines help much are where we have lots of projectiles all next to each other
                    // In that case, we want small boxes to best optimize the worst case
                    this.searchers[i, j] = new WorldSearcher(2, typicalProjectileDimensions, dimensionsOfRealityBubble);
                }
                else
                {
                    this.searchers[i, j] = new WorldSearcher(2, typicalDimensions, dimensionsOfRealityBubble);
                }
            }
        }
        // terrain is usually spaced out further, so use larger boxes for optimizing terrain
        double[] typicalTerrainDimensions = new double[2];
        typicalTerrainDimensions[0] = typicalTerrainDimensions[1] = 200;
        this.searchers[0, 0] = new WorldSearcher(2, typicalTerrainDimensions, dimensionsOfRealityBubble);
        this.worldImageTransform = new MatrixTransform();
        this.camera = new Camera(new WorldBox(0, screenSize[0], 0, screenSize[1]), new WorldBox(0, screenSize[0], 0, screenSize[1]));
        this.camera.saveTransformIn(worldImageTransform);
        //this.worldImageTransform.Matrix.Translate(0, 200);
        //this.worldImageTransform.Matrix.Skew(0, 30);
        //this.worldImageTransform.Matrix = new Matrix(1, 0, 2, 1, 0, 800);

        //this.worldImageTransform = new TranslateTransform(0, -200);
    }
    public void setLoader(WorldLoader newLoader)
    {
        this.loader = newLoader;
    }

    // Adds the item to the world. This is the usual way for outside code to add something to the world
    public void addItem(GameObject item)
    {
        if (item.isACharacter())
        {
            this.addCharacter((Character)item);
            return;
        }
        if (item.isAProjectile())
        {
            this.addProjectile((Projectile)item);
            return;
        }
        if (item.isAnExplosion())
        {
            this.addExplosion((Explosion) item);
            return;
        }
        if (item.isAPlatform())
        {
            this.addPlatform((Platform) item);
            return;
        }
        if (item.isAGhost())
        {
            this.addGhost((Ghost)item);
            return;
        }
        this.addObject(item);
    }
    // adds the given object to the world, but only as an image and not as an object that interacts
    public void addObject(GameObject o)
    {
        this.addDisabledObject(o);
        this.activateObject(o);
    }

    // adds the given object of the specified type to the world
    public void addCharacter(Character c)
    {
        this.addDisabledCharacter(c);
        this.activateCharacter(c);
    }
    public void addProjectile(Projectile p)
    {
        this.addDisabledProjectile(p);
        this.activateProjectile(p);
    }
    public void addExplosion(Explosion e)
    {
        this.addDisabledExplosion(e);
        this.activateExplosion(e);
    }
    public void addPlatform(Platform p)
    {
        this.addDisabledPlatform(p);
        this.activateObject(p);
    }
    public void addGhost(Ghost g)
    {
        this.addDisabledGhost(g);
        this.activateGhost(g);
    }

    // add an object without allowing the passage of time
    public void addDisabledCharacter(Character c)
    {
        // save the character in the searcher for projectiles
        this.searchers[c.getTeamNum(), 0].addItem(c);
        // inform the character of the world that it is in
        c.setWorld(this);

        this.addDisabledObject(c);
    }
    public void addDisabledProjectile(Projectile p)
    {
        // save the projectile in the searcher for projectiles
        this.searchers[p.getTeamNum(), 1].addItem(p);
        
        this.addDisabledObject(p);
    }
    public void addDisabledExplosion(Explosion e)
    {
        // save the explosion in the searcher for explosions
        this.searchers[e.getTeamNum(), 2].addItem(e);
        
        this.addDisabledObject(e);
    }
    public void addDisabledPlatform(Platform p)
    {
        // save the platform in the searcher for platforms and characters
        this.searchers[p.getTeamNum(), 0].addItem(p);

        this.addDisabledObject(p);
    }
    public void addDisabledGhost(Ghost g)
    {
        // save the explosion in the searcher for ghosts
        this.searchers[g.getTeamNum(), 3].addItem(g);

        this.addDisabledObject(g);
    }
    public void addDisabledObject(GameObject o)
    {
        // add the common transform so we can shift the screen all at once
        Image image = o.getImage();
        TransformGroup transforms = new TransformGroup();
        transforms.Children.Add(o.getRenderTransform());
        transforms.Children.Add(this.worldImageTransform);
        image.RenderTransform = transforms;
        // add the image to the screen
        canvas.Children.Add(image);
    }
    public void addDisabledItem(GameObject item)
    {
        if (item.isACharacter())
        {
            this.addDisabledCharacter((Character)item);
            return;
        }
        if (item.isAProjectile())
        {
            this.addDisabledProjectile((Projectile)item);
            return;
        }
        if (item.isAnExplosion())
        {
            this.addDisabledExplosion((Explosion)item);
            return;
        }
        if (item.isAPlatform())
        {
            this.addDisabledPlatform((Platform)item);
            return;
        }
        if (item.isAGhost())
        {
            this.addGhost((Ghost)item);
            return;
        }
        this.addDisabledObject(item);
    }

    // tells the world that time is allowed to pass for this object
    public void activateItem(GameObject item)
    {
        if (item.isACharacter())
        {
            this.activateCharacter((Character)item);
            return;
        }
        if (item.isAProjectile())
        {
            this.activateProjectile((Projectile)item);
            return;
        }
        if (item.isAnExplosion())
        {
            this.activateExplosion((Explosion)item);
            return;
        }
        /*if (item.isAPlatform())
        {
            this.activatePlatform((Platform)item);
            return;
        }*/
        this.activateObject(item);
    }
    public void activateCharacter(Character c)
    {
        // save the character in the list of characters
        if (this.characters.Contains(c))
            System.Diagnostics.Trace.WriteLine("Error: world tried to activate a projectile that was already active");
        else
            this.characters.Add(c);
        this.activateObject(c);
    }
    public void activateProjectile(Projectile p)
    {
        // save the projectile to the list of projectiles
        if (this.projectiles.Contains(p))
            System.Diagnostics.Trace.WriteLine("Error: world tried to activate a projectile that was already active");
        else
            this.projectiles.Add(p);
        this.activateObject(p);
    }
    public void activateExplosion(Explosion e)
    {
        // save the explosion in the searcher for explosions
        if (this.explosions.Contains(e))
            System.Diagnostics.Trace.WriteLine("Error: world tried to activate an explosion that was already active");
        else
            this.explosions.Add(e);
        this.activateObject(e);
    }
    public void activateGhost(Ghost g)
    {
        // we don't ever need to iterate over all the ghosts so we don't store a separate list of them
        activateObject(g);
    }
    public void activateObject(GameObject o)
    {
        if (this.objects.Contains(o))
            System.Diagnostics.Trace.WriteLine("Error: world tried to activate an object that was already active");
        else
            this.objects.Add(o);
        //o.activations.Add(1);
    }

    // tells the world that time is no longer allowed to pass for this object
    public void deactivateItem(GameObject o)
    {
        if (o.isACharacter())
        {
            Character converted = (Character)o;
            if (!this.characters.Contains(converted))
                System.Diagnostics.Trace.WriteLine("Error: World tried to deactivate a character that was not active");
            else
                this.characters.Remove(converted);
        }
        if (o.isAProjectile())
        {
            //System.Diagnostics.Trace.WriteLine("deactivating projectile at " + o.getCenter()[0].ToString() + "," + o.getCenter()[1].ToString());
            Projectile converted = (Projectile)o;
            if (!this.projectiles.Contains(converted))
                System.Diagnostics.Trace.WriteLine("Error: World tried to deactivate a projectile that was not active");
            else
                this.projectiles.Remove(converted);
        }
        if (o.isAnExplosion())
        {
            Explosion converted = (Explosion)o;
            if (!this.explosions.Contains(converted))
                System.Diagnostics.Trace.WriteLine("Error: World tried to deactivate an explosion that was not active");
            else
                this.explosions.Remove(converted);
        }
        if (!this.objects.Contains(o))
            System.Diagnostics.Trace.WriteLine("Error: World tried to deactivate an object that was not active");
        else
            this.objects.Remove(o);
        //o.activations.Add(0);
    }

    // Tells the world to remove the object from the world for now
    // The world loader still remembers the object, so it may come back later if the reality bubble moves back
    public void unloadItem(GameObject item)
    {
        if (item.isACharacter())
        {
            this.unloadCharacter((Character)item);
            return;
        }
        if (item.isAProjectile())
        {
            this.unloadProjectile((Projectile)item);
            return;
        }
        if (item.isAnExplosion())
        {
            this.unloadExplosion((Explosion)item);
            return;
        }
        if (item.isAPlatform())
        {
            this.unloadPlatform((Platform)item);
            return;
        }
        if (item.isAPickupItem())
        {
            this.unloadPickupItem((PickupItem)item);
            return;
        }
        this.unloadObject(item);
    }
    // tells the world to get rid of this object
    public void removeItem(GameObject item)
    {
        this.deactivateItem(item);
        this.unloadItem(item);
    }

    public WorldBox getVisibleWorldBox()
    {
        return this.camera.getWorldBox();
    }

    public void removeCharacter(Character c)
    {
        // remove it from the world
        this.unloadCharacter(c);
        // tell the world loader not to bring it back
        this.removingObject(c);
    }
    public void unloadCharacter(Character c)
    {
        // remove it from the searcher for characters and platforms
        this.searchers[c.getTeamNum(), 0].removeItem(c);
        // remove it from the list of characters
        this.characters.Remove(c);
        // remove the image
        this.unloadObject(c);
    }
    public void removePlatform(Platform p)
    {
        // remove it from the world
        this.unloadPlatform(p);
        // tell the world loader not to bring it back
        this.removingObject(p);
    }
    public void unloadPlatform(Platform p)
    {
        // remove it from the searcher for characters and platforms
        this.searchers[p.getTeamNum(), 0].removeItem(p);
        // remove the object
        this.unloadObject(p);
    }
    public void removeProjectile(Projectile p)
    {
        // remove it from the world
        this.unloadProjectile(p);
        // tell the world loader not to bring it back
        this.removingObject(p);
    }
    public void unloadProjectile(Projectile p)
    {
        // remove the projectile from the searcher for projectiles
        this.searchers[p.getTeamNum(), 1].removeItem(p);
        // remove the projectile from the list of projectiles
        this.projectiles.Remove(p);
        // remove the object
        this.unloadObject(p);
    }
    public void removeExplosion(Explosion e)
    {
        // remove it from the world
        this.unloadExplosion(e);
        // tell the world loader not to bring it back
        this.removingObject(e);
    }
    public void unloadExplosion(Explosion e)
    {
        // remove it from the searcher for explosions
        this.searchers[e.getTeamNum(), 2].removeItem(e);
        // remove it from the list of explosions
        this.explosions.Remove(e);
        // remove the object
        this.unloadObject(e);
    }
    public void removePickupItem(PickupItem i)
    {
        // remove it from the world
        this.unloadPickupItem(i);
        // tell the world loader not to bring it back
        this.removingObject(i);
    }
    public void unloadPickupItem(PickupItem item)
    {
        // remove it from the searcher
        this.searchers[item.getTeamNum(), 3].removeItem(item);
        // remove the image
        this.unloadObject(item);
    }

    public int getNumExplosions()
    {
        return this.explosions.Count;
    }
    // RemoveObject gets rid of the object forever, but only gets rid of attributes pertaining specific to the GameObject class
    public void removeObject(GameObject o)
    {
        // remove it from the world
        this.unloadObject(o);
        this.removingObject(o);
    }
    // unloadObject removes it from the world but it may come back if the reality bubble moves back
    public void unloadObject(GameObject o)
    {
        this.objects.Remove(o);
        //o.activations.Add(2);
        canvas.Children.Remove(o.getImage());
    }
    // This function gets called when an object is being removed
    public void removingObject(GameObject o)
    {
        // tell the world loader that it's being removed
        if (this.loader != null)
            this.loader.removingObject(o);
    }
    public void scrollTo(GameObject o)
    {
        if (this.camera.scrollTo(o))
            this.camera.saveTransformIn(this.worldImageTransform);
    }
    public void timerTick(double numSeconds)
    {
        // Must process AI before clearing collision flags because we only let it jump if it's on the ground
        // Must clear collision flags before moving because moving will set the collision flags again
        // Should have projectiles find targets before moving
        // Should process explosions before processing death beacause explosions can cause death
        // Must process explosions before exploding projectiles because the explosions can explode other projectiles
        // Should process explosions before moving characters so that a detonated projectile will always do damage

        this.projectilesFindTargets();
        this.processAIs();
       
        this.clearCollisionFlags();

        this.processExplosions(numSeconds);

        this.moveCharacters(numSeconds);
        this.moveProjectiles(numSeconds);
        
        this.spawnProjectiles(numSeconds);        
        
        this.processDeath(numSeconds);
        this.applyContactDamage(numSeconds);
        this.explodeProjectiles(numSeconds);

        this.pickupPowerups(numSeconds);
    }
    // this function tells whether the chararacter is touching a portal, to determine when the character leaves the world
    public bool characterTouchingPortal(Character character)
    {
        CollisionRequest request = new CollisionRequest();
        request.setObject(character);
        request.setRequestGaia(true);
        request.setRequestGhosts(true);
        List<GameObject> collisions = this.findCollisions(request);
        if (collisions.Count > 0)
        {
            foreach (GameObject collision in collisions)
            {
                if (collision.isAPortal() && collision.intersects(character))
                    return true;
            }
        }
        return false;
    }
    //
    public void destroy()
    {
        int i;
        GameObject item;
        for (i = objects.Count - 1; i >= 0; i--)
        {
            item = objects[i];
            this.deactivateItem(item);
            this.unloadItem(item);
        }
    }
    // Find the closest object to a certain object
    public GameObject findNearestObject(CollisionRequest request)
    {
        System.Collections.Generic.List<GameObject> candidates = null;
        GameObject o = request.getObject();
        bool hasAHint = request.hasASizeHint();
        if (hasAHint)
        {
            // If there is a hint, then we know the closest object can not be any further than the hint is
            // Search for all the collisions in this box
            candidates = this.findCollisions(request);
            if (candidates.Count == 0)
            {
                // It's possible that the object was recently removed from the world, and that now there are no relevant object in the world.
                // We didn't want to ignore the hint because it still gives a good idea about the size to search.
                // However, if we didn't find anything inside this box, then we should now search the rest of the world for something
                // Setting the hint to false will cause us to go back and search a much larger area
                hasAHint = false;
            }
        }
        // If we have no idea where to find it, then search all over
        if (!hasAHint)
        {
            GameObject collisionBox = new GameObject();
            collisionBox.setCenter(o.getCenter());
            collisionBox.setShape(new GameRectangle(5000, 5000));
            collisionBox.setTeamNum(o.getTeamNum());
            request.setObject(collisionBox);
            // find all objects of the appropriate type
            candidates = this.findCollisions(request);
        }
        return o.findClosest(candidates);
    }
// private
    void clearCollisionFlags()
    {
        foreach (Projectile p in this.projectiles)
        {
            p.setColliding(null);
            p.setCollisionLocation(null);
        }
        foreach (Character c in this.characters)
        {
            c.setColliding(null);
            c.setCollisionLocation(null);
        }
        /*
        foreach (GameObject o in this.objects)
        {
            o.setColliding(null);
            o.setCollisionLocation(null);
        }
        */
    }
    void clearStunFlags()
    {
        foreach (GameObject o in this.objects)
        {
            o.resetTimeMultiplier();
        }
    }

    // Have the enemies decide what to do
    void processAIs()
    {
        // first, find the necessary data to provide to each AI first

        int i;
        //System.Diagnostics.Trace.WriteLine("Number of characters in the world = " + this.characters.Count.ToString());
        for (i = 0; i < this.characters.Count; i++)
        {
            //if (characters[i] != this.player)
            {
                //characters[i].reinforce(0.01);
                characters[i].think();
                characters[i].adjustBehavior();
            }
        }
    }
    // Have each projectile find a target
    void projectilesFindTargets()
    {
        GameObject target;
        foreach (Projectile p in this.projectiles)
        {
            if (p.needsATarget())
            {
                CollisionRequest request = new CollisionRequest();
                request.setObject(p);
                request.setRequestEnemies(true);
                request.setRequestCharacters(p.shouldHomeOnCharacters());
                request.setRequestProjectiles(p.shouldHomeOnProjectiles());
                request.growToTouch(p.getTarget());
                target = this.findNearestObject(request);
                if ((target != null) && target.isACharacter())
                {
                    // tell the character that it's doing a good job for not being hit by the projectile
                    ((Character)target).reinforce(0.01);
                }
                p.setTarget(target);
            }
        }
    }

    // search for a target for the given projectile
    /*GameObject findTargetFor(Projectile p)
    {
        System.Collections.Generic.List<GameObject> candidates;
        if (p.getTarget() == null)
        {
            // If it doesn't yet have a target, then we have little idea about where the closest target may be.
            // So, for now, we search far and wide for it
            GameObject collisionBox = new GameObject();
            collisionBox.setCenter(p.getCenter());
            collisionBox.setShape(new GameRectangle(5000, 5000));
            collisionBox.setTeamNum(p.getTeamNum());
            //candidates = this.findEnemyCollisions(collisionBox);
            candidates = this.findCollisions(collisionBox, false, false, true, null, true, true, false);
        }
        else
        {
            // If it already has a target, then the closest possible target shouldn't be much closer than the current target
            // So, make a box with radius equal to the distance and search for things inside it
            // First find the distance to the current target
            GameObject collisionBox = new GameObject();
            collisionBox.setCenter(p.getCenter());
            collisionBox.setShape(new GameRectangle(0, 0));
            collisionBox.setTeamNum(p.getTeamNum());
            double dist = collisionBox.distanceTo(p.getTarget());
            // Increase the radius appropriately
            collisionBox.setShape(new GameRectangle(p.getShape().getWidth() + 2 * dist, p.getShape().getHeight() + 2 * dist));
            // Search for all the collisions in this box
            //candidates = this.findEnemyCollisions(collisionBox);
            candidates = this.findCollisions(collisionBox, false, false, true, null, true, true, false);
        }
        if (candidates.Count == 0)
        {
            // If we can't find any targets anywhere, then return that we can't find any targets
            return null;
        }
        // Now we've found a bunch of possible targets. Search for the closest
        return p.findClosest(candidates);
    }*/
    void moveCharacters(double numSeconds)
    {
	    // move objects
        foreach (Character c in this.characters)
        {
            this.moveObject(c, numSeconds);
        }
    }
    void moveProjectiles(double numSeconds)
    {
        foreach (Projectile p in this.projectiles)
        {
            this.moveObject(p, numSeconds);
        }
    }
    // create any new projectiles from the weapons that characters are firing
    void spawnProjectiles(double numSeconds)
    {
	    // spawn new projectiles
	    System.Collections.ArrayList projectiles = null;
	    foreach (Character tempCharacter in this.characters)
	    {
		    projectiles = tempCharacter.getProjectiles(numSeconds * tempCharacter.getTimeMultiplier());
            foreach (Projectile tempProjectile in projectiles)
            {
                //if (this.loader != null)
                //    this.loader.addItem(tempProjectile);
                //else
                // create the projectile
                this.addProjectile(tempProjectile);
                // figure out whether it has to explode right away
                this.checkDetonation(tempProjectile);
            }
	    }
    }
    // Explode any projetiles that are colliding or spent
    void explodeProjectiles(double numSeconds)
    {
        int i;
	    Projectile p;
        // Explode any projectile that is colliding with something
	    for (i = this.projectiles.Count - 1; i >= 0; i--)
	    {
		    p = projectiles[i];
		    p.timerTick(numSeconds);
		    if (p.doneFlying() || p.isColliding())
		    {
			    this.explode(p);
		    }
	    }
    }
    // Explosions deal damage and may disappear
    void processExplosions(double numSeconds)
    {
        int i, j;
        // for each explosion, figure out what's nearby
        System.Collections.Generic.List<GameObject> collisions = new System.Collections.Generic.List<GameObject>();
        for (i = explosions.Count - 1; i >= 0; i--)
        {
            // find all enemy characters and platforms
            CollisionRequest request = new CollisionRequest();
            request.setObject(explosions[i]);
            request.setRequestAllies(explosions[i].isFriendlyFireEnabled());
            request.setRequestGaia(true);
            request.setRequestEnemies(true);
            request.setRequestCharacters(true);
            collisions = this.findCollisions(request);
            //collisions = this.findEnemyCharacters(explosions[i]);
            for (j = 0; j < collisions.Count; j++)
            {
                if (explosions[i].intersects(collisions[j]))
                {
                    // set a flag to deal damage
                    explosions[i].interactWith(collisions[j], numSeconds);
                    /*// It's possible that this explosion appeared recently around the enemy projectile.
                    // If that happens, we explode any such projectile
                    if (collisions[j].isAProjectile())
                        this.explodeProjectile((Projectile)collisions[j]);*/
                }
            }
            // when the explosion first appears, it can trigger other projectiles to explode
            if (explosions[i].isNew())
            {
                request = new CollisionRequest();
                request.setObject(explosions[i]);
                request.setRequestAllies(explosions[i].isFriendlyFireEnabled());
                request.setRequestEnemies(true);
                request.setRequestProjectiles(true);
                collisions = this.findCollisions(request);
                //collisions = this.findCollisions(explosions[i], explosions[i].isFriendlyFireEnabled(), false, true, null, false, true, false, false);
                for (j = 0; j < collisions.Count; j++)
                {
                    if (explosions[i].intersects(collisions[j]))
                    {
                        //this.explode((Projectile)collisions[j]);
                        // mark the projectile as colliding with this explosion
                        collisions[j].setColliding(explosions[i]);
                    }
                }
            }
        }
        // for each character, receive damage from each applicable stun
        foreach (Character tempCharacter in this.characters)
        {
            tempCharacter.processStuns(numSeconds);
        }
	    // remove explosions that are done
	    for (i = this.explosions.Count - 1; i >= 0; i--)
	    {
		    if (explosions[i].isFinished(numSeconds))
		    {
			    this.removeExplosion(explosions[i]);
		    }
	    }
    }
    // have each character take damage for touching anything dangerous
    void applyContactDamage(double numSeconds)
    {
        GameObject collision;
        double damage;
        foreach (Character c in this.characters)
        {
            collision = c.getCollision();
            if (collision != null)
            {
                if (collision.getTeamNum() != c.getTeamNum())
                {
                    // An object can only run into one thing, but several things can run into it
                    // Anything that runs into it will do damage
                    // So the thing we collide with is what takes damage
                    damage = c.getContactDamagePerSecond();
                    collision.takeDamage(damage, numSeconds);
                }
            }
        }
    }
    // remove any dead characters from the world
    void processDeath(double numSeconds)
    {
        int i;
        for (i = this.characters.Count - 1; i >= 0; i--)
        {
            if (!characters[i].isAlive())
            {
                // create an ammo box on the character's location
                this.addItem(new PickupItem(characters[i].getCenter()));
                // get rid of the character
                removeCharacter(characters[i]);
            }
        }
    }
    // have each character pick up any powerups that he or she is touching
    void pickupPowerups(double numSeconds)
    {
        System.Collections.Generic.List<GameObject> collisions;
        foreach (Character character in this.characters)
        {
            CollisionRequest request = new CollisionRequest();
            request.setObject(character);
            request.setRequestAllies(true);
            request.setRequestGaia(true);
            request.setRequestGhosts(true);
            //collisions = this.findCollisions(character, true, true, false, null, false, false, false, true);
            collisions = this.findCollisions(request);
            foreach (GameObject collision in collisions)
            {
                if (collision.isAPickupItem() && character.intersects(collision))
                {
                    character.refillSomeAmmo();
                    this.removePickupItem((PickupItem)collision);
                }
            }
        }
    }


    // Moves the object according to its velocity. Also figure out if it's colliding with anything
    void moveObject(GameObject o, double numSeconds)
    {
	    // update velocity based on accelerations
        numSeconds *= o.getTimeMultiplier();
        if (numSeconds == 0)
            return;
	    o.updateVelocity(numSeconds);

	    // If it's trying to move, then go through with all the collision detection
	    if ((o.getVelocity()[0] != 0) || (o.getVelocity()[1] != 0))
	    {
            // tell the searcher that we're about to move it
            this.getSearcherForObject(o).itemStartingMove(o);
            // Inform the world loader that it's about to move
            if (this.loader != null)
                this.loader.objectStartingMove(o);
            double[] move = o.getMove(numSeconds);
            
		    // Collision detection with the ground
            if (move[1] + o.getCenter()[1] <= o.getShape().getHeight() / 2)
            {
                if ((!o.isAProjectile()) || (o.getCenter()[1] > o.getShape().getHeight() / 2))
                {
                    // for now we just say it's colliding with itself
                    o.setColliding(o);
                    double[] collisionMove = new double[2];
                    collisionMove[0] = move[0];
                    collisionMove[1] = -(o.getCenter()[1] - o.getShape().getHeight() / 2);
                    // tell it where the collision occurs
                    o.setCollisionMove(collisionMove);
                    // anything other than a projectile has to stop once it hits the ground
                    if (!o.isAProjectile())
                        move = collisionMove;
                }
            }

		    // find obstacles that may collide with it
            System.Collections.Generic.List<GameObject> collisions;
            double[] newMove;
            if (o.isAProjectile())
            {
                this.checkDetonationForMove((Projectile)o, move);
            }
            else
            {
                // The current object isn't a projectile.
                // Look for characters and platforms that collide
                //collisions = this.getObjectsInTheWay(o, move);
                CollisionRequest request = new CollisionRequest();
                request.setObject(o);
                request.setCollisionMove(move);
                request.setRequestAllies(true);
                request.setRequestGaia(true);
                request.setRequestEnemies(true);
                request.setRequestCharacters(true);
                //collisions = this.findCollisions(o, true, true, true, move, true, false, false, false);
                collisions = this.findCollisions(request);
                GameObject collision = null;
                // make sure that there actually are collisions to check
                if (collisions.Count > 0)
                {
                    GameObject other;
                    int lastChangedIndex = collisions.Count - 1;
                    int i;
                    i = 0;
                    // decide how far it moves before it gets stuck
                    while (true)
                    {
                        other = collisions[i];
                        newMove = this.movementForCollision(o, other, move);    // physics collisions handling
                        // Yes, this is a pointer comparision and not a value comparision. The function returns the same address if there is no collision
                        if (newMove != move)
                        {
                            // characters need to know when they're contacting something physical
                            //o.setColliding(other);
                            //other.setColliding(o);
                            collision = other;
                            // keep track of the last collision that caused a change
                            lastChangedIndex = i;
                            move = newMove;
                        }
                        else
                        {
                            // if we went through the whole loop once with no changes, then there won't be any more collisions and we can stop handling collisions
                            if (lastChangedIndex == i)
                                break;
                        }
                        // advance to the next index and wrap around
                        i++;
                        if (i >= collisions.Count)
                            i = 0;
                    }
                }
                if (collision != null)
                {
                    // Tell this object what the first thing is that it collides with
                    o.setColliding(collision);
                    // Tell the first thing that it collides with what this object is
                    collision.setColliding(o);
                    //collision.setCollisionLocation(o.getCenter());
                }
                // Now look for enemy projectiles that are colliding. We can travel through them, but they still need to know about it so they can explode
                //collisions = this.findEnemyProjectiles(o, move);
                request = new CollisionRequest();
                request.setObject(o);
                request.setCollisionMove(move);
                request.setRequestEnemies(true);
                request.setRequestProjectiles(true);
                //collisions = this.findCollisions(o, false, false, true, move, false, true, false, false);
                collisions = this.findCollisions(request);
                foreach (GameObject other in collisions)
                {
                    newMove = this.movementForCollision(o, other, move);
                    if (newMove != move)
                    {
                        other.setColliding(o);
                        //other.setCollisionLocation(other.getCenter());
                    }
                }
            }
		    // When we get here, move stores the longest valid move. We now actually move the object
		    double[] location = o.getCenter();
		    location[0] += (float)(move[0]);
		    location[1] += (float)(move[1]);
		    o.setCenter(location);
		    // save velocity
            double[] newV = new double[2];
            newV[0] = move[0] / numSeconds;
            newV[1] = move[1] / numSeconds;
		    o.setVelocity(newV);

            // tell the searcher that we just moved it, and that it may need to update its position
            this.getSearcherForObject(o).itemEndingMove(o);
            // Inform the world loader that it's done moving
            if (this.loader != null)
                this.loader.objectEndingMove(o);
        }
    }

    // Figures out if making this move would cause this projectile to explode. If so, it updates collision flags in the projectile and whatever it hit
    void checkDetonationForMove(Projectile p, double[] move)
    {
        // find any enemy object that collides with it
        CollisionRequest request = new CollisionRequest();
        request.setObject(p);
        request.setCollisionMove(move);
        request.setRequestEnemies(true);
        request.setRequestCharacters(true);
        request.setRequestProjectiles(true);
        request.setRequestExplosions(true);
        //List<GameObject> collisions = this.findCollisions(p, false, false, true, move, true, true, true, false);
        List<GameObject> collisions = this.findCollisions(request);
        double[] newMove;
        foreach (GameObject other in collisions)
        {
            newMove = this.movementForCollision(p, other, move);    // check that it actually collides
            if (newMove != move)
            {
                // Check that it's not a repeat collision
                if (!p.intersects(other))
                {
                    // Inform both objects of the collision
                    p.setColliding(other);
                    p.setCollisionMove(newMove);
                    if (other.isAProjectile())
                    {
                        other.setColliding(p);
                        //other.setCollisionLocation(other.getCenter());
                    }
                }
            }
        }
        if (!p.isColliding())
        {
            // if it didn't hit any enemies, we still have to check whether it hit any terrain
            //collisions = this.findTerrainCollisions(o, move);
            request = new CollisionRequest();
            request.setObject(p);
            request.setCollisionMove(move);
            request.setRequestGaia(true);
            request.setRequestCharacters(true);
            //collisions = this.findCollisions(p, false, true, false, move, true, false, false, false);
            collisions = this.findCollisions(request);
            foreach (GameObject other in collisions)
            {
                newMove = this.movementForCollision(p, other, move);    // check that it actually collides
                if (newMove != move)
                {
                    // Check that it's not a repeat collision
                    if (!p.intersects(other))
                    {
                        // inform it of the collision
                        p.setColliding(other);
                        p.setCollisionMove(newMove);
                        // terrain doesn't care about collisions, so if we get here we can stop checking
                        break;
                    }
                }
            }
        }
    }
    // figures out if this projectile should explode based on the current location. If so, it updates collision flags and whatever it hit
    void checkDetonation(Projectile p)
    {
        // find any enemy object that collides with it
        CollisionRequest request = new CollisionRequest();
        request.setObject(p);
        request.setRequestEnemies(true);
        request.setRequestCharacters(true);
        request.setRequestProjectiles(true);
        request.setRequestExplosions(true);
        List<GameObject> collisions = this.findCollisions(request);
        //List<GameObject> collisions = this.findCollisions(p, false, false, true, null, true, true, true, false);
        //double[] newMove = new double[2];
        foreach (GameObject other in collisions)
        {
            if (p.intersects(other))
            {
                // Inform both objects of the collision
                p.setColliding(other);
                //p.setCollisionMove(newMove);
                if (other.isAProjectile())
                {
                    other.setColliding(p);
                    //other.setCollisionLocation(other.getCenter());
                }
            }
        }
        if (!p.isColliding())
        {
            // if it didn't hit any enemies, we still have to check whether it hit any terrain
            request = new CollisionRequest();
            request.setObject(p);
            request.setRequestGaia(true);
            request.setRequestCharacters(true);
            collisions = this.findCollisions(request);
            foreach (GameObject other in collisions)
            {
                if (p.intersects(other))
                {
                    // inform it of the collision
                    p.setColliding(other);
                    //p.setCollisionMove(newMove);
                    // terrain doesn't care about collisions, so if we get here we can stop checking
                    break;
                }
            }
        }
    }
    // given that the mover wants to make the desiredMove and obstacle may be in the way, return the actual move that we will allow
	double[] movementForCollision(GameObject mover, GameObject obstacle, double[] desiredMove)
    {
        /*
	    // figure out how far it wants to move
	    double desiredDist = 0;
	    int i;
	    for (i = 0; i < desiredMove.Length; i++)
	    {
		    desiredDist += desiredMove[i] * desiredMove[i];
	    }
	    desiredDist = Math.Sqrt(desiredDist);
        */

	    // figure out how far it can go before colliding
	    double[] allowedMove = mover.moveTo(obstacle, desiredMove);
	    // If there are no collisions, return the same pointer so that the pointer can simply be compared
	    if (allowedMove == desiredMove)
		    return allowedMove;
	    // Here we make an approximation that deviates from a perfect physics simulation and instead go for the easy solution
#if true
	    /*// if you hit an enemy explosion, then you get stuck
	    if (obstacle.isAnExplosion())
	    {
		    return allowedMove;
	    }
	    else */
	    {
		    // If it's not an explosion then we do normal physics
		    // Next, figure out which way it can slide
            // This check is so complicated to be resistant to rounding error
            double spaceX = Math.Abs(mover.getCenter()[0] + allowedMove[0] - obstacle.getCenter()[0]) - (mover.getShape().getWidth() + obstacle.getShape().getWidth()) / 2;
            double spaceY = Math.Abs(mover.getCenter()[1] + allowedMove[1] - obstacle.getCenter()[1]) - (mover.getShape().getHeight() + obstacle.getShape().getHeight()) / 2;
            if (spaceX > spaceY)
		    //if (Math.Abs(mover.getCenter()[0] + allowedMove[0] - obstacle.getCenter()[0]) >= (mover.getShape().getWidth() + obstacle.getShape().getWidth()) / 2)
		    {
			    // if we get here, it slides vertically
			    allowedMove[1] = desiredMove[1];
		    }
		    else
		    {
			    // if we get here, it slides horizontally
			    allowedMove[0] = desiredMove[0];
		    }
	    }

#else
	    if (allowedDist > 0)
	    {
		    // Here we cut the move short and don't allow the object to slide laterally until the next time step
		    // For the moment this is easier to do and faster to run
		    for (i = 0; i < allowedMove.Length; i++)
		    {
			    allowedMove[i] = desiredMove[i] * allowedDist / desiredDist;
		    }
	    }
	    else
	    {
		    // If we get here then the object can't move any further in the desired direction
		    // If the simulation were perfect then this case would be very unlikely
		    // However, if the first half of the if-statement was executed for this object last tick then this half will probably be executed this tick
		    // Also, here we assume that both objects are rectangular
	    }
#endif
	    return allowedMove;
    }
    // Make an explosion for this projectile
    void explode(Projectile p)
    {
	    // make the explosion
	    Explosion e = p.explode();
        this.addExplosion(e);        
	    if (p.doneFlying())
	    {
		    // remove the projectile
            this.removeProjectile(p);
	    }
    }
    // finds all potential collisions of the desired type for the given item
    //System.Collections.Generic.List<GameObject> findCollisions(GameObject item, bool includeAllies, bool includeGaia, bool includeEnemies, double[] move, bool includeCharacters, bool includeProjectiles, bool includeExplosions, bool includeGhosts)
    System.Collections.Generic.List<GameObject> findCollisions(CollisionRequest request)
    {
        // open up some of the data in the request
        GameObject item = request.getObject();
        double[] move = request.getCollisionMove();
        // create an empty list to save in
        System.Collections.Generic.List<GameObject> collisions = new System.Collections.Generic.List<GameObject>();
        System.Collections.Generic.List<GameObject> temp = new System.Collections.Generic.List<GameObject>();
        int team;
        for (team = 0; team < this.searchers.GetLength(0); team++)
        {
            // first make sure that we care about this team
            if (team == 0)
            {
                if (!request.shouldRequestGaia())
                    continue;
            }
            else
            {
                if (team == item.getTeamNum())
                {
                    if (!request.shouldRequestAllies())
                        continue;
                }
                else
                {
                    if (!request.shouldRequestEnemies())
                        continue;
                }
            }
            // now search for the types that we care about
            if (request.shouldRequestCharacters())
            {
                temp = this.searchers[team, 0].getCollisions(item, move);   // find characters and platforms
                foreach (GameObject g in temp)
                {
                    collisions.Add(g);
                }
            }
            if (request.shouldRequestProjectiles())
            {
                temp = this.searchers[team, 1].getCollisions(item, move);   // find projectiles
                foreach (GameObject g in temp)
                {
                    collisions.Add(g);
                }
            }
            if (request.shouldRequestExplosions())
            {
                temp = this.searchers[team, 2].getCollisions(item, move);   // find explosions
                foreach (GameObject g in temp)
                {
                    collisions.Add(g);
                }
            }
            if (request.shouldRequestGhosts())
            {
                temp = this.searchers[team, 3].getCollisions(item, move);   // find ghosts
                foreach (GameObject g in temp)
                {
                    collisions.Add(g);
                }
            }
        }
        // we don't care about collisions with itself
        collisions.Remove(item);
        return collisions;
    }

    // return the WorldSearcher that has to handle this item
    WorldSearcher getSearcherForObject(GameObject item)
    {
        int typeIndex = 0;
        /*if (typeof(item).IsSubclassOf(typeof(Character)) || typeof(item).IsSubclassOf(typeof(Platform)))
        {
            typeIndex = 0;
        }
        if (typeof(item).IsSubclassOf(typeof(Projectile)))
        {
            typeIndex = 1;
        }
        if (typeof(item).IsSubclassOf(typeof(Explosion)))
        {
            typeIndex = 2;
        }*/
        if (item.isAProjectile())
            typeIndex = 1;
        if (item.isAnExplosion())
            typeIndex = 2;
        return this.searchers[item.getTeamNum(), typeIndex];
        
    }
    // variables
    System.Collections.Generic.List<GameObject> objects;
	System.Collections.Generic.List<Projectile> projectiles;
	System.Collections.Generic.List<Explosion> explosions;
	System.Collections.Generic.List<Character> characters;
	double[] dimensionsOfRealityBubble;
    // the first index into the WorldSearcher array is the team number
    // the second index into the WorldSearcher array is the type: {0,1,2} = {Character, projectile, explosion}
	WorldSearcher[,] searchers;
    Canvas canvas;
    MatrixTransform worldImageTransform;
    Camera camera;
    WorldLoader loader;
};