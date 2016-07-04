using Castle.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// A WeaponConfiguration is an assembly of pieces by the player, that end up forming a weapon
// It's contains a BasicWeapon and a list of WeaponAugment
namespace Castle.WeaponDesign
{
    class WeaponConfiguration
    {
        public WeaponConfiguration(BasicWeapon basicWeapon, List<WeaponAugment> augments)
        {
            this.basicWeapon = basicWeapon;
            this.augments = augments;
        }
        public WeaponStats GetStats()
        {
            List<WeaponAugmentTemplate> augmentTemplates = new List<WeaponAugmentTemplate>();
            foreach (WeaponAugment augment in this.augments)
            {
                augmentTemplates.Add(augment.Template);
            }
            return this.basicWeapon.WithAugments(augmentTemplates);
        }
        public Weapon MakeWeapon()
        {
            return new Weapon(this.GetStats());
        }
        public override string ToString()
        {
            String result = this.basicWeapon.ToString();
            basicWeapon.ToString();
            if (this.augments.Count() < 1)
            {
                result += " with 0 items";
            }
            else
            {
                List<String> components = new List<string>();
                foreach (WeaponAugment augment in this.augments)
                {
                    components.Add(augment.ToString());
                }
                result += " with " + LanguageUtils.FormatQuantity(this.augments.Count(), "augment") + 
                    ": " + LanguageUtils.Enumerate(components);
            }
            return result;
        }
        public List<WeaponAugment> Augments
        {
            get
            {
                return this.augments;
            }
        }
        public WeaponAugment GetAugmentAtIndex(int i)
        {
            if (i >= 0 && i < this.augments.Count)
                return this.augments[i];
            return null;
        }
        public BasicWeapon BasicWeapon
        {
            get
            {
                return this.basicWeapon;
            }
        }
        private BasicWeapon basicWeapon;
        private List<WeaponAugment> augments;
    }
}
