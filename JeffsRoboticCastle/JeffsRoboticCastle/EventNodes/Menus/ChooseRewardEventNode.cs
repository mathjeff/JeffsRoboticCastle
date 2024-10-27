using Castle.EventNodes.Customization;
using Castle.EventNodes.Rewards;
using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.EventNodes.Menus
{
    class ChooseRewardEventNode : LinearSelectionEventNode<Patron>
    {
        public ChooseRewardEventNode(GamePlayer player, WeaponAugmentFactory weaponAugmentFactory)
        {
            if (player == null)
                throw new ArgumentException("Player cannot be null");
            this.player = player;
            LinkedList<Patron> patrons = new LinkedList<Patron>();
            foreach (WeaponAugmentTemplate weaponAugmentTemplate in weaponAugmentFactory.All)
            {
                Patron patron = new Patron(new WeaponAugmentProvider(weaponAugmentTemplate), 1, 0);
                patrons.AddLast(patron);
            }
            patrons.AddLast(new Patron(new WeaponProvider(weaponAugmentFactory.BasicWeapon), 1, 0));
            this.Choices = new ChoiceBuilder<Patron>(patrons);

            this.Prompt = "Choose an item";
        }

        private GamePlayer player;

        public override void OnChoice(Patron choice)
        {
            choice.GenerateReward(1).ApplyTo(this.player);
        }
    }
}
