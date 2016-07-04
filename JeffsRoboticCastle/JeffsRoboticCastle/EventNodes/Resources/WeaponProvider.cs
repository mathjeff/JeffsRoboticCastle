using Castle.Generics;
using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.EventNodes.Rewards
{
    class WeaponProvider : ValueProvider<Receivable>
    {
        public WeaponProvider(BasicWeapon template)
        {
            this.template = template;
        }
        public BasicWeapon Get()
        {
            return this.template.Clone();
        }
        public Receivable Get(Receivable outputType)
        {
            return this.Get();
        }
        BasicWeapon template = new BasicWeapon();
    }
}
