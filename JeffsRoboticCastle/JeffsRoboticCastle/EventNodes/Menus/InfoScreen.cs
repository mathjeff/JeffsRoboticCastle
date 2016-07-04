using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// Just shows the same image all the time
namespace Castle.EventNodes.Menus
{
    class InfoScreen : Screen
    {
        public override void Initialize(Point screenPosition, Size screenSize)
        {
            this.was_keyDown_pressed = this.done = false;
            base.Initialize(screenPosition, screenSize);
            Image image = new Image();
            this.getCanvas().MouseUp += InfoScreen_MouseUp;
        }

        void InfoScreen_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.done = true;
        }
        // public
        
        public override void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            this.was_keyDown_pressed = true;
        }
        public override void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (this.was_keyDown_pressed)
                this.done = true;
        }
        public Boolean isDone()
        {
            return this.done;
        }

        // private
        bool was_keyDown_pressed;
        bool done;
    }
}
