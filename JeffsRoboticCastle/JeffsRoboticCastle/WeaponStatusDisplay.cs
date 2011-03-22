using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

class WeaponStatusDisplay
{
// public
    //public WeaponStatusDisplay(Canvas newCanvas, Character subject, double[] position, double[] size, int relativeIndex)
    public WeaponStatusDisplay(Canvas newCanvas, double[] position, double[] size, Weapon newWeapon)
    {
        //this.character = subject;
        //this.indexOffset = relativeIndex;
        // The image of the weapon
        this.weaponImage = new Image();
        this.weaponImage.Height = size[1];
        this.weaponImage.Width = Math.Min(size[1], size[0] / 2);
        this.weaponImage.RenderTransform = new TranslateTransform(position[0], position[1] + (size[1] - weaponImage.Height) / 2);
        newCanvas.Children.Add(this.weaponImage);
        // A background for the ammo bar
        this.ammoBarBackground = new Rectangle();
        this.ammoBarBackground.RenderTransform = new TranslateTransform(position[0] + weaponImage.Width, position[1]);
        this.ammoBarBackground.Fill = Brushes.Gray;
        this.ammoBarBackground.Width = size[0] - weaponImage.Width;
        this.ammoBarBackground.Height = size[1];
        newCanvas.Children.Add(this.ammoBarBackground);
        // An ammo bar for the weapon
        this.ammoBar = new Rectangle();
        this.ammoBar.RenderTransform = new TranslateTransform(position[0] + weaponImage.Width, position[1]);
        this.ammoBar.Fill = Brushes.AliceBlue;
        this.ammoBar.Width = size[0] - weaponImage.Width;
        this.ammoBar.Height = size[1];
        newCanvas.Children.Add(this.ammoBar);
        this.canvas = newCanvas;
        this.setWeapon(newWeapon);
    }
    public void setWeapon(Weapon newWeapon)
    {
        if (newWeapon != this.currentWeapon)
        {
            // update the weapon image
            Projectile templateProjectile = newWeapon.getTemplateProjectile();
            BitmapImage bitmap = templateProjectile.getBitmap();
            this.weaponImage.Source = bitmap;
            //this.weaponImage.Width = templateProjectile.getShape().getWidth();
            //this.weaponImage.Height = templateProjectile.getShape().getHeight();
            this.currentWeapon = newWeapon;
            this.update();
        }
    }
    public void update()
    {
        //Weapon newWeapon = this.character.getCurrentWeaponShiftedByIndex(this.indexOffset);
        double value = this.currentWeapon.getCurrentAmmo() / this.currentWeapon.getMaxAmmo();
        this.ammoBar.Width = (value * this.ammoBarBackground.Width);
    }
    public void remove()
    {
        this.canvas.Children.Remove(this.weaponImage);
        this.canvas.Children.Remove(this.ammoBar);
        this.canvas.Children.Remove(this.ammoBarBackground);
    }
// private
    Canvas canvas;
    Image weaponImage;
    //Character character;
    //int indexOffset;
    Weapon currentWeapon;
    Rectangle ammoBar;
    Rectangle ammoBarBackground;
}