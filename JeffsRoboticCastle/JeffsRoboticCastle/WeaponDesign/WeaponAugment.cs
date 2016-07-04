using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// A WeaponAugment is an individual instance of a WeaponAugmentTemplate
// It can be assigned to a Weapon to improve its Stats
namespace Castle.WeaponDesign
{
    class WeaponAugment : Receivable
    {
        public WeaponAugment(WeaponAugmentTemplate template)
        {
            this.Template = template;
        }
        public WeaponAugmentTemplate Template;
        public WeaponConfiguration AssignedWeapon;
        public void GiveTo(GamePlayer player)
        {
            player.AddWeaponAugment(this);
        }
        public void AssignTo(WeaponConfiguration configuration)
        {
            this.Unassign();
            if (configuration != null)
            {
                this.AssignedWeapon = configuration;
                configuration.Augments.Add(this);
            }
        }
        public void Unassign()
        {
            if (this.AssignedWeapon != null)
            {
                this.AssignedWeapon.Augments.Remove(this);
            }
            this.AssignedWeapon = null;
        }
        public override string ToString()
        {
            return this.Template.ToString();
        }

    }
}
