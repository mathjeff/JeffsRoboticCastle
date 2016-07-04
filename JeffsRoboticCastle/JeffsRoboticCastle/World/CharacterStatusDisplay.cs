using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

class CharacterStatusDisplay
{
// public
    public CharacterStatusDisplay(Character subject, Canvas c, Point position, Size size)
    {
        // setup a canvas to draw on
        this.canvas = new Canvas();
        canvas.Width = size.Width;
        canvas.Height = size.Height;
        canvas.RenderTransform = new TranslateTransform(position.X, position.Y);
        c.Children.Add(this.canvas);

        // add the hitpoint bar
        Size hpSize = new Size();
        hpSize.Width = size.Width;
        hpSize.Height = size.Height / 8;
        Point hpPosition = new Point();
        this.hitPointBar = new HitPointBar(subject, this.canvas, hpPosition, hpSize);

        // create a display showing the current, next, and previous weapons
        Size weaponsSize = new Size();
        weaponsSize.Width = size.Width;
        weaponsSize.Height = size.Height * 7 / 8;
        Point weaponsPosition = new Point(0, hpPosition.Y + hpSize.Height);
        this.weaponsDisplay = new SelectedWeaponsDisplay(subject, this.canvas, weaponsPosition, weaponsSize);
    }
    public void update()
    {
        this.hitPointBar.update();
        this.weaponsDisplay.update();
    }
// private
    Canvas canvas;
    HitPointBar hitPointBar;
    SelectedWeaponsDisplay weaponsDisplay;
};