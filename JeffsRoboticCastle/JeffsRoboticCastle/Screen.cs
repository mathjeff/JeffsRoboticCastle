using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

abstract class Screen
{
// public
    public Screen()
    {
    }
    public Screen(Size screenSize)
    {
        this.Initialize(screenSize);
    }
    public void Initialize(Size screenSize)
    {
        Point screenPosition = new Point();
        this.Initialize(screenPosition, screenSize);
    }
    public virtual void Initialize(Point screenPosition, Size screenSize)
    {        
        // save size
        if (screenSize.Width <= 0)
            throw new ArgumentException("Screen width (" + screenSize.Width + ") must be positive");
        if (screenSize.Height <= 0)
            throw new ArgumentException("Screen height (" + screenSize.Height + ") must be positive");
        this.size = screenSize;
        // setup a canvas to draw on
        this.canvas = this.makeCanvas(screenPosition, screenSize);
    }
    
    public Canvas getCanvas()
    {
        return this.canvas;
    }
    protected Canvas makeCanvas(Point position, Size size)
    {
        Canvas c = new Canvas();
        c = new Canvas();
        c.Width = size.Width;
        c.Height = size.Height;
        c.ClipToBounds = true;
        return c;
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

    // this function gets called when a key is pressed
    public virtual void KeyDown(object sender, KeyEventArgs e) { }
    // this function gets called when a key is released
    public virtual void KeyUp(object sender, KeyEventArgs e) { }
    public Size getSize()
    {
        return this.size;
    }
    public double getTop(Control control)
    {
        return control.RenderTransform.Transform(new System.Windows.Point(0, 0)).Y;
    }
    public double getLeft(Control control)
    {
        return control.RenderTransform.Transform(new System.Windows.Point(0, 0)).X;
    }
    public double getBottom(Control control)
    {
        return control.RenderTransform.Transform(new System.Windows.Point(0, 0)).Y + control.Height;
    }
    public double getRight(Control control)
    {
        return control.RenderTransform.Transform(new System.Windows.Point(0, 0)).X + control.Width;
    }
    public void addControl(Control control, double x, double y, double width, double height)
    {
        control.RenderTransform = new TranslateTransform(x, y);
        control.Width = width;
        control.Height = height;
        this.canvas.Children.Add(control);
    }
    public double parseDouble(TextBox field)
    {
        if (field.Text == "")
            field.Text = "0";
        if ((field.Text == ".") || (field.Text == "-"))
            return 0;
        return Double.Parse(field.Text);
    }
    public int parseInt(TextBox field)
    {
        if (field.Text == "")
            field.Text = "0";
        return Int32.Parse(field.Text);
    }


// private
    //Canvas parentCanvas;
    Canvas canvas;
    Size size;
    Image backgroundImage;
}