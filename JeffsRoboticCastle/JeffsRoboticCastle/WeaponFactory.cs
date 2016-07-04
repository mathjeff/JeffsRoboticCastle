using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*class WeaponFactory
{
    public WeaponFactory()
    {
    }
    public BasicWeapon BasicWeapon
    {
        get
        {
            return new BasicWeapon();
        }
    }
    public void addDefaultWeapons()
    {
        this.weapons = new List<WeaponConfiguration>();
        double[] tempVector = new double[2];
        Projectile templateProjectile;
        Explosion templateExplosion;
        BasicWeapon baseWeapon = this.BasicWeapon;
        WeaponConfiguration configuration;

        Stun templateStun;


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Add a new weapon
        configuration = new WeaponConfiguration(baseWeapon, new List<WeaponAugment>);
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
    public void addWeapon(Weapon newWeapon)
    {
        // fill in some additional Stats so we don't need to specify it every time
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
    private List<WeaponConfiguration> weapons;
}*/