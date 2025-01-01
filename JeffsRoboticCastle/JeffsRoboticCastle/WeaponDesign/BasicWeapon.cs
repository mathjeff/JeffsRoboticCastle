using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// A BasicWeapon is just a weapon with no WeaponAugment attached
// It knows how to compute the Stats of a weapon based on a list of WeaponAugment objects
// Most likely the player will only ever have one type of BasicWeapon, but they'll have many copies of it
namespace Castle.WeaponDesign
{
    class BasicWeapon : Receivable
    {
        public String Name;

        public WeaponAugmentTemplate BaseStats;

        // Adds the given items and returns the resultant Weapon
        public WeaponStats WithAugments(IEnumerable<WeaponAugmentTemplate> augments)
        {
            // Add up all the Stats of all the items
            WeaponAugmentTemplate overallAugment = this.BaseStats;
            foreach (WeaponAugmentTemplate augment in augments)
            {
                overallAugment = overallAugment.Plus(augment);
            }

            // Now do some last computations on the totals (like converting from rates to times)
            WeaponStats weapon = new WeaponStats();

            // Attributes about when you may fire
            weapon.OwnersVelocityScale = overallAugment.OwnerVelocityFraction;
            weapon.MaxAmmo = overallAugment.MaxAmmo;
            weapon.WarmupTime = 1 / overallAugment.WarmupRate;
            weapon.CooldownTime = 1 / overallAugment.CooldownRate;
            
            // Attributes of the projectile it launches, to determine when it hits
            Projectile templateProjectile = new Projectile();
            templateProjectile.setBoomerangAccel(overallAugment.BoomerangAccel);
            templateProjectile.setCenter(this.projectileStartPosition);
            templateProjectile.setVelocity(this.projectileStartVelocity);
            templateProjectile.setRemainingFlightTime(overallAugment.FlightDuration);
            templateProjectile.setPenetration(0.5);
            templateProjectile.setNumExplosionsRemaining(overallAugment.MaxNumExplosions);
            templateProjectile.setHomingAccel(overallAugment.HomingAccel);
            templateProjectile.enableHomingOnCharacters(true);
            templateProjectile.enableHomingOnProjectiles(true);
            templateProjectile.setShape(this.ProjectileShape);
            templateProjectile.setBitmap(this.ProjectileBitmap);
            templateProjectile.setGravity(overallAugment.Gravity);
            templateProjectile.setDragCoefficient(overallAugment.AirResistance);

            weapon.TemplateProjectile = templateProjectile;

            // Attributes of the explosion it creates, to determine what happens in the area it hits
            Explosion templateExplosion = new Explosion();
            templateExplosion.setDuration(overallAugment.ExplosionDuration);
            templateExplosion.setFriendlyFireEnabled(false);
            templateExplosion.setKnockbackAccel(overallAugment.KnockbackAccel);
            templateExplosion.setShape(new GameCircle(overallAugment.ExplosionRadius));
            templateExplosion.setBitmap(this.ExplosionBitmap);
            templateExplosion.setFriendlyFireEnabled(overallAugment.FriendlyFireEnabled);

            templateProjectile.setTemplateExplosion(templateExplosion);

            // Attributes of the stun that gets applied to characters caught in the explosion
            Stun templateStun = new Stun();
            templateStun.setTimeMultiplier(1.0 / (overallAugment.TimestopWeight + 1));
            templateStun.setDamagePerSecond(overallAugment.StunDamagePerSecond);
            templateStun.setAmmoDrain(0);
            templateStun.setDuration(overallAugment.StunDuration);

            templateExplosion.setTemplateStun(templateStun);

            return weapon;
        }

        public BasicWeapon()
        {
            this.ProjectileShape = new GameCircle(10);
            this.projectileStartPosition = new double[] { 100, 0 };
            this.projectileStartVelocity = new double[] { 10, 0 };
        }

        public BasicWeapon(WeaponStats basicStats)
        {
            this.BaseStats = new WeaponAugmentTemplate();
            this.BaseStats.MaxAmmo = basicStats.MaxAmmo;
            this.BaseStats.WarmupRate = 1 / (basicStats.WarmupTime + 0.0000001);
            this.BaseStats.CooldownRate = 1 / (basicStats.CooldownTime + 0.0000001);

            this.projectileStartPosition = basicStats.TemplateProjectile.getCenter();
            this.projectileStartVelocity = basicStats.TemplateProjectile.getVelocity();
            this.BaseStats.OwnerVelocityFraction = basicStats.OwnersVelocityScale;
            this.ProjectileBitmap = basicStats.TemplateProjectile.getBitmap();
            this.ProjectileShape = basicStats.TemplateProjectile.getShape();
            this.BaseStats.HomingAccel = basicStats.TemplateProjectile.getHomingAccel();
            this.BaseStats.BoomerangAccel = basicStats.TemplateProjectile.getBoomerangAccel();
            this.BaseStats.AirResistance = basicStats.TemplateProjectile.getDragCoefficient();
            this.BaseStats.MaxNumExplosions = basicStats.TemplateProjectile.getNumExplosionsRemaining();
            this.BaseStats.FlightDuration = basicStats.TemplateProjectile.getRemainingFlightTime();
            this.BaseStats.Gravity = basicStats.TemplateProjectile.getGravity();

            this.BaseStats.ExplosionRadius = basicStats.TemplateProjectile.getTemplateExplosion().getShape().getWidth() / 2;
            this.ExplosionBitmap = basicStats.TemplateProjectile.getTemplateExplosion().getBitmap();
            this.BaseStats.ExplosionDuration = basicStats.TemplateProjectile.getTemplateExplosion().getDuration();
            this.BaseStats.StunDamagePerSecond = basicStats.TemplateProjectile.getTemplateExplosion().getTemplateStun().getDamagePerSecond();
            this.BaseStats.TimestopWeight = 1.0 / basicStats.TemplateProjectile.getTemplateExplosion().getTimeMultiplier() - 1;
            this.BaseStats.KnockbackAccel = basicStats.TemplateProjectile.getTemplateExplosion().getKnockbackAccel();
            this.BaseStats.FriendlyFireEnabled = basicStats.TemplateProjectile.getTemplateExplosion().isFriendlyFireEnabled();

        }

        public BasicWeapon Clone()
        {
            BasicWeapon weapon = new BasicWeapon();
            weapon.copyFrom(this);
            return weapon;
        }
        private void copyFrom(BasicWeapon other)
        {
            this.NumAugmentSlots = other.NumAugmentSlots;
            this.BaseStats = other.BaseStats;
            this.ProjectileBitmap = other.ProjectileBitmap;
            this.ExplosionBitmap = other.ExplosionBitmap;
        }
        public void GiveTo(GamePlayer player)
        {
            player.WeaponConfigurations.Add(new WeaponConfiguration(this, new List<WeaponAugment>()));
        }

        public override string ToString()
        {
            return "Weapon";
        }

        public int NumAugmentSlots;
        public BitmapImage ProjectileBitmap;
        public BitmapImage ExplosionBitmap;

        private GameShape ProjectileShape;
        private double[] projectileStartPosition;
        private double[] projectileStartVelocity;
    }
}
