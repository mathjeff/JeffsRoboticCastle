#define WEAPON_SEARCH_TREE_VISUAL
#if !WEAPON_SEARCH_TREE_VISUAL
#define WEAPON_SCROLLWHEEL_VISUAL
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

class SelectedWeaponsDisplay
{
// public
    public SelectedWeaponsDisplay(Character subject, Canvas newCanvas, Point position, Size size)
    {
#if WEAPON_SCROLLWHEEL_VISUAL
        this.weapons = new WeaponStatusDisplay[3];
        int i;
        double[] weaponPosition = new double[2];
        position.CopyTo(weaponPosition, 0);
        double[] weaponSize = new double[2];
        weaponsize.Width = size.Width;
        weaponsize.Height = size.Height / 3;
        for (i = 0; i < weapons.Length; i++)
        {
            this.weapons[i] = new WeaponStatusDisplay(newCanvas, subject, weaponPosition, weaponSize, i - 1);
            weaponPosition[1] += size.Height / 3;
        }
#endif
#if WEAPON_SEARCH_TREE_VISUAL
        Point childPosition = new Point(position.X, position.Y);
        Size childSize = new Size(size.Width, size.Height);
        this.grids = new WeaponGridDisplay[3];
        childSize.Width /= (1.5 * grids.Length - .5);
        int i;
        for (i = 0; i < grids.Length; i++)
        {
            grids[i] = new WeaponGridDisplay(newCanvas, childPosition, childSize);
            childPosition.X += 1.5 * childSize.Width;
        }
        //this.rightGrid = new WeaponGridDisplay(newCanvas, childPosition, childSize);
#endif
        this.followCharacter(subject);
    }
    public void followCharacter(Character subject)
    {
        this.character = subject;
        this.update();
    }
    public void update()
    {
#if WEAPON_SCROLLWHEEL_VISUAL
        foreach (WeaponStatusDisplay display in this.weapons)
        {
            display.update();
        }
#endif
#if WEAPON_SEARCH_TREE_VISUAL
        //this.leftGrid.update(character.getLeftWeaponTree());
        //this.rightGrid.update(character.getRightWeaponTree());
        int i;
        for (i = 0; i < this.grids.Length; i++)
        {
            grids[i].update(this.character.getWeaponTreeAtIndex(i));
        }
#endif
    }
// private
    Character character;
#if WEAPON_SCROLLWHEEL_VISUAL
    WeaponStatusDisplay[] weapons;
#endif
#if WEAPON_SEARCH_TREE_VISUAL
    WeaponGridDisplay[] grids;
#endif
}