using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class WeaponFactory
{
    public WeaponFactory()
    {
    }
    public void addDefaultWeapons()
    {
        this.weapons = new List<Weapon>();
        double[] tempVector = new double[2];
        Projectile templateProjectile;
        Explosion templateExplosion;
        Weapon newWeapon;
        Stun templateStun;
        // #define DESIGN_HERE


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Add a new weapon
        newWeapon = new Weapon();

        // Attributes about when you may own the weapon
        newWeapon.setCost(1600);

        // Attributes about when you may fire
        newWeapon.setMaxAmmo(15);
        newWeapon.setAmmoPerBox(15);
        //newWeapon.setAmmoRechargeRate(0.222);
        newWeapon.setReloadTime(3);
        newWeapon.setAmmoReloadRate(0.667);
        newWeapon.setWarmupTime(0);
        newWeapon.setCooldownTime(0.5);
        newWeapon.setAutomatic(true);
        newWeapon.setOwnersVelocityScale(0);
        newWeapon.enableFiringWhileInactive(false);
        newWeapon.enableCooldownWhileInactive(false);
        newWeapon.enableRechargeWhileInactive(false);

        // Attributes of the projectile it launches, to determine when it hits
        templateProjectile = new Projectile();
        templateProjectile.setShape(new GameCircle(10));
        templateProjectile.setBitmap(ImageLoader.loadImage("Ice.png"));
        tempVector[0] = 100; tempVector[1] = 0;
        templateProjectile.setVelocity(tempVector);
        templateProjectile.setGravity(0);
        templateProjectile.setHomingAccel(280);
        templateProjectile.setDragCoefficient(0.8);
        templateProjectile.setRemainingFlightTime(5);
        templateProjectile.setPenetration(0);
        templateProjectile.setNumExplosionsRemaining(1);
        newWeapon.setTemplateProjectile(templateProjectile);

        // Attributes of the explosions that are created
        templateExplosion = new Explosion();
        templateExplosion.setShape(new GameCircle(20));
        templateExplosion.setBitmap(ImageLoader.loadImage("Ice.png"));
        templateExplosion.setDuration(0.1);
        templateExplosion.setKnockbackAccel(0);
        templateProjectile.setTemplateExplosion(templateExplosion);

        // Attributes of what happens when a Character gets hit
        templateStun = new Stun();
        templateStun.setDamagePerSecond(0.12);
        templateStun.setTimeMultiplier(0.04);
        templateStun.setDuration(6);
        templateExplosion.setTemplateStun(templateStun);

        // Add this new weapon
        this.addWeapon(newWeapon);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Add a new weapon
        newWeapon = new Weapon();

        // Attributes about when you may own the weapon
        newWeapon.setCost(1600);

        // Attributes about when you may fire
        newWeapon.setMaxAmmo(5);
        newWeapon.setAmmoPerBox(5);
        //newWeapon.setAmmoRechargeRate(0.5);
        newWeapon.setReloadTime(5);
        newWeapon.setAmmoReloadRate(1);
        newWeapon.setWarmupTime(0);
        newWeapon.setCooldownTime(2);
        newWeapon.setAutomatic(false);
        newWeapon.setOwnersVelocityScale(2);
        newWeapon.enableFiringWhileInactive(false);
        newWeapon.enableCooldownWhileInactive(true);
        newWeapon.enableRechargeWhileInactive(false);

        // Attributes of the projectile it launches, to determine when it hits
        templateProjectile = new Projectile();
        templateProjectile.setShape(new GameCircle(20));
        templateProjectile.setBitmap(ImageLoader.loadImage("Fireball3.png"));
        tempVector[0] = 0; tempVector[1] = 0;
        templateProjectile.setCenter(tempVector);
        tempVector[0] = 400; tempVector[1] = 0;
        templateProjectile.setVelocity(tempVector);
        templateProjectile.setGravity(0);
        templateProjectile.setHomingAccel(400);
        templateProjectile.setDragCoefficient(-.4);
        templateProjectile.setRemainingFlightTime(3);
        templateProjectile.setPenetration(1);
        templateProjectile.setNumExplosionsRemaining(7);
        newWeapon.setTemplateProjectile(templateProjectile);

        // Attributes of the explosions that are created
        templateExplosion = new Explosion();
        templateExplosion.setShape(new GameCircle(60));
        templateExplosion.setBitmap(ImageLoader.loadImage("Explosion2.png"));
        templateExplosion.setDuration(0.2);
        templateExplosion.setKnockbackAccel(1000);
        templateProjectile.setTemplateExplosion(templateExplosion);

        // Attributes of what happens when a Character gets hit
        templateStun = new Stun();
        templateStun.setDamagePerSecond(2.25);
        templateStun.setTimeMultiplier(1);
        templateStun.setDuration(0.6);
        templateExplosion.setTemplateStun(templateStun);

        // Add this new weapon
        this.addWeapon(newWeapon);


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Add a new weapon
        newWeapon = new Weapon();

        // Attributes about when you may own the weapon
        newWeapon.setCost(1600);

        // Attributes about when you may fire
        newWeapon.setMaxAmmo(10);
        newWeapon.setAmmoPerBox(10);
        //newWeapon.setAmmoRechargeRate(0.333);
        newWeapon.setReloadTime(2);
        newWeapon.setAmmoReloadRate(1);
        newWeapon.setWarmupTime(0);
        newWeapon.setCooldownTime(0.5);
        newWeapon.setAutomatic(false);
        newWeapon.setOwnersVelocityScale(0);
        newWeapon.enableFiringWhileInactive(false);
        newWeapon.enableCooldownWhileInactive(false);
        newWeapon.enableRechargeWhileInactive(false);

        // Attributes of the projectile it launches, to determine when it hits
        templateProjectile = new Projectile();
        templateProjectile.setShape(new GameCircle(10));
        templateProjectile.setBitmap(ImageLoader.loadImage("Wasp.png"));
        tempVector[0] = 0; tempVector[1] = 0;
        templateProjectile.setCenter(tempVector);
        tempVector[0] = 300; tempVector[1] = 0;
        templateProjectile.setVelocity(tempVector);
        templateProjectile.setGravity(0);
        templateProjectile.setHomingAccel(1200);
        templateProjectile.setDragCoefficient(0.8);
        templateProjectile.setRemainingFlightTime(10);
        templateProjectile.setPenetration(1);
        templateProjectile.setNumExplosionsRemaining(3);
        templateProjectile.enableHomingOnProjectiles(true);
        newWeapon.setTemplateProjectile(templateProjectile);

        // Attributes of the explosions that are created
        templateExplosion = new Explosion();
        templateExplosion.setShape(new GameCircle(40));
        templateExplosion.setBitmap(ImageLoader.loadImage("Explosion.png"));
        templateExplosion.setDuration(.3);
        templateProjectile.setTemplateExplosion(templateExplosion);

        // Attributes of what happens when a Character gets hit
        templateStun = new Stun();
        templateStun.setDamagePerSecond(2);
        templateStun.setTimeMultiplier(1);
        templateStun.setDuration(0);
        templateExplosion.setTemplateStun(templateStun);

        // Add this new weapon
        this.addWeapon(newWeapon);


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Add a new weapon
        newWeapon = new Weapon();

        // Attributes about when you may own the weapon
        newWeapon.setCost(1600);

        // Attributes about when you may fire
        newWeapon.setMaxAmmo(10);
        newWeapon.setAmmoPerBox(10);
        //newWeapon.setAmmoRechargeRate(1000);
        newWeapon.setReloadTime(2);
        newWeapon.setAmmoReloadRate(2);
        newWeapon.setWarmupTime(1);
        newWeapon.setCooldownTime(0);
        newWeapon.setAutomatic(false);
        newWeapon.setOwnersVelocityScale(0);
        newWeapon.enableFiringWhileInactive(true);
        newWeapon.enableCooldownWhileInactive(true);
        newWeapon.enableRechargeWhileInactive(false);

        // Attributes of the projectile it launches, to determine when it hits
        templateProjectile = new Projectile();
        templateProjectile.setShape(new GameCircle(20));
        templateProjectile.setBitmap(ImageLoader.loadImage("Missile.png"));
        tempVector[0] = 0; tempVector[1] = 0;
        templateProjectile.setVelocity(tempVector);
        templateProjectile.setGravity(0);
        templateProjectile.setHomingAccel(1500);
        templateProjectile.setDragCoefficient(.7);
        templateProjectile.setRemainingFlightTime(3);
        templateProjectile.setPenetration(1);
        templateProjectile.setNumExplosionsRemaining(1);
        newWeapon.setTemplateProjectile(templateProjectile);

        // Attributes of the explosions that are created
        templateExplosion = new Explosion();
        templateExplosion.setShape(new GameCircle(30));
        templateExplosion.setBitmap(ImageLoader.loadImage("Explosion2.png"));
        templateExplosion.setDuration(0.1);
        templateProjectile.setTemplateExplosion(templateExplosion);

        // Attributes of what happens when a Character gets hit
        templateStun = new Stun();
        templateStun.setDamagePerSecond(20);
        templateStun.setTimeMultiplier(1);
        templateStun.setDuration(0);
        templateExplosion.setTemplateStun(templateStun);

        // Add this new weapon
        this.addWeapon(newWeapon);


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Add a new weapon
        newWeapon = new Weapon();

        // Attributes about when you may own the weapon
        newWeapon.setCost(1600);

        // Attributes about when you may fire
        newWeapon.setMaxAmmo(10);
        newWeapon.setAmmoPerBox(10);
        //newWeapon.setAmmoRechargeRate(1);
        newWeapon.setReloadTime(0.5);
        newWeapon.setAmmoReloadRate(2);
        newWeapon.setWarmupTime(0);
        newWeapon.setCooldownTime(0.5);
        newWeapon.setAutomatic(false);
        newWeapon.setOwnersVelocityScale(3);
        newWeapon.enableFiringWhileInactive(true);
        newWeapon.enableCooldownWhileInactive(true);
        newWeapon.enableRechargeWhileInactive(false);

        // Attributes of the projectile it launches, to determine when it hits
        templateProjectile = new Projectile();
        templateProjectile.setShape(new GameCircle(10));
        templateProjectile.setBitmap(ImageLoader.loadImage("Shield.png"));
        tempVector[0] = 0; tempVector[1] = 0;
        templateProjectile.setCenter(tempVector);
        tempVector[0] = 100; tempVector[1] = 0;
        templateProjectile.setVelocity(tempVector);
        templateProjectile.setGravity(0);
        templateProjectile.setHomingAccel(200);
        templateProjectile.setBoomerangAccel(0);
        templateProjectile.setDragCoefficient(2);
        templateProjectile.setRemainingFlightTime(2);
        templateProjectile.setPenetration(0);
        templateProjectile.setNumExplosionsRemaining(1);
        newWeapon.setTemplateProjectile(templateProjectile);

        // Attributes of the explosions that are created
        templateExplosion = new Explosion();
        templateExplosion.setShape(new GameCircle(50));
        templateExplosion.setBitmap(ImageLoader.loadImage("Shield.png"));
        templateExplosion.setDuration(1.5);
        templateExplosion.setFriendlyFireEnabled(false);
        templateProjectile.setTemplateExplosion(templateExplosion);

        // Attributes of what happens when a Character gets hit
        templateStun = new Stun();
        templateStun.setDamagePerSecond(2);
        templateStun.setTimeMultiplier(1);
        templateStun.setDuration(0);
        templateExplosion.setTemplateStun(templateStun);

        // Add this new weapon
        this.addWeapon(newWeapon);
    }
    /*
    // Previous weapon data   
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
    */
    public void addWeapon(Weapon newWeapon)
    {
        // fill in some additional stats so we don't need to specify it every time
        newWeapon.refillAmmo();
        this.weapons.Add(newWeapon);
    }
    public Weapon makeWeapon(int weaponType)
    {
        return new Weapon(this.weapons[weaponType]);
    }
    public int getNumWeapons()
    {
        return this.weapons.Count;
    }
    private List<Weapon> weapons;
}