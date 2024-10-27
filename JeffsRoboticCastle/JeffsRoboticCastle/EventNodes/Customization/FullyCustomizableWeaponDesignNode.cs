using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Castle.EventNodes.Customization
{
    class FullyCustomizableWeaponDesignNode : EventNode
    {
        public FullyCustomizableWeaponDesignNode(System.Windows.Size screenSize, GamePlayer player)
        {
            this.screen = new FullyCustomizableWeaponDesignScreen(screenSize, player);
        }

        public void Show(System.Windows.Size screenSize)
        {
            
        }

        public Screen GetScreen()
        {
            return this.screen;
        }

        public EventNode TimerTick(double duration)
        {
            if (this.screen.Done)
                return this.NextNode;
            return this;
        }

        public EventNode NextNode;


        FullyCustomizableWeaponDesignScreen screen;
    }
}
