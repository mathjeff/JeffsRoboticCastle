#define WEAPON_SEARCH_TREE_VISUAL
#if !WEAPON_SEARCH_TREE_VISUAL
#define WEAPON_SCROLLWHEEL_VISUAL
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

class SelectedWeaponsDisplay
{
// public
    public SelectedWeaponsDisplay(Canvas newCanvas, double[] position, double[] size)
    {
#if WEAPON_SCROLLWHEEL_VISUAL
        this.weapons = new WeaponStatusDisplay[3];
        int i;
        double[] weaponPosition = new double[2];
        position.CopyTo(weaponPosition, 0);
        double[] weaponSize = new double[2];
        weaponSize[0] = size[0];
        weaponSize[1] = size[1] / 3;
        for (i = 0; i < weapons.Length; i++)
        {
            this.weapons[i] = new WeaponStatusDisplay(newCanvas, subject, weaponPosition, weaponSize, i - 1);
            weaponPosition[1] += size[1] / 3;
        }
#endif
#if WEAPON_SEARCH_TREE_VISUAL
        double[] childPosition = new double[position.Length];
        position.CopyTo(childPosition, 0);
        double[] childSize = new double[size.Length];
        size.CopyTo(childSize, 0);
        //this.grids = new WeaponGridDisplay[subject.getWeaponTreeBranchFactor()];
        this.grids = new WeaponGridDisplay[3];
        childSize[0] /= (1.5 * grids.Length - .5);
        int i;
        for (i = 0; i < grids.Length; i++)
        {
            grids[i] = new WeaponGridDisplay(newCanvas, childPosition, childSize);
            childPosition[0] += 1.5 * childSize[0];
        }
        //this.rightGrid = new WeaponGridDisplay(newCanvas, childPosition, childSize);
#endif
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