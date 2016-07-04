using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

class HitPointBar
{
// public
    public HitPointBar(Character subject, Canvas canvas, Point position, Size size)
    {
        if (subject == null)
            throw new ArgumentException("Character cannot be null");
        this.character = subject;

        this.backgroundBar = new Rectangle();
        backgroundBar.Width = size.Width;
        backgroundBar.Height = size.Height;
        backgroundBar.Fill = Brushes.GhostWhite;
        backgroundBar.RenderTransform = new TranslateTransform(position.X, position.Y);
        canvas.Children.Add(backgroundBar);

        this.valueBar = new Rectangle();
        valueBar.Width = size.Width;
        valueBar.Height = size.Height;
        valueBar.Fill = Brushes.Red;
        valueBar.RenderTransform = new TranslateTransform(position.X, position.Y);
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