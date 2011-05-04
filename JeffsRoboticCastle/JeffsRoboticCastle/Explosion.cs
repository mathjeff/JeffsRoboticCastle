using System.Windows.Media;
using System;

class Explosion : GameObject
{
// public
    // default constructor
    public Explosion()
    {
        initialize();
    }
    public Explosion(bool enableRendering)
    {
        setRenderable(enableRendering);
        initialize();
    }
    // copy constructor
    public Explosion(Explosion original)
    {
        initialize();
        copyFrom(original);
    }
    public Explosion(Explosion original, bool enableRendering)
    {
        setRenderable(enableRendering);
        initialize();
        copyFrom(original);
    }
    void initialize()
    {
        this.spawnedRecently = true;
    }
    // Make this explosion a copy of the original
    public void copyFrom(Explosion original)
    {
        base.copyFrom(original);
        this.duration = original.duration;
        this.creator = original.creator;
        this.setTemplateStun(new Stun(original.templateStun));
        //this.damagePerSec = original.damagePerSec;
        //this.stunFraction = original.stunFraction;
        this.friendlyFire = original.friendlyFire;
        this.knockbackAccel = original.knockbackAccel;
    }
    // Type checking
    public override bool isAnExplosion()
    {
        return true;
    }
    public void setTemplateStun(Stun value)
    {
        this.templateStun = value;
        this.templateStun.setCreator(this);
    }
    public Stun getTemplateStun()
    {
        return this.templateStun;
    }
    public void setDuration(double numSeconds)
    {
        this.duration = numSeconds;
    }
    public double getDuration()
    {
        return this.duration;
    }
    public void setKnockbackAccel(double pixelsPerSecPerSec)
    {
        this.knockbackAccel = pixelsPerSecPerSec;
    }
    public double getKnockbackAccel()
    {
        return this.knockbackAccel;
    }
    /*public void setDamagePerSec(double value)
    {
        this.damagePerSec = value;
    }
    public double getDamagePerSec()
    {
        return this.damagePerSec;
    }*/
    /*public void setStunFraction(double fraction)
    {
        this.stunFraction = fraction;
    }
    public double getStunFraction()
    {
        return this.stunFraction;
    }
    */
    public bool isFinished(double numSeconds)
    {
        this.duration -= numSeconds;
        bool result;
        if (this.duration <= 0)
            result = true;
        else
            result = false;
        return result;
    }
    public bool isNew()
    {
        if (this.spawnedRecently)
        {
            this.spawnedRecently = false;
            return true;
        }
        return false;
    }
    public bool isFriendlyFireEnabled()
    {
        return this.friendlyFire;
    }
    public void setFriendlyFireEnabled(bool value)
    {
        this.friendlyFire = value;
    }
    public bool canAffect(GameObject target)
    {
        if (this.getTeamNum() != target.getTeamNum())
            return true;
        else
            return false;
    }
    // This is called after collision checks are passed. Now we deal damage and stun the target
    public void interactWith(GameObject target, double numSeconds)
    {
        Stun newStun = new Stun(this.templateStun);
        // calculate stun duration
        double stunDuration = Math.Min(this.duration, numSeconds) + this.templateStun.getDuration();
        newStun.setDuration(stunDuration);
        // calculate stun acceleration
        double dx = target.getCenter()[0] - this.getCenter()[0];
        double dy = target.getCenter()[1] - this.getCenter()[1];
        double distance = Math.Sqrt(dx * dx + dy * dy);
        if (distance <= 0)
            distance = 1;
        double[] accel = new double[2];
        double accelScale = this.getKnockbackAccel() / distance;
        accel[0] = dx * accelScale;
        accel[1] = dy * accelScale;
        newStun.setAccel(accel);
        // apply the stun
        target.applyStun(newStun);
        //target.takeDamage(this.damagePerSec, numSeconds);
        //if (this.stunFraction < 1)
        //    target.scaleTimeMultiplier(1 - this.stunFraction);
        //else
        //    target.scaleTimeMultiplier(0);
        Character myOwner = this.getOwner();
        if (myOwner != null)
        {
            myOwner.reinforce(1);
        }
        if (target.isACharacter())
        {
            ((Character)target).reinforce(-1);
        }
    }
    // Multiply all relevant stats by the given scale
    public void degrade(double scale)
    {
        this.duration *= scale;
        this.templateStun.degrade(scale);
        //this.damagePerSec *= scale;
        //this.stunFraction *= scale;
    }
    // The projectile that created this explosion
    public void setCreator(Projectile p)
    {
        this.creator = p;
    }
    public Projectile getCreator()
    {
        return this.creator;
    }
    // The character that fired the weapon that created the projectile that created this explosion
    public Character getOwner()
    {
        if (this.creator != null)
        {
            Weapon w = this.creator.getShooter();
            if (w != null)
            {
                return w.getOwner();
            }
        }
        return null;
    }
private
	double duration;
	//double damagePerSec;
	//double stunFraction;
    bool spawnedRecently; // true for the first tick and false after that
    Projectile creator;
    Stun templateStun;
    bool friendlyFire;
    double knockbackAccel;
};