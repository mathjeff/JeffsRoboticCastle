using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        public override void Show(System.Windows.Size screenSize)
        {
            base.Show(screenSize);

            TextBlock block = new TextBlock();
            block.Text = this.Text;
            block.FontSize = 24;
            block.TextWrapping = TextWrapping.Wrap;
            block.MaxWidth = screenSize.Width;
            this.screen.getCanvas().Children.Add(block);
            this.screen.getCanvas().Background = new SolidColorBrush(Colors.LightGray);

        }
        public String Text;
    }
}
