using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Castle.WeaponDesign;
using Castle;

// A LevelPlayer is the representation of the Player inside the World
// A GamePlayer is the representation of the representation of the Player inside the storyline
// Essentially the difference is that a LevelPlayer has a location and things like that
class LevelPlayer : Character
{
// public
    // constructor
    public LevelPlayer(GamePlayer source, double[] location, List<Weapon> weapons)
    {
        this.initialize(source);
        this.setCenter(location);
        foreach (Weapon weapon in weapons)
        {
            this.addWeapon(weapon);
        } 
    }
    public LevelPlayer(LevelPlayer original)
    {
        this.initialize(original.source);
        this.copyFrom(original);
    }
    public new void initialize(GamePlayer source)
    {
        this.source = source;
        double[] offset = new double[2]; offset[0] = 0; offset[1] = 0;
        this.setImageOffset(offset);
        this.setShape(new GameRectangle(30, 43));
        BitmapImage startingImage = ImageLoader.loadImage("player1.png");
        this.setGravity(1000);
        this.setJumpSpeed(900);
        this.setBitmap(startingImage);
        double[] accel = new double[2]; accel[0] = 600; accel[1] = 0;
        this.setMaxAccel(accel);
        this.setDragCoefficient(3);
        this.setTeamNum(2);
        this.initializeHitpoints(source.Hitpoints);
    }
    
    
    //List<Weapon> weapons;
    
    GamePlayer source;
    
};