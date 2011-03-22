using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

class HitPointBar
{
// public
    public HitPointBar(Canvas canvas, Character subject, double[] position, double[] size)
    {
        this.character = subject;

        this.backgroundBar = new Rectangle();
        backgroundBar.Width = size[0];
        backgroundBar.Height = size[1];
        backgroundBar.Fill = Brushes.GhostWhite;
        backgroundBar.RenderTransform = new TranslateTransform(position[0], position[1]);
        canvas.Children.Add(backgroundBar);

        this.valueBar = new Rectangle();
        valueBar.Width = size[0];
        valueBar.Height = size[1];
        valueBar.Fill = Brushes.Red;
        valueBar.RenderTransform = new TranslateTransform(position[0], position[1]);
        canvas.Children.Add(valueBar);
        
        this.update();
    }
    public void update()
    {
        this.setValue(character.getHitpoints() / character.getMaxHitpoints());
    }
    public void setValue(double value)
    {
        if (value < 0)
            value = 0;
        this.valueBar.Width = this.backgroundBar.Width * value;
    }
    // private
    Character character;
    Rectangle valueBar;
    Rectangle backgroundBar;
}