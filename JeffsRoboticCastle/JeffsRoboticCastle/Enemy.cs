using System.Windows.Media.Imaging;
class Enemy : Character
{
public
    // constructor for an enemy
	Enemy(double[] location, int type, int weapon1, int weapon2)
    {
        BitmapImage startingImage = null;
        double[] accel;
        this.setTeamNum(1);
        // #define DESIGN_HERE
        switch (type)
        {
            case 0:
                startingImage = ImageLoader.loadImage("horseman.png");
	            this.setCenter(location);
                this.setShape(new GameRectangle(94, 110));
                accel = new double[2]; accel[0] = 300; accel[1] = 0;
	            this.setMaxAccel(accel);
	            this.setDragCoefficient(1);
	            this.enemyType = type;
                this.addWeapon(new Weapon(weapon1));
                this.addWeapon(new Weapon(weapon2));
                this.setGravity(1000);
                break;
            case 1:
                startingImage = ImageLoader.loadImage("eagle.png");
                this.setCenter(location);
                this.setShape(new GameRectangle(56, 43));
                accel = new double[2]; accel[0] = 250; accel[1] = 500;
                this.setMaxAccel(accel);
                this.setDragCoefficient(3);
                this.enemyType = type;
                this.addWeapon(new Weapon(weapon1));
                this.addWeapon(new Weapon(weapon2));
                this.setGravity(50);
                break;
            case 2:
                startingImage = ImageLoader.loadImage("swarm.png");
                this.setCenter(location);
                this.setShape(new GameRectangle(16, 9));
                accel = new double[2]; accel[0] = 200; accel[1] = 0;
                this.setMaxAccel(accel);
                this.setDragCoefficient(3);
                this.enemyType = type;
                this.addWeapon(new Weapon(weapon1));
                this.addWeapon(new Weapon(weapon2));
                this.setGravity(1000);
                this.initializeHitpoints(5);
                break;
            default:
                break;
        }
        this.setContactDamagePerSecond(2);
        this.setBitmap(startingImage);
        this.setImageOffset(new double[2]);
        this.setBrain(new AI());
    }

    // Have the AI decide what the enemy should do
    public override void think()
    {
        this.getBrain().think(this);
    }
// private
    // variables
    int enemyType;
};
