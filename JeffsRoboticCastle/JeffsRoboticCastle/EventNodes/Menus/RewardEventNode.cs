using Castle.EventNodes.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Castle.Menus
{
    class RewardEventNode : TextInfoEventNode
    {
        public RewardEventNode(int moneyToGain, Player player)
        {
            this.moneyToGain = moneyToGain;
            this.Text = "Congratulations! You receive " + moneyToGain + " gold pieces.";
        }
        public override void Show(Size screenSize)
        {
            base.Show(screenSize);
            this.player.addMoney(this.moneyToGain);
        }
        private Player player;
        int moneyToGain;
    }
}
