using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
//using System.Drawing;
using System.Windows.Media.Imaging;

class MenuScreen : Screen
{
// public
    public MenuScreen()
    {
    }
    public MenuScreen(Canvas c, double[] screenSize)
    {
        this.initialize(c, screenSize);
    }
    public override void initialize(Canvas c, double[] screenSize)
    {
        this.setNextScreen(this);
        base.initialize(c, screenSize);
    }
    public override void show()
    {
        this.desiredScreen = this;
        this.keyDown = false;
        base.show();
    }
    public void setBackgroundImage(System.Windows.Controls.Image image)
    {
        if (this.backgroundImage != null)
        {
            this.getCanvas().Children.Remove(this.backgroundImage);
        }
        this.backgroundImage = image;
        if (image != null)
        {
            this.getCanvas().Children.Add(image);
        }
    }
    public void setBackgroundBitmap(BitmapImage bitmap)
    {
        Image newImage = new Image();
        newImage.Source = bitmap;
        //newImage.Stretch = System.Windows.Media.Stretch.Uniform;
        newImage.Stretch = System.Windows.Media.Stretch.Fill;
        newImage.Width = this.getSize()[0];
        newImage.Height = this.getSize()[1];
        this.setBackgroundImage(newImage);
    }
    public void setNextScreen(Screen next)
    {
        this.nextScreen = next;
    }
    // tells which Screen will be shown whenever this screen is done
    public Screen getNextScreen()
    {
        return this.nextScreen;
    }
    public override Screen timerTick(double numSeconds)
    {
        return this.desiredScreen;
    }
    public override void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        this.keyDown = true;
    }
    public override void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (this.keyDown)
        {
            // go to the next screen at the next timer tick
            setDesiredScreen(this.nextScreen);
        }
    }
    public virtual void requestToExit(object sender, EventArgs e)
    {
        this.setDesiredScreen(this.getNextScreen());
    }

    public void setDesiredScreen(Screen newScreen)
    {
        this.desiredScreen = newScreen;
    }
    // private
    Screen nextScreen;
    System.Windows.Controls.Image backgroundImage;
    bool keyDown;
    Screen desiredScreen;
}