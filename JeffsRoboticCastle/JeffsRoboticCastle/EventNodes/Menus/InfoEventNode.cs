using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

// Just shows one screen and then proceeds
namespace Castle.EventNodes.Menus
{
    class InfoEventNode : EventNode
    {
        public static InfoEventNode FromImage(String fileName)
        {
            return new InfoEventNode(new StaticImageProvider(fileName));
        }
        public static InfoEventNode FromText(String message)
        {
            return new TextInfoEventNode(message);
        }
        public InfoEventNode()
        {
        }
        public InfoEventNode(ValueConverter<Size, Image> imageProvider)
        {
            this.backgroundImageProvider = imageProvider;
        }
        public EventNode NextNode;
        public void Show(Size screenSize)
        {
            this.screen = new InfoScreen();
            this.screen.Initialize(screenSize);
            if (this.backgroundImageProvider != null)
                this.screen.setBackgroundImage(this.backgroundImageProvider.convert(screenSize));
        }
        public Screen GetScreen()
        {
            return this.screen;
        }
        public EventNode TimerTick(double numSeconds)
        {
            if (this.screen.isDone())
                return this.NextNode;
            return this;
        }
        protected InfoScreen screen;
        private ValueConverter<Size, Image> backgroundImageProvider;

    }
}
