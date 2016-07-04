using Castle.Generics;
using Castle.Language;
using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// A Patron repeatedly gives Receivable objects to the LevelPlayer as they finish levels
namespace Castle
{
    class Patron
    {
        public Patron(ValueProvider<Receivable> itemProvider, double pendingRewardQuantity, double repeatedRewardQuantity)
        {
            this.ItemProvider = itemProvider;
            this.PendingRewardQuantity = pendingRewardQuantity;
            this.RepeatedRewardQuantity = repeatedRewardQuantity;
        }
        // Attributes of each Patron
        ValueProvider<Receivable> ItemProvider;
        double RepeatedRewardQuantity;
        double PendingRewardQuantity;

        public Reward GenerateReward(double rewardSize)
        {
            LinkedList<Receivable> items = new LinkedList<Receivable>();
            this.PendingRewardQuantity += this.RepeatedRewardQuantity * rewardSize;
            int numGifts = (int)Math.Floor(this.PendingRewardQuantity);
            while (this.PendingRewardQuantity >= 1)
            {
                items.AddLast(this.newItem());
                this.PendingRewardQuantity -= 1;
            }
            return new Reward(items);
        }
        public bool Active
        {
            get
            {
                return (this.RepeatedRewardQuantity > 0 || this.PendingRewardQuantity >= 1);
            }
        }
        public override string ToString()
        {
            String result = "";
            List<String> components = new List<string>();
            if (this.PendingRewardQuantity > 0)
            {
                components.Add(LanguageUtils.FormatQuantity(this.PendingRewardQuantity, this.newItem().ToString()) + " now");
            }
            if (this.RepeatedRewardQuantity > 0)
            {
                components.Add(LanguageUtils.FormatQuantity(this.RepeatedRewardQuantity, this.newItem().ToString()) + " per day");
            }
            result += LanguageUtils.Enumerate(components);
            return result;
        }
        private Receivable newItem()
        {
            return this.ItemProvider.Get((Receivable)null);
        }
    }
}
