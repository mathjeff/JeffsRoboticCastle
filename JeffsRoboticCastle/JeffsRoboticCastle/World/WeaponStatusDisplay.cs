using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

// Shows the status of a single Weapon
class WeaponStatusDisplay
{
// public
    //public WeaponStatusDisplay(Canvas newCanvas, Character subject, double[] position, double[] size, int relativeIndex)
    public WeaponStatusDisplay(Canvas newCanvas, Point position, Size size, Weapon newWeapon)
    {
        //this.character = subject;
        //this.indexOffset = relativeIndex;
        // The image of the weapon
        this.weaponImage = new Image();
        this.weaponImage.Height = size.Height;
        this.weaponImage.Width = Math.Min(size.Height, size.Width / 2);
        this.weaponImage.RenderTransform = new TranslateTransform(position.X, position.Y + (size.Height - weaponImage.Height) / 2);
        newCanvas.Children.Add(this.weaponImage);
        // A background for the ammo bar
        this.ammoBarBackground = new Rectangle();
        this.ammoBarBackground.RenderTransform = new TranslateTransform(position.X + weaponImage.Width, position.Y);
        this.ammoBarBackground.Fill = Brushes.Gray;
        this.ammoBarBackground.Width = size.Width - weaponImage.Width;
        this.ammoBarBackground.Height = size.Height;
        newCanvas.Children.Add(this.ammoBarBackground);
        // An ammo bar for the weapon
        this.ammoBar = new Rectangle();
        this.ammoBar.RenderTransform = new TranslateTransform(position.X + weaponImage.Width, position.Y);
        this.ammoBar.Fill = Brushes.AliceBlue;
        this.ammoBar.Width = size.Width - weaponImage.Width;
        this.ammoBar.Height = size.Height;
        newCanvas.Children.Add(this.ammoBar);
        // a status bar indicating how soon the weapon will fire
        this.firingBar = new Rectangle();
        this.firingBar.RenderTransform = new TranslateTransform(position.X + weaponImage.Width, position.Y);
        this.firingBar.Width = size.Width - weaponImage.Width;
        this.firingBar.Height = size.Height / 3;
        newCanvas.Children.Add(firingBar);
        this.canvas = newCanvas;
        this.setWeapon(newWeapon);
    }
    public void setWeapon(Weapon newWeapon)
    {
        if (newWeapon != this.currentWeapon)
        {
            // update the weapon image
            Projectile templateProjectile = newWeapon.Stats.TemplateProjectile;
            ImageSource bitmap = templateProjectile.getBitmap();
            if (bitmap == null)
            {
                throw new ArgumentException("Weapon image cannot be null");
            }
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
        // update the bar that shows the current firing timer
        double maxDuration;
        double currentDuration;
        if (currentWeapon.isWarmingUp())
        {
            maxDuration = currentWeapon.getWarmupTime();
            currentDuration = maxDuration - currentWeapon.getRemainingWarmup();
            this.firingBar.Fill = Brushes.Blue;
        }
        else
        {
            maxDuration = currentWeapon.getCooldownTime();
            currentDuration = currentWeapon.getRemainingCooldown();
            this.firingBar.Fill = Brushes.Yellow;
        }
        if (maxDuration <= 0)
            maxDuration = 1;
        this.firingBar.Width = this.ammoBarBackground.Width * currentDuration / maxDuration;

        // update the bar that shows the current ammo
        double value = this.currentWeapon.getCurrentAmmo() / this.currentWeapon.getMaxAmmo();
        this.ammoBar.Width = (value * this.ammoBarBackground.Width);
    }
    public void remove()
    {
        this.canvas.Children.Remove(this.weaponImage);
        this.canvas.Children.Remove(this.ammoBar);
        this.canvas.Children.Remove(this.ammoBarBackground);
        this.canvas.Children.Remove(this.firingBar);
    }
// private
    Canvas canvas;
    Image weaponImage;
    //Character character;
    //int indexOffset;
    Weapon currentWeapon;
    Rectangle ammoBar;
    Rectangle ammoBarBackground;
    Rectangle firingBar;
}