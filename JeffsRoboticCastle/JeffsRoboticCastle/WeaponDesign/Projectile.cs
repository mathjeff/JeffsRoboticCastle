using System.Windows.Media.Imaging;
using System;
class Projectile : GameObject
{
//public
    // default constructor
    public Projectile()
    {
        this.initialize();
    }
    public Projectile(bool enableRendering)
    {
        this.setRenderable(enableRendering);
        this.initialize();
    }
    public void initialize()
    {
        this.setZIndex(2);
        this.numExplosionsRemaining = 1;
        this.homeOnCharacters = true;
    }
    // copy constructor
    public Projectile(Projectile original)
    {
        this.copyFrom(original);
    }
    public Projectile(Projectile original, bool enableRendering)
    {
        this.setRenderable(enableRendering);
        this.copyFrom(original);
    }
    // Make this equal to the other projectile
    public void copyFrom(Projectile original)
    {
        base.copyFrom(original);
        this.remainingFlightTime = original.remainingFlightTime;
        this.penetration = original.penetration;
        this.numExplosionsRemaining = original.numExplosionsRemaining;
        this.templateExplosion = new Explosion(original.templateExplosion, this.isRenderable());
        this.homingAccel = original.homingAccel;
        this.homeOnCharacters = original.homeOnCharacters;
        this.homeOnProjectiles = original.homeOnProjectiles;
        this.target = original.target;
        this.shooter = original.shooter;
        this.owner = original.owner;
        this.boomerangAccel = original.boomerangAccel;
    }
    // tells whether this object can make itself move
    public override bool isMovable()
    {
        return true;
    }

    // Information about flight
    // Time until it is forced to explode and disappear
    public void setRemainingFlightTime(double numSeconds)
    {
	    this.remainingFlightTime = numSeconds;
    }
    public double getRemainingFlightTime()
    {
	    return this.remainingFlightTime;
    }
    // The damage is scaled by the penetration fraction each time the projectile explodes.
    public void setPenetration(double fraction)
    {
        this.penetration = fraction;
    }
    public double getPenetration()
    {
        return this.penetration;
    }
    // Set the number of times that it can explode before being removed
    public void setNumExplosionsRemaining(int value)
    {
        this.numExplosionsRemaining = value;
    }
    public int getNumExplosionsRemaining()
    {
        return this.numExplosionsRemaining;
    }
    // Whether it homes on enemy projectiles
    public void enableHomingOnProjectiles(bool enable)
    {
        this.homeOnProjectiles = enable;
    }
    public bool shouldHomeOnProjectiles()
    {
        return this.homeOnProjectiles;
    }
    public void enableHomingOnCharacters(bool enable)
    {
        this.homeOnCharacters = enable;
    }
    public bool shouldHomeOnCharacters()
    {
        return this.homeOnCharacters;
    }

    // Saves the given explosion, which will be created each time this projectile explodes
    public void setTemplateExplosion(Explosion newExplosion)
    {
        this.templateExplosion = newExplosion;
        templateExplosion.setCreator(this);
    }
    public Explosion getTemplateExplosion()
    {
        return this.templateExplosion;
    }
    // type checking
    public override bool isAProjectile()
    {
	    return true;
    }
    public override void setColliding(GameObject other)
    {
        this.collision = other;
    }
    public override bool isColliding()
    {
        if (this.collision != null)
            return true;
        else
            return false;
    }
    public override void setCollisionLocation(double[] location)
    {
        this.collisionLocation = location;
    }
    public override double[] getCollisionLocation()
    {
        return this.collisionLocation;
    }
    public override void setCollisionMove(double[] move)
    {
        this.collisionLocation = new double[2];
        collisionLocation[0] = this.getCenter()[0] + move[0];
        collisionLocation[1] = this.getCenter()[1] + move[1];
    }
    public void timerTick(double numSeconds)
    {
	    this.remainingFlightTime -= numSeconds;
    }
    // Lock onto this target. Without this, homing won't work.
    public void setTarget(GameObject newTarget)
    {
        this.target = newTarget;
    }
    public GameObject getTarget()
    {
        return this.target;
    }
    public void setOwner(GameObject newOwner)
    {
        this.owner = newOwner;
    }
    public void setHomingAccel(double pixelsPerSecPerSec)
    {
        this.homingAccel = pixelsPerSecPerSec;
    }
    public double getHomingAccel()
    {
        return this.homingAccel;
    }
    public void setBoomerangAccel(double pixelsPerSecPerSec)
    {
        this.boomerangAccel = pixelsPerSecPerSec;
    }
    public double getBoomerangAccel()
    {
        return this.boomerangAccel;
    }
    // Tells whether it's time to remove the projectile
    public bool doneFlying()
    {
        if (numExplosionsRemaining <= 0)
            return true;
	    if (remainingFlightTime <= 0)
		    return true;
        // flag it as destroyed
        this.setHitpoints(0);
	    //if ((this.templateExplosion.getDamagePerSec() <= 0) && (this.templateExplosion.getStunFraction() <= 0))
		//    return true;
	    return false;
    }
    // Generate an explosion. Only call this when the collision tests have been passed
    public Explosion explode()
    {
	    // make the new explosion
	    Explosion e = new Explosion(this.templateExplosion);
        if (this.collisionLocation != null)
            e.setCenter(this.collisionLocation);
        else
    	    e.setCenter(this.getCenter());
        if (this.isFacingLeft())
            e.setFacingLeft(true);
        // Degrade some of our Stats for having exploded
        this.templateExplosion.degrade(this.penetration);
        this.numExplosionsRemaining--;
	    return e;
    }
    // change the team of both the projectile and the explosions it makes
    public override void setTeamNum(int value)
    {
        if (templateExplosion != null)
            templateExplosion.setTeamNum(value);
        base.setTeamNum(value);
    }
    // Adjust the current velocity according to the current accelerations, and the provided (small)time interval
    public override void updateVelocity(double numSeconds)
    {
        // get the current velocity
        double[] currentVelocity = this.getVelocity();
        double[] newVelocity = new double[currentVelocity.Length];
        currentVelocity.CopyTo(newVelocity, 0);

        // home in on the target
        if ((this.homingAccel != 0) && (this.target != null) && this.target.isAlive())
        {
            double[] accel = new double[this.getCenter().Length];
            int i;
            double dist = 0;
            // assign a value for each dimension
            for (i = 0; i < accel.Length; i++)
            {
                // calculate the desired velocity to hit most quickly
                accel[i] = target.getCenter()[i] - this.getCenter()[i];
                dist += accel[i] * accel[i];
            }
            // calculate distance and normalize
            dist = Math.Sqrt(dist);
            if (dist > 0)
            {
                // accelerate as much as possible toward the target
                double scale = this.homingAccel * numSeconds / dist;
                for (i = 0; i < accel.Length; i++)
                {
                    newVelocity[i] += accel[i] * scale;
                }
            }
        }
        // accelerate towards the owner if necessary
        if ((this.boomerangAccel != 0) && (this.owner != null) && this.owner.isAlive())
        {
            double[] accel = new double[this.getCenter().Length];
            int i;
            double dist = 0;
            // assign a value for each dimension
            for (i = 0; i < accel.Length; i++)
            {
                // calculate the desired velocity to hit most quickly
                accel[i] = owner.getCenter()[i] - this.getCenter()[i];
                dist += accel[i] * accel[i];
            }
            // calculate distance and normalize
            dist = Math.Sqrt(dist);
            if (dist > 0)
            {
                // accelerate as much as possible toward the target
                double scale = this.boomerangAccel * numSeconds / dist;
                for (i = 0; i < accel.Length; i++)
                {
                    newVelocity[i] += accel[i] * scale;
                }
            }
        }
        this.setVelocity(newVelocity);
        base.updateVelocity(numSeconds);
    }
    public bool needsATarget()
    {
        if (this.homingAccel != 0)
        {
            if ((this.target != null) && (this.target.isAlive()))
                this.target = null;
            return true;
        }
        return false;
    }
    // The weapon that created this projectile
    public void setShooter(Weapon w)
    {
        this.shooter = w;
    }
    public Weapon getShooter()
    {
        return this.shooter;
    }
private

	double remainingFlightTime;
    int numExplosionsRemaining; // how many more times it can explode before being removed

    double penetration;
    Explosion templateExplosion;
    double homingAccel;
    double boomerangAccel;
    bool homeOnProjectiles;
    bool homeOnCharacters;
    
    // information about the current state of the projectile that don't apply to the template projectile
    GameObject collision;
    double[] collisionLocation;
    Weapon shooter;
    GameObject target;
    GameObject owner;
};
