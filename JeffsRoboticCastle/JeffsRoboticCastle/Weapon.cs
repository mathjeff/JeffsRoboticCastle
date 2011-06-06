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
class Weapon
{
//public
	// constructor
    public Weapon()
    {
    }
    public Weapon(int premadeType)
    {
	    // type
        double[] tempVector = new double[2];
        Explosion templateExplosion;
        Stun templateStun;
        // #define DESIGN_HERE
        switch (premadeType)
        {

            case 0:

                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 10;
                ammoPerBox = 4;
                ammoRechargeRate = .03;
                this.setWarmupTime(.9);
                cooldownTime = 1;
                switchToTime = 1;
                switchFromTime = 2;
                automatic = true;
                this.ownersVelocityScale = 1.2;
                fireWhileInactive = true;
                cooldownWhileInactive = true;
                rechargeWhileInactive = true;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameCircle(20));
                templateProjectile.setBitmap(ImageLoader.loadImage("Bomb2.png"));
                tempVector[0] = 0; tempVector[1] = 0;
                templateProjectile.setVelocity(tempVector);
                templateProjectile.setGravity(800);
                templateProjectile.setHomingAccel(100);
                templateProjectile.setDragCoefficient(.8);
                templateProjectile.setRemainingFlightTime(10);
                templateProjectile.setPenetration(0.5);
                templateProjectile.setNumExplosionsRemaining(1);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                //templateExplosion.setDamagePerSec(4);
                templateExplosion.setShape(new GameCircle(110));
                templateExplosion.setBitmap(ImageLoader.loadImage("explosion2.png"));
                //templateExplosion.setStunFraction(0.6);
                templateExplosion.setDuration(2);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(5);
                templateStun.setTimeMultiplier(0.5);
                templateStun.setDuration(0);
                templateExplosion.setTemplateStun(templateStun);
                break;

            case 1:
                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 10;
                ammoPerBox = 5;
                ammoRechargeRate = 0.07;
                this.setWarmupTime(0);
                cooldownTime = 2.7;
                switchToTime = 1;
                switchFromTime = 2;
                automatic = false;
                this.ownersVelocityScale = 0;
                fireWhileInactive = true;
                cooldownWhileInactive = true;
                rechargeWhileInactive = true;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                tempVector[0] = 0; tempVector[1] = 0;
                templateProjectile.setCenter(tempVector);
                templateProjectile.setShape(new GameCircle(10));
                templateProjectile.setBitmap(ImageLoader.loadImage("Toxic.png"));
                tempVector[0] = 0; tempVector[1] = 0;
                templateProjectile.setVelocity(tempVector);
                templateProjectile.setGravity(1);
                templateProjectile.setHomingAccel(300);
                templateProjectile.setBoomerangAccel(1);
                templateProjectile.setDragCoefficient(1);
                templateProjectile.setRemainingFlightTime(20);
                templateProjectile.setPenetration(1);
                templateProjectile.setNumExplosionsRemaining(2);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                templateExplosion.setShape(new GameCircle(11));
                templateExplosion.setBitmap(ImageLoader.loadImage("Toxic2.png"));
                templateExplosion.setDuration(0);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(.4);
                templateStun.setTimeMultiplier(0.9);
                templateStun.setDuration(5.5);
                templateExplosion.setTemplateStun(templateStun);
                break;

            case 2:
                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 8;
                ammoPerBox = 4;
                ammoRechargeRate = 0.25;
                this.setWarmupTime(0.01);
                cooldownTime = 0.2;
                switchToTime = 1;
                switchFromTime = 2;
                automatic = false;
                this.ownersVelocityScale = 2.2;
                fireWhileInactive = true;
                cooldownWhileInactive = true;
                rechargeWhileInactive = true;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameCircle(20));
                templateProjectile.setBitmap(ImageLoader.loadImage("Fireball3.png"));
                tempVector[0] = 240; tempVector[1] = 40;
                templateProjectile.setVelocity(tempVector);
                templateProjectile.setGravity(0);
                templateProjectile.setHomingAccel(280);
                templateProjectile.setDragCoefficient(0);
                templateProjectile.setRemainingFlightTime(10);
                templateProjectile.setPenetration(0.5);
                templateProjectile.setNumExplosionsRemaining(1);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                //templateExplosion.setDamagePerSec(6);
                templateExplosion.setShape(new GameCircle(70));
                templateExplosion.setBitmap(ImageLoader.loadImage("explosion2.png"));
                //templateExplosion.setStunFraction(0.3);
                templateExplosion.setDuration(0.667);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(6);
                templateStun.setTimeMultiplier(0.7);
                templateStun.setDuration(0);
                templateExplosion.setTemplateStun(templateStun);
                break;


            case 3:
                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 10;
                ammoPerBox = 8;
                ammoRechargeRate = .24;
                this.setWarmupTime(0);
                cooldownTime = .8;
                switchToTime = 1;
                switchFromTime = 2;
                automatic = true;
                this.ownersVelocityScale = .5;
                fireWhileInactive = true;
                cooldownWhileInactive = true;
                rechargeWhileInactive = false;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameCircle(20));
                templateProjectile.setBitmap(ImageLoader.loadImage("BlackHole.png"));
                tempVector[0] = 400; tempVector[1] = 0;
                templateProjectile.setVelocity(tempVector);
                tempVector[0] = 0; tempVector[1] = 0;
                templateProjectile.setCenter(tempVector);
                templateProjectile.setGravity(0);
                templateProjectile.setHomingAccel(500);
                templateProjectile.setBoomerangAccel(0);
                templateProjectile.setDragCoefficient(-.5);
                templateProjectile.setRemainingFlightTime(1);
                templateProjectile.setPenetration(1);
                templateProjectile.setNumExplosionsRemaining(6);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                //templateExplosion.setDamagePerSec(6);
                templateExplosion.setShape(new GameCircle(50));
                templateExplosion.setBitmap(ImageLoader.loadImage("BlackHole.png"));
                templateExplosion.setKnockbackAccel(-200);
                //templateExplosion.setStunFraction(0.3);
                templateExplosion.setDuration(.333);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(3);
                templateStun.setTimeMultiplier(.9);
                templateStun.setDuration(0);
                templateExplosion.setTemplateStun(templateStun);
                break;

            case 4:
                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 10;
                ammoPerBox = 10;
                ammoRechargeRate = 0.4;
                this.setWarmupTime(0);
                cooldownTime = 0.5;
                switchToTime = 1;
                switchFromTime = 2;
                automatic = false;
                this.ownersVelocityScale = 0;
                fireWhileInactive = true;
                cooldownWhileInactive = true;
                rechargeWhileInactive = true;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameCircle(10));
                templateProjectile.setBitmap(ImageLoader.loadImage("wasp2.png"));
                tempVector[0] = 100; tempVector[1] = 30;
                templateProjectile.setVelocity(tempVector);
                templateProjectile.setGravity(0);
                templateProjectile.setHomingAccel(1500);
                templateProjectile.enableHomingOnProjectiles(true);
                templateProjectile.setBoomerangAccel(0);
                templateProjectile.setDragCoefficient(0.35);
                templateProjectile.setRemainingFlightTime(5);
                templateProjectile.setPenetration(1);
                templateProjectile.setNumExplosionsRemaining(4);
                templateProjectile.initializeHitpoints(70);


                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                //templateExplosion.setDamagePerSec(0.04);
                templateExplosion.setShape(new GameCircle(40));
                templateExplosion.setBitmap(ImageLoader.loadImage("explosion.png"));
                //templateExplosion.setStunFraction(0.05);
                templateExplosion.setDuration(.7);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(0.5);
                templateStun.setTimeMultiplier(1);
                templateStun.setDuration(0);
                templateExplosion.setTemplateStun(templateStun);
                break;

            case 5:

                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 3;
                ammoPerBox = 3;
                ammoRechargeRate = .1;
                this.setWarmupTime(0);
                cooldownTime = 1;
                switchToTime = 1;
                switchFromTime = 2;
                automatic = false;
                this.ownersVelocityScale = 1;
                fireWhileInactive = false;
                cooldownWhileInactive = true;
                rechargeWhileInactive = true;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameCircle(20));
                templateProjectile.setBitmap(ImageLoader.loadImage("Banana.png"));
                tempVector[0] = 500; tempVector[1] = 250;
                templateProjectile.setVelocity(tempVector);
                templateProjectile.setGravity(1000);
                templateProjectile.setHomingAccel(120);
                templateProjectile.setDragCoefficient(0);
                templateProjectile.setRemainingFlightTime(10);
                templateProjectile.setPenetration(0);
                templateProjectile.setNumExplosionsRemaining(1);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                templateExplosion.setShape(new GameCircle(30));
                templateExplosion.setBitmap(ImageLoader.loadImage("Banana.png"));
                templateExplosion.setDuration(12);
                templateExplosion.setKnockbackAccel(16000);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(1);
                templateStun.setTimeMultiplier(.25);
                templateStun.setDuration(.1);
                templateExplosion.setTemplateStun(templateStun);
                break;


            case 6:

                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 5;
                ammoPerBox = 2;
                ammoRechargeRate = .1;
                this.setWarmupTime(0);
                cooldownTime = 6;
                switchToTime = 1;
                switchFromTime = 2;
                automatic = false;
                this.ownersVelocityScale = 1;
                fireWhileInactive = false;
                cooldownWhileInactive = true;
                rechargeWhileInactive = true;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameCircle(20));
                templateProjectile.setBitmap(ImageLoader.loadImage("Missile.png"));
                tempVector[0] = 800; tempVector[1] = 200;
                templateProjectile.setCenter(tempVector);
                tempVector[0] = 0; tempVector[1] = -10;
                templateProjectile.setVelocity(tempVector);
                templateProjectile.setGravity(10);
                templateProjectile.setHomingAccel(6000);
                templateProjectile.setBoomerangAccel(0);
                templateProjectile.setDragCoefficient(1);
                templateProjectile.setRemainingFlightTime(.4);
                templateProjectile.setPenetration(1);
                templateProjectile.setNumExplosionsRemaining(1);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                templateExplosion.setShape(new GameCircle(85));
                templateExplosion.setBitmap(ImageLoader.loadImage("Explosion2.png"));
                templateExplosion.setDuration(.6);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(7);
                templateStun.setTimeMultiplier(.1);
                templateStun.setDuration(.5);
                templateExplosion.setTemplateStun(templateStun);
                break;


            case 7:

                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 40;
                ammoPerBox = 30;
                ammoRechargeRate = 1;
                this.setWarmupTime(0);
                cooldownTime = .07;
                switchToTime = 1;
                switchFromTime = 2;
                automatic = true;
                this.ownersVelocityScale = 0;
                fireWhileInactive = false;
                cooldownWhileInactive = true;
                rechargeWhileInactive = true;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameRectangle(20, 3));
                templateProjectile.setBitmap(ImageLoader.loadImage("Laser.png"));
                tempVector[0] = 50; tempVector[1] = 0;
                templateProjectile.setCenter(tempVector);
                tempVector[0] = 500; tempVector[1] = 0;
                templateProjectile.setVelocity(tempVector);
                templateProjectile.setGravity(0);
                templateProjectile.setHomingAccel(0);
                templateProjectile.setDragCoefficient(0);
                templateProjectile.setRemainingFlightTime(.5);
                templateProjectile.setPenetration(0);
                templateProjectile.setNumExplosionsRemaining(1);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                templateExplosion.setShape(new GameCircle(35));
                templateExplosion.setBitmap(ImageLoader.loadImage("Sparks.png"));
                templateExplosion.setDuration(.1);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(12);
                templateStun.setTimeMultiplier(1);
                templateStun.setDuration(0);
                templateExplosion.setTemplateStun(templateStun);
                break;


            case 8:

                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                this.maxAmmo = 16;
                this.ammoPerBox = 16;
                this.ammoRechargeRate = .25;
                this.setWarmupTime(0);
                this.cooldownTime = 1.1;
                this.switchToTime = 1;
                this.switchFromTime = 2;
                this.automatic = true;
                this.triggerIsSticky = true;
                this.ownersVelocityScale = 1;
                this.fireWhileInactive = true;
                this.cooldownWhileInactive = true;
                this.rechargeWhileInactive = true;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameCircle(60));
                templateProjectile.setBitmap(ImageLoader.loadImage("Ice.png"));
                tempVector[0] = 0; tempVector[1] = 0;
                templateProjectile.setVelocity(tempVector);
                tempVector[0] = 50; tempVector[1] = 50;
                templateProjectile.setCenter(tempVector);
                templateProjectile.setGravity(0);
                templateProjectile.setHomingAccel(100);
                templateProjectile.setDragCoefficient(0);
                templateProjectile.setRemainingFlightTime(.5);
                templateProjectile.setPenetration(0);
                templateProjectile.setNumExplosionsRemaining(1);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                templateExplosion.setShape(new GameCircle(65));
                templateExplosion.setBitmap(ImageLoader.loadImage("Ice.png"));
                templateExplosion.setDuration(4.5);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(.1);
                templateStun.setTimeMultiplier(.3);
                templateStun.setDuration(0);
                templateExplosion.setTemplateStun(templateStun);
                break;


            case 9:

                // Attributes about when you may own the item
                cost = 1600;

                // Attributes about when you may fire
                maxAmmo = 600;
                ammoPerBox = 600;
                ammoRechargeRate = 0;
                this.setWarmupTime(0);
                this.cooldownTime = .05;
                this.switchToTime = 1;
                this.switchFromTime = 2;
                this.automatic = true;
                this.triggerIsSticky = true;
                this.ownersVelocityScale = 1;
                this.fireWhileInactive = true;
                this.cooldownWhileInactive = true;
                this.rechargeWhileInactive = false;

                // Attributes of the projectile it launches, to determine when it hits
                templateProjectile = new Projectile();
                templateProjectile.setShape(new GameCircle(10));
                templateProjectile.setBitmap(ImageLoader.loadImage("Balloon.png"));
                tempVector[0] = 0; tempVector[1] = 43;
                templateProjectile.setCenter(tempVector);
                tempVector[0] = 0; tempVector[1] = 0;
                templateProjectile.setVelocity(tempVector);
                templateProjectile.setGravity(0);
                templateProjectile.setHomingAccel(0);
                templateProjectile.setDragCoefficient(0);
                templateProjectile.setRemainingFlightTime(0);
                templateProjectile.setPenetration(0);
                templateProjectile.setNumExplosionsRemaining(1);
                templateProjectile.initializeHitpoints(70);

                // Attributes of the explosions that are created
                templateExplosion = new Explosion();
                templateExplosion.setShape(new GameRectangle(110, 140));
                templateExplosion.setBitmap(ImageLoader.loadImage("Balloon.png"));
                templateExplosion.setDuration(.2);
                templateExplosion.setFriendlyFireEnabled(true);
                templateExplosion.setKnockbackAccel(-350);
                templateProjectile.setTemplateExplosion(templateExplosion);

                templateStun = new Stun();
                templateStun.setDamagePerSecond(.1);
                templateStun.setTimeMultiplier(1);
                templateStun.setDuration(0);
                templateExplosion.setTemplateStun(templateStun);
                break;


            default:
                System.Diagnostics.Trace.WriteLine("Weapon type does not exist");
                break;
        }
        this.currentAmmo = this.maxAmmo;
        this.templateProjectile.setTeamNum(this.teamNum);
        this.templateProjectile.setShooter(this);
        this.templateProjectile.setOwner(this.owner);
    }
    public Weapon(Weapon original)
    {
        this.copyFrom(original);
    }
    public void copyFrom(Weapon other)
    {
        this.teamNum = other.teamNum;

        // Attributes about when you may own the item
        this.cost = other.cost;

        // Attributes about when you may fire
        this.maxAmmo = other.maxAmmo;
        this.ammoPerBox = other.ammoPerBox;
        this.ammoRechargeRate = other.ammoRechargeRate;
        this.setWarmupTime(other.warmupTime);
        this.cooldownTime = other.cooldownTime;
        this.switchToTime = other.switchToTime;
        this.switchFromTime = other.switchFromTime;
        this.automatic = other.automatic;
        this.triggerIsSticky = other.triggerIsSticky;
        this.fireWhileInactive = other.fireWhileInactive;
        this.cooldownWhileInactive = other.cooldownWhileInactive;
        this.rechargeWhileInactive = other.rechargeWhileInactive;

        // Attributes of the projectile it launches, to determine when it hits
        this.ownersVelocityScale = other.ownersVelocityScale;
        this.templateProjectile = new Projectile(other.getTemplateProjectile());

        // Attributes about the current state of the weapon
        this.currentAmmo = other.currentAmmo;
        this.owner = other.owner;
        this.firingTimer = other.firingTimer;
    }

    // Information about when you may own the item
    public void setMaxAmmo(double numShots)
    {
	    this.maxAmmo = numShots;
    }
    public void resetForLevel()
    {
        this.resetTrigger();
        this.resetCooldown();
        this.refillAmmo();
    }
    public void refillAmmo()
    {
        this.currentAmmo = this.maxAmmo;
    }
    // refills up to maxFraction of a box worth of ammo, and returns what fraction it filled up
    public double refillAmmo(double maxFraction)
    {
        double difference = this.maxAmmo - currentAmmo;
        double increase = Math.Min(difference, maxFraction * this.ammoPerBox);
        this.currentAmmo += increase;
        if (this.ammoPerBox != 0)
            return increase / this.ammoPerBox;
        else
            return 0;
    }
    public void resetCooldown()
    {
        this.firingTimer = this.warmupTime;
    }
    public void resetTrigger()
    {
        this.triggerPressed = false;
        this.triggerDown = false;
        this.pressedRecently = false;
        this.triggerStuck = false;
    }
    public double getMaxAmmo()
    {
        return this.maxAmmo;
    }
    public double getCurrentAmmo()
    {
        return this.currentAmmo;
    }
    public void setAmmoRechargeRate(double ammoPerSecond)
    {
	    this.ammoRechargeRate = ammoPerSecond;
    }
    public double getAmmoRechargeRate()
    {
	    return this.ammoRechargeRate;
    }
    public void setAmmoPerBox(double count)
    {
        this.ammoPerBox = count;
    }
    public double getAmmoPerBox()
    {
        return this.ammoPerBox;
    }
    public void setWarmupTime(double numSeconds)
    {
	    this.warmupTime = numSeconds;
	    // when the warmup time is set, we then start the weapon in the nonfiring state
	    this.firingTimer = this.warmupTime;
    }
    public double getWarmupTime()
    {
	    return this.warmupTime;
    }
    public void setCooldownTime(double numSeconds)
    {
	    this.cooldownTime = numSeconds;
    }
    public double getCooldownTime()
    {
	    return this.cooldownTime;
    }
    public void setSwitchToTime(double numSeconds)
    {
	    this.switchToTime = numSeconds;
    }
    public double getSwitchToTime()
    {
	    return this.switchToTime;
    }
    public void setSwitchFromTime(double numSeconds)
    {
	    this.switchFromTime = numSeconds;
    }
    public double getSwitchFromTime()
    {
	    return this.switchFromTime;
    }
    public void setAutomatic(bool value)
    {
        this.automatic = value;
    }
    public bool isAutomatic()
    {
        return this.automatic;
    }
    public void setTriggerSticky(bool value)
    {
        this.triggerIsSticky = value;
    }
    public bool isTriggerSticky()
    {
        return this.triggerIsSticky;
    }
    public void setOwnersVelocityScale(double value)
    {
        this.ownersVelocityScale = value;
    }
    public double getOwnersVelocityScale()
    {
        return this.ownersVelocityScale;
    }
    public void enableFiringWhileInactive(bool enable)
    {
        this.fireWhileInactive = enable;
    }
    public bool canFireWhileInactive()
    {
        return this.fireWhileInactive;
    }
    public void enableCooldownWhileInactive(bool enable)
    {
        this.cooldownWhileInactive = enable;
    }
    public bool coolsDownWhileInactive()
    {
        return this.cooldownWhileInactive;
    }
    public void enableRechargeWhileInactive(bool enable)
    {
        this.rechargeWhileInactive = enable;
    }
    public bool rechargesWhileInactive()
    {
        return this.rechargeWhileInactive;
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
        // keep track of whether the trigger is stuck down
        if (this.triggerIsSticky && this.pressedRecently)
            this.triggerStuck = !this.triggerStuck;
        // keep track of whether the trigger is down
        if (pressed || this.triggerStuck)
            this.triggerDown = true;
        else
            this.triggerDown = false;
    }
    public bool isWarmingUp()
    {
	    if (this.firingTimer < this.warmupTime)
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
        return this.firingTimer - this.warmupTime;
    }
    public bool isCoolingDown()
    {
	    if (this.firingTimer > this.warmupTime)
		    return true;
	    else
		    return false;
    }
    // Tells whether the Character has expressed an interest in firing
    public bool wantsToFire()
    {
        bool shouldFire;
        if (automatic)
        {
            // Automatic firing. Shoot if the trigger is currently down (or was pressed sometime during the last tick)
            if (this.triggerDown || this.pressedRecently)
                shouldFire = true;
            else
                shouldFire = false;
        }
        else
        {
            // manual firing
            if (this.pressedRecently)
                shouldFire = true;
            else
                shouldFire = false;
        }
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
        if (this.shouldStartFiring() || (this.firingTimer != this.warmupTime))
        {
		    // check if it's cooling down
            if (this.firingTimer > this.warmupTime)
            {
                // If we get here, it's cooling down
                if (active || this.cooldownWhileInactive)
                {
                    busy = true;    // keep track of whether the weapon is doing anything
                    // decrease the cooldown time toward zero, so that only the warmup time remains
                    this.firingTimer = Math.Max(this.firingTimer - numSeconds, this.warmupTime);
                }
            }
            else
            {
                // If we get here, it's warming up
                if (active || this.fireWhileInactive)
                {
                    busy = true;    // keep track of whether the weapon is doing anything
                    this.firingTimer -= numSeconds;
                    // check if the projectile launches yet
                    if (this.firingTimer <= 0)
                    {
                        // update the firing timer
                        firingTimer += (warmupTime + cooldownTime);
                        // we can only fire one shot per tick
                        if (firingTimer < warmupTime)
                            firingTimer = warmupTime;

                        // create the projectile and fill in the information from the template
                        this.shot = this.makeProjectile(true);
                        // expend one ammo
                        this.currentAmmo--;
                    }
			    }		 
            }
        }
        if (this.currentAmmo < this.maxAmmo)
        {
            if (active || this.rechargeWhileInactive)
            {
                busy = true;    // keep track of whether the weapon is doing anything
                this.currentAmmo = Math.Min(this.currentAmmo + numSeconds * this.ammoRechargeRate, this.maxAmmo);
                if (currentAmmo < 0)
                    currentAmmo = 0;
            }
        }
        // tell whether the weapon is doing anything
        return busy;
    }
    // builds a projectile and doesn't count as actually firing
    public Projectile makeProjectile(bool enableRendering)
    {
        Projectile tempShot = new Projectile(this.templateProjectile, enableRendering);
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
        newV[0] += owner.getVelocity()[0] * this.ownersVelocityScale;
        newV[1] += owner.getVelocity()[1] * this.ownersVelocityScale;
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
    public void setTemplateProjectile(Projectile newProjectile)
    {
        this.templateProjectile = newProjectile;
    }
    public Projectile getTemplateProjectile()
    {
        return this.templateProjectile;
    }
    /*// Refill the ammo for the this (small) time interval
    public void rechargeAmmo(double numSeconds)
    {
	    // recharge ammo
	    this.currentAmmo = Math.Min(currentAmmo + numSeconds * this.ammoRechargeRate, this.maxAmmo);
    }*/
    // Purchasing the weapon
    public void setOwner(Character value)
    {
	    this.owner = value;
        this.teamNum = owner.getTeamNum();
        this.templateProjectile.setTeamNum(teamNum);
        this.templateProjectile.setOwner(this.owner);
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
        // compute what the attributes of the projectile would be if it were created now
        Projectile currentProjectile = this.makeProjectile(false);
        // simulate it in short intervals for a bunch of counts
        int i;
        double[] location;
        double[] desiredMove;
        double[] actualMove;
        //int count = 25;
        // determine the number of iterations and the length of each
        double numSeconds = 0.2;
        int count = Math.Min(25, (int)(currentProjectile.getRemainingFlightTime() / numSeconds));
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
            // check if it collides
            if (actualMove != desiredMove)
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
    public double calculateCost()
    {
#if false

#else
        double tempCost = 1;

        // attributes of the weapon that affect the cost
        tempCost *= (1 + maxAmmo / 10);
        if (ammoRechargeRate > 0)
            tempCost *= 3;
        tempCost *= (1 + ammoRechargeRate * 15);
        tempCost *= (1 + Math.Abs(ammoPerBox) / 8);
        double warmup = Math.Abs(warmupTime);
        double fireDuration = Math.Abs(warmupTime) + Math.Abs(cooldownTime) + 0.001;
        tempCost *= Math.Pow(.5, warmup / fireDuration);
        //tempCost *= (1 + 1 / (Math.Abs(warmupTime) + 0.001));
        tempCost *= (1 + 0.3 / fireDuration);
        //tempCost *= (1 + 0.1 / (Math.Abs(switchToTime) + 0.001));
        //tempCost *= (1 + 0.1 / (Math.Abs(switchFromTime) + 0.001));
        if (fireWhileInactive)
            tempCost *= 1.2;
        if (cooldownWhileInactive)
            tempCost *= 1.5;
        if (rechargeWhileInactive)
            tempCost *= 1.9;

        // attributes of the projectile that affect the cost
        tempCost *= (1 + Math.Abs(templateProjectile.getCenter()[0]) / 400);
        tempCost *= (1 + Math.Abs(templateProjectile.getCenter()[1]) / 400);
        tempCost *= (1 + templateProjectile.getShape().getWidth() / 150);
        tempCost *= (1 + templateProjectile.getShape().getHeight() / 150);
        //tempCost *= (1 + templateProjectile.getRemainingFlightTime() / 5);
        //tempCost *= (1 + 1 / (Math.Abs(templateProjectile.getPenetration() - 1) + 0.001));
        //tempCost *= (1 + templateProjectile.getNumExplosionsRemaining() * 2);
        double penetration = templateProjectile.getPenetration();
        penetration *= penetration; // square it because it affects both explosion duration and damage
        if (penetration == 1)
            tempCost *= (templateProjectile.getNumExplosionsRemaining() * 5 + 1);
        else
            tempCost *= (templateProjectile.getNumExplosionsRemaining() * 5 + 
                (1 - Math.Pow(penetration, templateProjectile.getNumExplosionsRemaining())) / (1 - penetration));
        tempCost *= (1 + (1 + Math.Abs(templateProjectile.getHomingAccel())) * (1 + 3 * Math.Sqrt(Math.Abs(templateProjectile.getRemainingFlightTime()))) / 300);
        // attributes of the explosion that affect the cost
        Explosion templateExplosion = templateProjectile.getTemplateExplosion();
        tempCost *= (1 + templateExplosion.getShape().getWidth() / 50);
        tempCost *= (1 + templateExplosion.getShape().getHeight() / 50);
        //tempCost *= (.1 + 2 * Math.Abs(templateExplosion.getDuration()));
        if (templateExplosion.isFriendlyFireEnabled())
            tempCost /= 2;
        tempCost *= (1 + Math.Abs(templateExplosion.getKnockbackAccel()) / 100);

        // attributes of the stun that affect the cost
        Stun templateStun = templateExplosion.getTemplateStun();
        tempCost *= (1 + 1 / (Math.Abs(templateStun.getTimeMultiplier()) + 0.001));
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


        tempCost = Math.Ceiling(tempCost / 500);


#endif
        this.cost = tempCost;
        
        return cost;
    }
    public double getCost()
    {
        return this.cost;
    }
private
	// Attributes about the type of weapon
	//BitmapImage projectileImage;
	int teamNum;

	// Attributes about when you may own the item
	double cost;

	// Attributes about when you may fire
	double maxAmmo;
	double ammoRechargeRate;
    double ammoPerBox;
	double warmupTime;
	double cooldownTime;
	double switchToTime;
	double switchFromTime;
    bool automatic;
    bool triggerIsSticky;
    bool fireWhileInactive; // Tells whether this weapon can fire without being the current weapon
    bool cooldownWhileInactive; // Tells whether this weapon can cooldown without being the current weapon
    bool rechargeWhileInactive; // Tells whether this weapon can recharge ammo without being the current weapon

	// Attributes of the projectile it launches, to determine when it hits
    double ownersVelocityScale;
    Projectile templateProjectile;
    Projectile shot;

	// Attributes about the current state of the weapon
	double currentAmmo;
	Character owner;
	double firingTimer;	// firingTimer equals warmupTime when nothing is going on. It's 0 when it fires, and it equals warmupTime + cooldownTime right after.
    bool pressedRecently;	// Tells whether the trigger transitioned from not pressed to pressed since the last tick
    bool triggerPressed;    // Tells whether the trigger is currently being pressed by the owner
    bool triggerStuck;      // Tells whether the trigger will remain pressed the next time it is released
    bool triggerDown;       // Tells whether the trigger is engaged
};