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

// Shows the status of all selected weapons
class SelectedWeaponsDisplay
{
// public
    public SelectedWeaponsDisplay(Character subject, Canvas newCanvas, Point position, Size size)
    {
        Point childPosition = new Point(position.X, position.Y);
        Size childSize = new Size(size.Width, size.Height);
        this.grid = new WeaponGridDisplay(newCanvas, childPosition, childSize);
        this.followCharacter(subject);
    }
    public void followCharacter(Character subject)
    {
        this.character = subject;
        this.update();
    }
    public void update()
    {
        this.grid.update(this.character.Weapons);
    }
// private
    Character character;
    WeaponGridDisplay grid;
}