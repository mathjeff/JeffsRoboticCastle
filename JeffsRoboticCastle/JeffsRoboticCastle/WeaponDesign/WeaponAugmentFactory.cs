using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Makes WeaponAugmentTemplate objects

// Each augment should approximately double the effectiveness of its weapon
namespace Castle.WeaponDesign
{
    class WeaponAugmentFactory
    {
        public WeaponAugmentFactory()
        {
            this.augments = new List<WeaponAugmentTemplate>() {this.Warmer, this.Cooler, this.AmmoFiller,
                this.Tracker, this.Flier, this.ReExploder, this.AreaAffecter, this.Shielder, this.Damager, this.Slower};
        }
        public IEnumerable<WeaponAugmentTemplate> All
        {
            get
            {
                return this.augments;
            }
        }
        private WeaponAugmentTemplate New(String name)
        {
            WeaponAugmentTemplate template = new WeaponAugmentTemplate(name);
            return template;
        }
        public WeaponAugmentTemplate Warmer
        {
            // Ideally the decreased warmup time will double the hit ratio
            get
            {
                WeaponAugmentTemplate warmer = this.New("Faster warmup");
                warmer.WarmupRate = 10;
                return warmer;
            }
        }

        public WeaponAugmentTemplate Cooler
        {
            // AmmoFiller and Cooler combined should give 4X throughput for the same duration
            get
            {
                WeaponAugmentTemplate cooler = this.New("Faster cooldown");
                cooler.WarmupRate = 0.333;
                cooler.CooldownRate = 100;
                return cooler;

            }
        }

        public WeaponAugmentTemplate AmmoFiller
        {
            // AmmoFiller and Cooler combined should give 4X throughput for the same duration
            get
            {
                WeaponAugmentTemplate persister = this.New("More ammo");
                persister.MaxAmmo = 300;
                return persister;

            }
        }

        public WeaponAugmentTemplate Tracker
        {
            // Ideally should result in about double the hit ratio
            get
            {
                WeaponAugmentTemplate tracker = this.New("Homing on targets");
                tracker.HomingAccel = 400;
                tracker.FlightDuration = 1;
                return tracker;
            }
        }

        public WeaponAugmentTemplate Flier
        {
            // Ideally should result in about double the hit ratio
            get
            {
                WeaponAugmentTemplate flier = this.New("Longer projectile flight durations");
                flier.FlightDuration = 7;
                flier.HomingAccel = 50;
                return flier;
            }
        }

        public WeaponAugmentTemplate AreaAffecter
        {
            // Ideally should result in about twice as many enemies being hit
            get
            {
                WeaponAugmentTemplate areaAffecter = this.New("Larger Explosions");
                areaAffecter.ExplosionRadius = 100;
                return areaAffecter;
            }
        }

        public WeaponAugmentTemplate ReExploder
        {
            // Ideally should result in about twice as many explosions happening
            get
            {
                WeaponAugmentTemplate reExploder = this.New("More explosions per projectile");
                reExploder.MaxNumExplosions = 3;
                reExploder.FlightDuration = 1;
                return reExploder;
            }
        }

        public WeaponAugmentTemplate Shielder
        {
            // Should result in double damage if they actually stay in the explosion througout its duration
            // Should improve protection from incoming projectiles too
            get
            {
                WeaponAugmentTemplate poisoner = this.New("Longer explosion duration");
                poisoner.ExplosionDuration = 3;
                return poisoner;
            }
        }

        public WeaponAugmentTemplate Damager
        {
            // Should result in double damage
            get
            {
                WeaponAugmentTemplate damager = this.New("More damage");
                damager.StunDamagePerSecond = 1;
                return damager;
            }
        }

        public WeaponAugmentTemplate Slower
        {
            // Should slow the opponent down to an average of one-quarter speed over 3 seconds
            get
            {
                WeaponAugmentTemplate slower = this.New("More slowdown applied to affected targets");
                slower.TimestopWeight = 3;
                return slower;
            }
        }

        public BasicWeapon BasicWeapon
        {
            get
            {
                BasicWeapon weapon = new BasicWeapon();
                weapon.NumAugmentSlots = 5;
                WeaponAugmentTemplate stats = new WeaponAugmentTemplate();
                stats = new WeaponAugmentTemplate();
                stats.Name = "Basic weapon";
                stats.MaxAmmo = 100;
                stats.WarmupRate = 1;
                stats.CooldownRate = 0.5;
                stats.HomingAccel = 0;
                stats.FlightDuration = 1;
                stats.MaxNumExplosions = 1;
                stats.ExplosionRadius = 10;
                stats.ExplosionDuration = 1;
                stats.StunDamagePerSecond = 1;
                stats.StunDuration = 2;
                stats.TimestopWeight = 0;
                weapon.BaseStats = stats;
                weapon.ProjectileBitmap = ImageLoader.loadImage("fireball3.png");
                weapon.ExplosionBitmap = ImageLoader.loadImage("explosion2.png");
                return weapon;
            }
        }

        private List<WeaponAugmentTemplate> augments;

    }
}
