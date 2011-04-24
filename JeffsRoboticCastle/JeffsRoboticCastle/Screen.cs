using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

class Screen
{
// public
    public Screen()
    {
    }
    public Screen(Canvas c, double[] screenSize)
    {
        this.initialize(c, screenSize);
    }
    public Screen(Canvas c, double[] screenPosition, double[] screenSize)
    {
        this.initialize(c, screenPosition, screenSize);
    }
    public virtual void initialize(Canvas c, double[] screenSize)
    {
        double[] screenPosition = new double[2];
        this.initialize(c, screenPosition, screenSize);
    }
    public virtual void initialize(Canvas c, double[] screenPosition, double[] screenSize)
    {
        this.parentCanvas = c;
        // save size
        this.size = screenSize;
        // setup a canvas to draw on
        this.canvas = this.makeCanvas(screenPosition, screenSize);
    }
    // make the menu appear
    public virtual void show()
    {
        this.parentCanvas.Children.Add(this.canvas);
    }
    // make the menu disappear
    public void hide()
    {
        this.parentCanvas.Children.Remove(this.canvas);
    }
    // update anything on the screen that needs updating, and then return the next screen to go to
    public virtual Screen timerTick(double numSeconds)
    {
        return this;
    }
    public Canvas getCanvas()
    {
        return this.canvas;
    }
    public Canvas getParentCanvas()
    {
        return this.parentCanvas;
    }
    public Canvas makeCanvas(double[] position, double[] size)
    {
        Canvas c = new Canvas();
        c = new Canvas();
        c.Width = size[0];
        c.Height = size[1];
        c.RenderTransform = new TranslateTransform(position[0], position[1]);
        c.ClipToBounds = true;
        return c;
    }
    // this function gets called when a key is pressed
    public virtual void KeyDown(object sender, KeyEventArgs e)
    {
    }
    // this functoin gets called when a key is released
    public virtual void KeyUp(object sender, KeyEventArgs e)
    {
    }
    public double[] getSize()
    {
        return this.size;
    }
    public double getTop(Control control)
    {
        return control.RenderTransform.Transform(new Point(0, 0)).Y;
    }
    public double getLeft(Control control)
    {
        return control.RenderTransform.Transform(new Point(0, 0)).X;
    }
    public double getBottom(Control control)
    {
        return control.RenderTransform.Transform(new Point(0, 0)).Y + control.Height;
    }
    public double getRight(Control control)
    {
        return control.RenderTransform.Transform(new Point(0, 0)).X + control.Width;
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
    Canvas parentCanvas;
    Canvas canvas;
    double[] size;
}