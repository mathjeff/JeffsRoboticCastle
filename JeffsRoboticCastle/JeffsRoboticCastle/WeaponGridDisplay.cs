using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

class WeaponGridDisplay
{
// public
    public WeaponGridDisplay(Canvas newCanvas, Point position, Size size)
    {
        this.canvas = newCanvas;
        this.screenPosition = new Point(position.X, position.Y);
        this.size = new Size(size.Width, size.Height);
        this.displays = new List<WeaponStatusDisplay>();
    }
    public void update(System.Collections.Generic.List<Weapon> weaponsList)
    {
        // rearrange the icons if something changed
        if (weaponsList != previousWeaponList)
        {
            // remove the previous set of weapon status displays
            foreach (WeaponStatusDisplay display in this.displays)
            {
                display.remove();
            }
            this.displays.Clear();
            // recalculate the spacing for the weapon status displays
            int numWeapons = weaponsList.Count;
            if (numWeapons > 0)
            {
                int xsPerSide = (int)Math.Ceiling(Math.Sqrt((double)numWeapons));
                int ysPerSide = (int)Math.Ceiling((double)numWeapons / xsPerSide);
                Size spacing = new Size(this.size.Width / xsPerSide, this.size.Height / ysPerSide);
                int i, xIndex, yIndex;
                Point position = new Point(this.screenPosition.X, this.screenPosition.Y);

                // add each weapon to the screen
                Weapon currentWeapon;
                xIndex = yIndex = 0;
                for (i = 0; i < weaponsList.Count; i++)
                {
                    currentWeapon = weaponsList[i];
                    // add the weapon to the screen
                    this.displays.Add(new WeaponStatusDisplay(this.canvas, position, spacing, currentWeapon));
                    // compute the position of the next one
                    yIndex++;
                    if (yIndex >= ysPerSide)
                    {
                        yIndex = 0;
                        xIndex++;
                    }
                    position.X = this.screenPosition.X + xIndex * spacing.Width;
                    position.Y = this.screenPosition.Y + yIndex * spacing.Height;
                }
            }
            // save the list of weapons
            this.previousWeaponList = weaponsList;
        }
        // make sure each display is up-to-date
        foreach (WeaponStatusDisplay display in this.displays)
        {
            display.update();
        }
    }
    public void remove()
    {
        foreach (WeaponStatusDisplay display in this.displays)
        {
            display.remove();
        }
    }
// private
    System.Collections.Generic.List<Weapon> previousWeaponList;
    System.Collections.Generic.List<WeaponStatusDisplay> displays;
    Point screenPosition;
    Size size;
    Canvas canvas;
}
