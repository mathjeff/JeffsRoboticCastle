using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

class WeaponGridDisplay
{
// public
    public WeaponGridDisplay(Canvas newCanvas, double[] position, double[] dimensions)
    {
        this.canvas = newCanvas;
        this.screenPosition = new double[position.Length];
        position.CopyTo(screenPosition, 0);
        this.size = new double[dimensions.Length];
        dimensions.CopyTo(this.size, 0);
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
            int[] countsPerSide = new int[2];
            countsPerSide[0] = (int)Math.Ceiling(Math.Sqrt((double)numWeapons));
            countsPerSide[1] = (int)Math.Ceiling((double)numWeapons / countsPerSide[0]);
            double[] spacing = new double[size.Length];
            int i, xIndex, yIndex;
            double[] position = new double[size.Length];
            for (i = 0; i < spacing.Length; i++)
            {
                position[i] = this.screenPosition[i];
                spacing[i] = this.size[i] / countsPerSide[i];
            }
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
                if (yIndex >= countsPerSide[1])
                {
                    yIndex = 0;
                    xIndex++;
                }
                position[0] = this.screenPosition[0] + xIndex * spacing[0];
                position[1] = this.screenPosition[1] + yIndex * spacing[1];
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
    double[] screenPosition;
    double[] size;
    Canvas canvas;
}
