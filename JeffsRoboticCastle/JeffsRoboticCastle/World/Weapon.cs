// Weapon Attributes:
/*
Cost (in money)
Max ammo
Ammo recharge rate
Warmup time before firing
Cooldown time after firing
Time to switch to this weapon
Time to switch from this weapon
Automatic or manual trigger

Fire angle
Whether it adds the player's velocity to its starting velocity
Movement speed
Gravity (num Gees applied to this projectile)
Homing value
Coefficient of air resistance
Projectile size

Damage
Explosion radius
Stun fraction (by what percentage it slows enemies)
Penetration (fraction of damage and stun remaining after each explosion. Each time it hits something, it explodes and keeps moving)
Explosion Duration
Hitpoints (of projectiles fired. Whenever it bumps into an enemy explosion, it takes damage and may be destroyed).
Max duration (seconds. After this time, it detonates and is removed.)
Max Number of Explosions
*/

using System.Windows.Media.Imaging;
using System;
using Castle.WeaponDesign;
class Weapon
{
//public
    #region Setup
    // constructor
    public Weapon(WeaponStats stats)
    {
        this.Stats = stats;
        this.initialize();
    }
    public Weapon(Weapon original)
    {
        this.copyFrom(original);
        this.initialize();
    }
    public void copyFrom(Weapon other)
    {
        // Attributes about when you may own the item
        this.Stats = other.Stats;

        // Attributes about the current state of the weapon
        this.currentAmmo = other.currentAmmo;
        this.owner = other.owner;
        this.firingTimer = other.firingTimer;
    }
    #endregion

    private void initialize()
    {
        this.resetCooldown();
        this.refillAmmo();
    }
    private void refillAmmo()
    {
        this.currentAmmo = this.getMaxAmmo();
    }
    
    /*
    public double calculateCost()
    {
        double tempCost = 1;

        // levelAttributes of the weapon that affect the cost
        tempCost *= (1 + maxAmmo / 10);
        if (ammoRechargeRate > 0)
            tempCost *= 3;
        tempCost *= (1 + ammoRechargeRate * 15);
        tempCost *= (1 + Math.Abs(ammoPerBox) / 8);
        double warmup = Math.Abs(warmupTime);
        double fireDuration = Math.Abs(warmupTime) + Math.Abs(cooldownTime) + 0.001;
        tempCost *= Math.Pow(.75, warmup / fireDuration);
        tempCost *= (1 + 0.3 / fireDuration);
        if (fireWhileInactive)
            tempCost *= 1.2;
        if (cooldownWhileInactive)
            tempCost *= 1.2;
        if (rechargeWhileInactive)
            tempCost *= 1.9;

        // levelAttributes of the projectile that affect the cost
        tempCost *= (1 + Math.Abs(templateProjectile.getCenter()[0]) / 400);
        tempCost *= (1 + Math.Abs(templateProjectile.getCenter()[1]) / 400);
        double penetration = templateProjectile.getPenetration();
        penetration *= penetration; // square it because it affects both explosion duration and damage
        if (penetration == 1)
            tempCost *= (templateProjectile.getNumExplosionsRemaining() * 5 + 1);
        else
            tempCost *= (templateProjectile.getNumExplosionsRemaining() * 5 +
                (1 - Math.Pow(penetration, templateProjectile.getNumExplosionsRemaining())) / (1 - penetration));
        tempCost *= (1 + (1 + Math.Abs(templateProjectile.getHomingAccel() / 5)) * (1 + 3 * Math.Sqrt(Math.Abs(templateProjectile.getRemainingFlightTime()))) / 300);
        // levelAttributes of the explosion that affect the cost
        Explosion templateExplosion = templateProjectile.getTemplateExplosion();
        tempCost *= (1 + templateExplosion.getShape().getWidth() / 150);
        tempCost *= (1 + templateExplosion.getShape().getHeight() / 150);
        //tempCost *= (.1 + 2 * Math.Abs(templateExplosion.getDuration()));
        if (templateExplosion.isFriendlyFireEnabled())
            tempCost /= 2;
        tempCost *= (1 + Math.Abs(templateExplosion.getKnockbackAccel() * templateExplosion.getDuration()) / 100);

        // levelAttributes of the stun that affect the cost
        Stun templateStun = templateExplosion.getTemplateStun();
        tempCost *= (1 + 0.1 / (Math.Abs(templateStun.getTimeMultiplier()) + 0.001));
        tempCost *= (1 + 2 * Math.Abs(templateStun.getTimeMultiplier() - 1));
        tempCost *= (.1 + Math.Abs(templateStun.getDamagePerSecond()) * 2);
        if (templateExplosion.isFriendlyFireEnabled())
        {
            if (templateStun.getDamagePerSecond() <= 0)
                tempCost *= 35;
            if (templateStun.getTimeMultiplier() > 1)
                tempCost *= 35;
        }
        tempCost *= (.1 + 2 * Math.Abs(templateExplosion.getDuration()) + 3 * templateStun.getDuration());


        tempCost = Math.Ceiling(tempCost / 70);


        this.cost = tempCost;

        return cost;
    }*/
    public bool drainAmmo(double numShots)
    {
        if (this.currentAmmo > numShots)
        {
            this.currentAmmo -= numShots;
            return true;
        }
        return false;
    }

    public void resetCooldown()
    {
        this.firingTimer = this.Stats.WarmupTime;
    }
    public void resetTrigger()
    {
        this.triggerPressed = false;
        this.triggerDown = false;
        this.pressedRecently = false;
        this.triggerStuck = false;
    }
    public double getCurrentAmmo()
    {
        return this.currentAmmo;
    }
    
    public double getWarmupTime()
    {
        return this.Stats.WarmupTime;
    }
    public double getCooldownTime()
    {
        return this.Stats.CooldownTime;
    }

    public double getMaxAmmo()
    {
        return this.Stats.MaxAmmo;
    }



    // Information about the current state of the weapon
    // Call this function when the character presses or releases the trigger
    public void pressTrigger(bool pressed)
    {
        // keep track of whether they just pushed the trigger
        if (pressed && !this.triggerPressed)
            this.pressedRecently = true;
        // keep track of whether the trigger is currently being pressed
        this.triggerPressed = pressed;
        
        // keep track of whether the trigger is down
        if (pressed || this.triggerStuck)
            this.triggerDown = true;
        else
            this.triggerDown = false;
    }
    public bool isWarmingUp()
    {
	    if (this.firingTimer < this.getWarmupTime())
		    return true;
	    else
		    return false;
    }
    // returns the number of seconds remaining until the weapon fires
    public double getRemainingWarmup()
    {
        return this.firingTimer;
    }
    // returns the number of seconds 
    public double getRemainingCooldown()
    {
        return this.firingTimer - this.getWarmupTime();
    }
    public bool isCoolingDown()
    {
	    if (this.firingTimer > this.getWarmupTime())
		    return true;
	    else
		    return false;
    }
    // Tells whether the Character has expressed an interest in firing
    public bool wantsToFire()
    {
        bool shouldFire;
        // manual firing
        if (this.pressedRecently)
            shouldFire = true;
        else
            shouldFire = false;
        // pressedRecently tells whether the trigger transitioned from released to pressed within the last tick. This flag needs clearing now
        this.pressedRecently = false;
        return shouldFire;
    }
    // Tells whether the weapon should actually be firing now
    public bool shouldStartFiring()
    {
        if (this.wantsToFire() && this.currentAmmo >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // advances the state of the weapon by numSeconds. Returns false if nothing happens
    public bool timerTick(double numSeconds, bool active)
    {
        bool busy = false;
        // figure out if we need to advance the firing timer
        if (this.shouldStartFiring() || (this.firingTimer != this.getWarmupTime()))
        {
		    // check if it's cooling down
            if (this.firingTimer > this.getWarmupTime())
            {
                // If we get here, it's cooling down
                if (active)
                {
                    busy = true;    // keep track of whether the weapon is doing anything
                    // decrease the cooldown time toward zero, so that only the warmup time remains
                    this.firingTimer = Math.Max(this.firingTimer - numSeconds, this.getWarmupTime());
                }
            }
            else
            {
                // If we get here, it's warming up
                if (active)
                {
                    busy = true;    // keep track of whether the weapon is doing anything
                    this.firingTimer -= numSeconds;
                    // check if the projectile launches yet
                    if (this.firingTimer <= 0)
                    {
                        // update the firing timer
                        firingTimer += (this.getWarmupTime() + this.getCooldownTime());
                        // we can only fire one shot per tick
                        if (firingTimer < this.getWarmupTime())
                            firingTimer = this.getWarmupTime();

                        // create the projectile and fill in the information from the template
                        this.shot = this.makeProjectile(true);
                        // expend one ammo
                        this.currentAmmo--;
                    }
			    }		 
            }
        }
        // tell whether the weapon is doing anything
        return busy;
    }
    // builds a projectile and doesn't count as actually firing
    public Projectile makeProjectile(bool enableRendering)
    {
        Projectile tempShot = new Projectile(this.Stats.TemplateProjectile, enableRendering);
        tempShot.setTeamNum(this.owner.getTeamNum());
        double[] newPosition = tempShot.getCenter();
        if (this.owner.isFacingLeft())
        {
            // send the shot in the other direction if they're facing left
            tempShot.getVelocity()[0] *= -1;
            newPosition[0] *= -1;
            tempShot.setFacingLeft(true);
        }
        newPosition[0] += owner.getCenter()[0];
        newPosition[1] += owner.getCenter()[1];
        tempShot.setCenter(newPosition);
        double[] newV = tempShot.getVelocity();
        newV[0] += owner.getVelocity()[0] * this.Stats.OwnersVelocityScale;
        newV[1] += owner.getVelocity()[1] * this.Stats.OwnersVelocityScale;
        tempShot.setVelocity(newV);
        return tempShot;
    }
    // Returns the latest projectile created by firing the weapon
    public Projectile getProjectile()
    {
        // get the latest projectile we created
        Projectile shot = this.shot;
        // clear the latest projectile we created
        this.shot = null;
        // return the projectile
        return shot;
    }
    // Purchasing the weapon
    public void setOwner(Character value)
    {
	    this.owner = value;
    }
    public Character getOwner()
    {
	    return this.owner;
    }
    // simulate firing the weapon and calculate by how far it will miss
    public double[] simulateShooting(GameObject target)
    {
        // compute the expected minimum offset (x,y) of the projectile compared to the target
        if (this.owner == null)
            return null;
        if (target == null)
            return null;
        // compute what the levelAttributes of the projectile would be if it were created now
        Projectile currentProjectile = this.makeProjectile(false);
        // simulate it in short intervals for a bunch of counts
        int i;
        double[] location;
        double[] desiredMove;
        double[] actualMove;
        //int count = 25;
        // determine the number of iterations and the length of each
        double numSeconds = 0.2;
        int count = (int)(currentProjectile.getRemainingFlightTime() / numSeconds);
        if (count > 25)
            count = 25;
        if (count < 5)
            count = 5;
        numSeconds = currentProjectile.getRemainingFlightTime() / count;
        currentProjectile.setTarget(target);
        for (i = 0; i < count; i++)
        {
            currentProjectile.updateVelocity(numSeconds);
            // figure out how far the projectile wants to move
            desiredMove = currentProjectile.getMove(numSeconds);
            // figure out how far the projectile could actually move
            actualMove = currentProjectile.moveTo(target, desiredMove);
            // if it doesn't collide then step the simulation and continue
            location = currentProjectile.getCenter();
            location[0] += actualMove[0];
            location[1] += actualMove[1];
            currentProjectile.setCenter(location);
            // check if it collides with the target
            if (actualMove != desiredMove)
                break;
            // check if it collides with the floor
            if (location[1] <= 0)
                break;
        }
        double[] offset = new double[2];
        Explosion tempExplosion = currentProjectile.explode();
        if (tempExplosion.intersects(target))
        {
            //System.Diagnostics.Trace.WriteLine("trajectory predicts a hit");
            return offset;
        }
        // compute the offset from the projectile to the target
        offset[0] = target.getCenter()[0] - tempExplosion.getCenter()[0];
        offset[1] = target.getCenter()[1] - tempExplosion.getCenter()[1];
        //System.Diagnostics.Trace.WriteLine("offsetx = " + offset[0].ToString());
        return offset;
    }


    #region Private Variables

    WeaponConfiguration design;

    public WeaponStats Stats;

	// Attributes about when you may fire
    Projectile shot;

	// Attributes about the current state of the weapon
	double currentAmmo;
	Character owner;
	double firingTimer;	// firingTimer equals warmupTime when nothing is going on. It's 0 when it fires, and it equals warmupTime + cooldownTime right after.
    bool pressedRecently;	// Tells whether the trigger transitioned from not pressed to pressed since the last tick
    bool triggerPressed;    // Tells whether the trigger is currently being pressed by the owner
    bool triggerStuck;      // Tells whether the trigger will remain pressed the next time it is released
    bool triggerDown;       // Tells whether the trigger is engaged
    #endregion
};