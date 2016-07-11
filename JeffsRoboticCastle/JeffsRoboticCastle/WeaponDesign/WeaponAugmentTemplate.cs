using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// A WeaponAugmentTemplate can be added to a weapon to improve its statistics

namespace Castle.WeaponDesign
{
    class WeaponAugmentTemplate
    {
        // levelAttributes of each individual WeaponAugmentTemplate
        public WeaponAugmentTemplate() { }
        public WeaponAugmentTemplate(String name) { this.Name = name; }
        public WeaponAugmentTemplate(WeaponAugmentTemplate original)
        {
            this.Name = original.Name;
            this.WarmupRate = original.WarmupRate;
            this.CooldownRate = original.CooldownRate;
            this.MaxAmmo = original.MaxAmmo;
            this.FlightDuration = original.FlightDuration;
            this.MaxNumExplosions = original.MaxNumExplosions;
            this.ExplosionDuration = original.ExplosionDuration;
            this.StunDuration = original.StunDuration;
            this.StunDamagePerSecond = original.StunDamagePerSecond;
            this.TimestopWeight = original.TimestopWeight;
        }
        public WeaponAugmentTemplate Plus(WeaponAugmentTemplate other)
        {
            WeaponAugmentTemplate result = new WeaponAugmentTemplate();
            result.Name = this.Name + " + " + other.Name;
            result.WarmupRate = this.WarmupRate + other.WarmupRate;
            result.CooldownRate = this.CooldownRate + other.CooldownRate;
            result.MaxAmmo = this.MaxAmmo + other.MaxAmmo;
            result.HomingAccel += this.HomingAccel + other.HomingAccel;
            result.FlightDuration = this.FlightDuration + other.FlightDuration;
            result.MaxNumExplosions = this.MaxNumExplosions + other.MaxNumExplosions;
            result.ExplosionRadius = this.ExplosionRadius + other.ExplosionRadius;
            result.ExplosionDuration = this.ExplosionDuration + other.ExplosionDuration;
            result.StunDuration = this.StunDuration + other.StunDuration;
            result.StunDamagePerSecond = this.StunDamagePerSecond + other.StunDamagePerSecond;
            result.TimestopWeight = this.TimestopWeight + other.TimestopWeight;
            return result;
        }

        public String Name;

        public double WarmupRate;
        public double CooldownRate;
        public double MaxAmmo;
        public double HomingAccel;
        public double FlightDuration;
        public int MaxNumExplosions;
        public double ExplosionRadius;
        public double ExplosionDuration;
        public double StunDuration;
        public double StunDamagePerSecond;
        public double TimestopWeight; // weight X means time for the target passes 1/(X+1) as quckly
        //public double PenetrationWeight; // weight X means each explosion is X/(X+1) as powerful as the previous
        //public double MaxBoomerangAccel;
        //public double OwnerVelocityFraction;
        //public double MaxStartingVelocity;
        //public double MaxGravity;
        //public double MaxAirResistance;
        //public double MaxProjectileSize;


        public override string ToString()
        {
            return this.Name;
        }
    }

}
