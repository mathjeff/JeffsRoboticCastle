using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Castle.EventNodes.Editing
{
    class WeaponDesignEventNode : EventNode
    {
        public WeaponDesignEventNode(Player player)
        {
            this.player = player;
        }
        public void Show(Size size)
        {
            this.screen = new WeaponDesignScreen(size, this.player);
        }
        public Screen GetScreen()
        {
            return this.screen;
        }
        public EventNode TimerTick(double numSeconds)
        {
            this.screen.update();
            if (this.screen.WantsDemo)
                return this.NextNode;
            if (this.screen.Done)
                return this.NextNode;
            return this;
        }
        public EventNode NextNode;
        private WeaponDesignScreen screen;
        private Player player;
    }
}
