using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

class CharacterStatusDisplay
{
// public
    public CharacterStatusDisplay(Canvas newCanvas, double[] position, double[] size, Character subject)
    {
        // setup a canvas to draw on
        this.canvas = new Canvas();
        canvas.Width = size[0];
        canvas.Height = size[1];
        canvas.RenderTransform = new TranslateTransform(position[0], position[1]);
        newCanvas.Children.Add(this.canvas);

        // add the hitpoint bar
        double[] hpSize = new double[2];
        hpSize[0] = size[0];
        hpSize[1] = size[1] / 8;
        double[] hpPosition = new double[2];
        hpPosition[0] = 0;
        hpPosition[1] = 0;
        this.hitPointBar = new HitPointBar(this.canvas, subject, hpPosition, hpSize);

        // create a display showing the current, next, and previous weapons
        double[] weaponsSize = new double[2];
        weaponsSize[0] = size[0];
        weaponsSize[1] = size[1] * 7 / 8;
        double[] weaponsPosition = new double[2];
        weaponsPosition[0] = 0;
        weaponsPosition[1] = hpPosition[1] + hpSize[1];
        this.weaponsDisplay = new SelectedWeaponsDisplay(this.canvas, subject, weaponsPosition, weaponsSize);
    }
    public void update()
    {
        this.hitPointBar.update();
        this.weaponsDisplay.update();
    }
// private
    Canvas canvas;
    Character character;
    HitPointBar hitPointBar;
    SelectedWeaponsDisplay weaponsDisplay;
};