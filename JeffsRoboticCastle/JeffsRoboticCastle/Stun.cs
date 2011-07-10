using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// When a character collides with an explosion, a Stun is generated. The Stun can affect the character even after the Character moves away from the explosion
class Stun
{
// public
    public Stun()
    {
        initialize();
    }
    public Stun(Stun other)
    {
        initialize();
        this.copyFrom(other);
    }
    public void initialize()
    {
        //this.isNew = true;
    }
    public void copyFrom(Stun original)
    {
        this.creator = original.creator;
        this.timeMultiplier = original.timeMultiplier;
        this.damagePerSecond = original.damagePerSecond;
        this.ammoDrainedPerSec = original.ammoDrainedPerSec;
        this.duration = original.duration;
    }

    public void setCreator(Explosion value)
    {
        this.creator = value;
    }
    public Explosion getCreator()
    {
        return this.creator;
    }

    public void setTimeMultiplier(double value)
    {
        this.timeMultiplier = value;
    }
    public double getTimeMultiplier()
    {
        return this.timeMultiplier;
    }
    public void setDamagePerSecond(double value)
    {
        this.damagePerSecond = value;
    }
    public double getDamagePerSecond()
    {
        return this.damagePerSecond;
    }
    public void setAccel(double[] newAccel)
    {
        this.accel = newAccel;
    }
    public double[] getAccel()
    {
        return this.accel;
    }
    public void setDuration(double value)
    {
        this.duration = value;
    }
    public double getDuration()
    {
        return this.duration;
    }
    public double getRemainingDuration(double numSeconds)
    {
        return Math.Min(this.duration, numSeconds);
    }
    public void setAmmoDrain(double ammoPerSec)
    {
        this.ammoDrainedPerSec = ammoPerSec;
    }
    public double getAmmoDrain()
    {
        return this.ammoDrainedPerSec;
    }
    public bool isFinished(double numSeconds)
    {
        this.duration -= numSeconds;
        if (duration <= 0)
            return true;
        else
            return false;
    }
    public void degrade(double value)
    {
        //this.timeMultiplier = 1 - (1 - this.timeMultiplier) * value;
        this.damagePerSecond *= value;
        this.duration *= value;
    }
// private
    Explosion creator;
    double timeMultiplier;
    double damagePerSecond;
    double duration;
    double[] accel;
    double ammoDrainedPerSec;
    //bool isNew; // true for the first tick and false after that
    //System.Collections.Generic.Dictionary<Explosion, Stun> stuns;
};