using Castle.EventNodes.Menus;
using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

// Gives the player a reward
namespace Castle.Menus
{
    class RewardEventNode : TextInfoEventNode
    {
        public RewardEventNode(double magnitude, GamePlayer player)
        {
            this.player = player;
            this.magnitude = magnitude;
        }
        public override void Show(Size screenSize)
        {
            Reward reward = new Reward();
            foreach (Patron patron in this.player.GetPatrons())
            {
                reward = reward.Plus(patron.GenerateReward(this.magnitude));
            }
            this.Text = "Your family is thinking of you and sent " + reward + ".";
            base.Show(screenSize);
            reward.ApplyTo(this.player);
        }
        private GamePlayer player;
        double magnitude;
    }
}
