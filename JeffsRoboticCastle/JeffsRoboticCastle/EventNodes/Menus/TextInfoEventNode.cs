using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Castle.EventNodes.Menus
{
    class TextInfoEventNode : InfoEventNode
    {
        public TextInfoEventNode()
        {
        }
        public TextInfoEventNode(String text)
        {
            this.Text = text;
        }
        public virtual void Show(Size screenSize)
        {
            this.screen = new InfoScreen();
            this.screen.Initialize(screenSize);

            TextBlock block = new TextBlock();
            block.Text = this.Text;
            this.screen.getCanvas().Children.Add(block);

        }
        public String Text;
    }
}
