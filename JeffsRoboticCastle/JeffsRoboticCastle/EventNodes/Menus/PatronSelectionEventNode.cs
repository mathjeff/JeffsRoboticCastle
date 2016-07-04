using Castle.EventNodes.Customization;
using Castle.EventNodes.Rewards;
using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Castle.EventNodes.Menus
{
    class PatronSelectionEventNode : LinearSelectionEventNode<Patron>
    {
        public PatronSelectionEventNode(GamePlayer player, WeaponAugmentFactory weaponAugmentFactory)
        {
            if (player == null)
                throw new ArgumentException("Player cannot be null");
            this.player = player;
            LinkedList<Patron> patrons = new LinkedList<Patron>();
            foreach (WeaponAugmentTemplate weaponAugmentTemplate in weaponAugmentFactory.All)
            {
                Patron patron = new Patron(new WeaponAugmentProvider(weaponAugmentTemplate), 0, 1);
                patrons.AddLast(patron);
            }
            patrons.AddLast(new Patron(new WeaponProvider(weaponAugmentFactory.BasicWeapon), 0, 1));
            this.Choices = new ChoiceBuilder<Patron>(patrons);

            this.Prompt = "Choose a house to be born into. Your family will love you and will repeatedly"
                + " gift you an item of matching their specialty.";
        }

        private GamePlayer player;
        //private WeaponAugmentFactory weaponAugmentFactory;

        public override void OnChoice(Patron choice)
        {
            this.player.AddPatron(choice);
        }
    }
}
