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

using Castle.WeaponDesign;

class WeaponStats
{
    public WeaponConfiguration Configuration;

    // Attributes about when you may fire
    public double MaxAmmo;
    public double WarmupTime;
    public double CooldownTime;
    
    // Attributes of the projectile it launches, to determine when it hits
    public double OwnersVelocityScale;
    public Projectile TemplateProjectile;
    
};