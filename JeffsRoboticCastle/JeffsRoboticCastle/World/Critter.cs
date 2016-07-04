using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

class Critter : Character
{
    public Critter(double[] location, int type)
    {
        BitmapImage startingImage = null;
        double[] accel;
        this.setTeamNum(0);
        // #define DESIGN_HERE
        switch (type)
        {
            case 0:
                startingImage = ImageLoader.loadImage("Farmer.png");
                this.setShape(new GameRectangle(20, 43));
                accel = new double[2]; accel[0] = 300; accel[1] = 0;
                this.setMaxAccel(accel);
                this.setDragCoefficient(3);
                this.setGravity(1000);
                this.initializeHitpoints(5);
                this.setJumpSpeed(950);
                this.setContactDamagePerSecond(0);
                break;
           default:
                break;
        }
        this.setCenter(location);
        this.setBitmap(startingImage);
        this.setImageOffset(new double[2]);
        this.setBrain(new AI(0));
    }
}