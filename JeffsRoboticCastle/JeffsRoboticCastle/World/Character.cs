using System;
using System.Windows.Media;
using System.Collections.Generic;
class Character : GameObject
{
    
    #region Setup
    public Character()
    {
        this.initialize();
    }
    public void copyFrom(Character original)
    {
        // Initialize
        base.copyFrom(original);
        // copy attributes from the original
        this.brain = original.brain;
        this.world = original.world;
        this.setMaxAccel(original.getMaxAccel());
        this.setArmor(original.getArmor());
        this.money = original.getMoney();
        this.setTargetVX(original.getTargetVX());
        this.setTargetVY(original.getTargetVY());
        //this.setTargetVelocity(original.getTargetVelocity());
        this.setJumpVelocity(original.jumpVelocity);
        // for each weapon, copy it and add it
        foreach (Weapon currentWeapon in original.weapons)
        {
            this.addWeapon(new Weapon(currentWeapon));
        }
        this.contactDamagePerSecond = original.contactDamagePerSecond;
    }
    public void initialize()
    {
        this.initializeHitpoints(10);
        this.setJumpSpeed(1000);
        this.weapons = new System.Collections.Generic.List<Weapon>();
        //this.targetVelocity = new double[2];
        this.targetVX = 0;
        this.targetVY = 0;
        this.activeWeapons = new System.Collections.Generic.List<Weapon>();
        this.money = 0;
    }
    public override bool isACharacter()
    {
        return true;
    }
    #endregion

    #region Movement
    // tells whether this object can make itself move
    public override bool isMovable()
    {
        return true;
    }
    public double[] getMaxAccel()
    {
        return this.maxAccel;
    }
    public void setMaxAccel(double[] newAccel)
    {
        this.maxAccel = newAccel;
    }
    public double? getTargetVX()
    {
        return this.targetVX;
    }
    public void setTargetVX(double? newVelocity)
    {
        this.targetVX = newVelocity;
    }
    public double? getTargetVY()
    {
        return this.targetVY;
    }
    public void setTargetVY(double? newVelocity)
    {
        this.targetVY = newVelocity;
    }
    // accelerate toward the desired velocity
    public override void updateVelocity(double numSeconds)
    {
        // when reloading, the player gets no control
        if (true) //!this.isReloadingAmmo())
        {
            // First update velocity based on any accelerations from within the character
            // Compute desired acceleration
            double[] v = this.getVelocity();
            double accelX = 0;
            if (this.targetVX != null)
                accelX = (double)this.targetVX - v[0];
            double accelY = 0;
            if (this.targetVY != null)
                accelY = (double)this.targetVY - v[0];
            // Determine maximum acceleration
            double maxAccelX = numSeconds * this.maxAccel[0];
            double maxAccelY = numSeconds * this.maxAccel[1];
            // Compute ratio of desired to max accelerations
            double xAccelFrac = 0;
            if (maxAccelX != 0)
                xAccelFrac = accelX / maxAccelX;
            double yAccelFrac = 0;
            if (maxAccelY != 0)
                yAccelFrac = accelY / maxAccelY;
            double magnitude = Math.Sqrt(xAccelFrac * xAccelFrac + yAccelFrac * yAccelFrac);
            if (magnitude <= 1)
            {
                if ((maxAccelX != 0) && (this.targetVX != null))
                    v[0] = (double)this.targetVX;
                if ((maxAccelY != 0) && (this.targetVY != null))
                    v[1] = (double)this.targetVY;
            }
            else
            {
                // The set of maximum accelerations may be an oval
                v[0] += (float)(xAccelFrac * maxAccelX / magnitude);
                v[1] += (float)(yAccelFrac * maxAccelY / magnitude);
            }
            this.setVelocity(v);
        }

        // Now update velocity based on the physics of a projectile
        base.updateVelocity(numSeconds);
    }
    // keep track of something that it is colliding with
    public override void setColliding(GameObject other)
    {
        this.collision = other;
        if ((this.collision != null) && (this.targetVX != null))
        {
            if ((double)this.targetVX > 0)
            {
                this.setFacingRight(true);
            }
            if ((double)this.targetVX < 0)
            {
                this.setFacingRight(false);
            }
        }
    }
    public GameObject getCollision()
    {
        return this.collision;
    }
    public override bool isColliding()
    {
        if (this.collision != null)
            return true;
        else
            return false;
    }
    public void setJumpSpeed(double newSpeed)
    {
        double[] newVelocity = new double[2];
        newVelocity[0] = 0;
        newVelocity[1] = newSpeed;
        this.setJumpVelocity(newVelocity);
    }
    public void setJumpVelocity(double[] newVelocity)
    {
        this.jumpVelocity = newVelocity;
    }
    public void jump()
    {
        // make sure we're colliding with something and not moving up
	    if (this.isColliding() && this.getVelocity()[1] <= 0.00001)
	    {
            // make sure we're not hitting the ceiling
            if (this.collision.getCenter()[1] - this.collision.getShape().getHeight() / 2 + 0.00001 < this.getCenter()[1] + this.getShape().getHeight() / 2)
            {
                // make sure we're not reloading
                // now jump
                double[] newVelocity = new double[2];
                if (this.isFacingRight())
                    newVelocity[0] = this.getVelocity()[0] + this.jumpVelocity[0];
                else
                    newVelocity[0] = this.getVelocity()[0] - this.jumpVelocity[0];
                newVelocity[1] = this.jumpVelocity[1];
                this.setVelocity(newVelocity);
            }
	    }
    }

    #endregion

    #region Purchasing
    public bool spendMoney(double amount)
    {
        if (this.money >= amount)
        {
            this.money -= amount;
            return true;
        }
        return false;
    }
    public double getMoney()
    {
        return this.money;
    }
    public void addMoney(double amount)
    {
        this.money += amount;
    }
    #endregion

    #region Weapons
    public void pressTrigger(bool pressed)
    {
        Weapon currentWeapon = this.getCurrentWeapon();
	    if (currentWeapon != null)
	    {
            currentWeapon.pressTrigger(pressed);
	    }
    }
    public void cycleWeaponForward()
    {
        int newIndex = this.currentWeaponIndex + 1;
        if (newIndex >= this.weapons.Count)
        {
            newIndex = 0;
        }
        this.setWeaponIndex(newIndex);
    }
    public void cycleWeaponBackward()
    {
        int newIndex = currentWeaponIndex - 1;
        if (newIndex < 0)
        {
            newIndex = this.weapons.Count - 1;
        }
        this.setWeaponIndex(newIndex);
    }
    public Weapon getCurrentWeapon()
    {
        if (this.weapons.Count < 1)
            return null;
        return this.weapons[currentWeaponIndex];
    }
    public Weapon getCurrentWeaponShiftedByIndex(int indexOffset)
    {
        if (this.weapons.Count < 1)
            return null;
        int index = this.currentWeaponIndex + indexOffset;
        index = index % this.weapons.Count;
        if (index < 0)
            index += this.weapons.Count;
        return this.weapons[index];
    }
    public virtual System.Collections.Generic.List<Weapon> getWeaponTreeAtIndex(int index)
    {
        return null;
    }
    public virtual int getWeaponTreeBranchFactor()
    {
        return 0;
    }
    // returns the newly spawned projectile if there is one
    public System.Collections.ArrayList getProjectiles(double numSeconds)
    {
        int i;
        System.Collections.ArrayList projectiles = new System.Collections.ArrayList(1);
        Weapon currentWeapon = null;
        Projectile tempProjectile;
        if (this.weapons.Count > 0)
        {
            currentWeapon = this.weapons[this.currentWeaponIndex];
            // make sure that the currently held weapon isn't in the list of active weapons so that it only gets one timer tick
            this.activeWeapons.Remove(currentWeapon);
            // advance the state of the weapon by numSeconds
            currentWeapon.timerTick(numSeconds, true);
            tempProjectile = currentWeapon.getProjectile();
            if (tempProjectile != null)
                projectiles.Add(tempProjectile);
        }
        bool active;
        Weapon tempWeapon;
        for (i = this.activeWeapons.Count - 1; i >= 0; i--)
        {
            tempWeapon = activeWeapons[i];
            active = tempWeapon.timerTick(numSeconds, false);
            tempProjectile = tempWeapon.getProjectile();
            if (tempProjectile != null)
                projectiles.Add(tempProjectile);
            if (!active)
            {
                // If the weapon isn't doing anything then remove it from the list of weapons that are doing anything
                activeWeapons.RemoveAt(i);
            }
        }
        if (currentWeapon != null)
        {
            this.activeWeapons.Add(currentWeapon);
        }
        return projectiles;
    }

    // adds the given weapon the the character's arsenal
    public void addWeapon(Weapon newWeapon)
    {
        this.weapons.Add(newWeapon);
        newWeapon.setOwner(this);
    }
    public IEnumerable<Weapon> Weapons
    {
        get
        {
            return this.weapons;
        }
    }
    public Weapon getWeaponAtIndex(int index)
    {
        return this.weapons[index];
    }
    public int getNumWeapons()
    {
        return this.weapons.Count;
    }
    public void setWeaponIndex(int index)
    {
        if (this.currentWeaponIndex != index)
        {
            Weapon currentWeapon = this.getCurrentWeapon();
            if (currentWeapon.isWarmingUp())
            {
                // if the weapon can't fire while inactive and it is about to become inactive, cancel the firing
                currentWeapon.resetCooldown();
            }
            this.currentWeaponIndex = index;
        }
    }
    // recharge some ammunition due to reaching an ammo box
    
    #endregion

    #region Combat
    public override void takeDamage(double damagePerSec, double numSeconds)
    {
        // subtract the armor per second and take damage equal to the result
        if (damagePerSec >= this.armor)
            damagePerSec -= armor;
        base.takeDamage(damagePerSec, numSeconds);
    }
    public override bool isStunnable()
    {
        return true;
    }
    public override double getContactDamagePerSecond()
    {
        return this.contactDamagePerSecond;
    }
    public double getArmor()
    {
        return this.armor;
    }
    public void setArmor(double newArmor)
    {
        this.armor = newArmor;
    }

    public void setContactDamagePerSecond(double value)
    {
        this.contactDamagePerSecond = value;
    }
    public override void processStuns(double numSeconds)
    {
        base.processStuns(numSeconds);

        // drain ammo
        IEnumerable<Stun> stuns = this.getStuns();
        double currentAmmoDrained;
        Weapon currentWeapon = this.getCurrentWeapon();
        if (currentWeapon != null)
        {
            foreach (Stun currentStun in stuns)
            {
                currentAmmoDrained = currentStun.getAmmoDrain() * numSeconds;
                if (!(currentWeapon.drainAmmo(currentAmmoDrained)))
                {
                    break;
                }
            }
        }
    }
    #endregion



    #region AI
    // do whatever the AI requires
    public virtual void think()
    {
        if (this.brain != null)
            this.brain.think(this);
    }
    public void adjustBehavior()
    {
        if (this.brain != null)
            this.brain.adjustBehavior();
    }



    // Stuff that the AI may need
    public void setBrain(AI newBrain)
    {
        this.brain = newBrain;
    }
    public AI getBrain()
    {
        return this.brain;
    }
    public void invalidateNeighborData()
    {
        // flag them as needing updating, but don't clear their values because they may make for good hints
        this.nearestEnemyCharacterUpToDate = false;
        this.nearestEnemyProjectileUpToDate = false;
        //this.myGroundUpToDate = false;
        this.nearestObstacleUpToDate = false;
    }
    public Character getNearestEnemyCharacter()
    {
        // If it hasn't been updated yet, then update it
        if (!this.nearestEnemyCharacterUpToDate)
        {
            // find the nearest enemy character, using the one from the previous tick as a hint
            CollisionRequest request = new CollisionRequest();
            request.setObject(this);
            request.setRequestEnemies(true);
            request.setRequestCharacters(true);
            request.growToTouch(this.nearestEnemyCharacter);
            this.nearestEnemyCharacter = (Character)(this.getWorld().findNearestObject(request));
            this.nearestEnemyCharacterUpToDate = true;
        }
        return this.nearestEnemyCharacter;
    }
    public Projectile getNearestEnemyProjectile()
    {
        // If it hasn't been updated yet, then update it
        if (!this.nearestEnemyProjectileUpToDate)
        {
            // find the nearest enemy projectile, using the one from the previous tick as a hint
            CollisionRequest request = new CollisionRequest();
            request.setObject(this);
            request.setRequestEnemies(true);
            request.setRequestProjectiles(true);
            request.growToTouch(this.nearestEnemyProjectile);
            this.nearestEnemyProjectile = (Projectile)(this.getWorld().findNearestObject(request));
            this.nearestEnemyProjectileUpToDate = true;
        }
        return this.nearestEnemyProjectile;
    }
    public GameObject getNearestObstacle()
    {
        // If it hasn't been updated yet, then update it
        if (!this.nearestObstacleUpToDate)
        {
            // find the nearest non-ally, non-enemy object, using the one from the previous tick as a hint
            CollisionRequest request = new CollisionRequest();
            request.setObject(this);
            request.setRequestGaia(true);
            request.setRequestCharacters(true);
            request.growToTouch(this.nearestObstacle);
            this.nearestObstacle = this.getWorld().findNearestObject(request);
            this.nearestObstacleUpToDate = true;
        }
        return this.nearestObstacle;
    }
    public GameObject getMyGround()
    {
        return this.getCollision();
    }
    public void reinforce(double quantity)
    {
        if (this.brain != null)
            this.brain.reinforce(quantity);
    }
    public void setWorld(World w)
    {
        this.world = w;
    }
    public World getWorld()
    {
        return this.world;
    }

    
    #endregion

    #region Private Variables
    AI brain;
    World world;
	double[] maxAccel;
	double armor;
    double money;
    double? targetVX;
    double? targetVY;
    double[] jumpVelocity;
    GameObject collision;	// Tells what this object is colliding with, or null.
	System.Collections.Generic.List<Weapon> weapons;
	int currentWeaponIndex;
    Character nearestEnemyCharacter; bool nearestEnemyCharacterUpToDate;
    Projectile nearestEnemyProjectile; bool nearestEnemyProjectileUpToDate;
    GameObject nearestObstacle; bool nearestObstacleUpToDate;
    double contactDamagePerSecond;  // how much damage per second it deals to anything touching it
    System.Collections.Generic.List<Weapon> activeWeapons;
    #endregion
};


