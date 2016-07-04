using Castle.Generics;
using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.EventNodes.Rewards
{
    class WeaponAugmentProvider : ValueProvider<Receivable>
    {
        public WeaponAugmentProvider(WeaponAugmentTemplate template)
        {
            this.template = template;
        }
        public WeaponAugment Get()
        {
            return new WeaponAugment(this.template);
        }
        public Receivable Get(Receivable outputType)
        {
            return this.Get();
        }
        WeaponAugmentTemplate template;
    }
}
