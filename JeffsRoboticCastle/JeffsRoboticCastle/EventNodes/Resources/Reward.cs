using Castle.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.WeaponDesign
{
    class Reward
    {
        public Reward()
        {
            this.Items = new LinkedList<Receivable>();
        }
        public Reward(IEnumerable<Receivable> augments)
        {
            this.Items = augments;
        }
        public Reward Plus(Reward other)
        {
            return new Reward(this.Items.Concat(other.Items));
        }
        public IEnumerable<Receivable> Items;
        public void ApplyTo(GamePlayer player)
        {
            foreach (Receivable item in this.Items)
            {
                item.GiveTo(player);
            }
        }
        public override string ToString()
        {
            List<String> components = new List<string>();
            foreach (Receivable augment in this.Items)
            {
                components.Add(augment.ToString());
            }
            return LanguageUtils.Enumerate(components);
        }
    }
}
