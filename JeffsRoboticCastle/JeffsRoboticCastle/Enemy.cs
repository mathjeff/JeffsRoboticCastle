using System.Windows.Media.Imaging;
class Enemy : Character
{
public
    // constructor for an enemy
	Enemy(double[] location, int type)
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
                this.setShape(new GameRectangle(88, 103));
                accel = new double[2]; accel[0] = 300; accel[1] = 0;
	            this.setMaxAccel(accel);
	            this.setDragCoefficient(1);
	            this.enemyType = type;
                this.setGravity(1000);
                break;
            case 1:
                startingImage = ImageLoader.loadImage("eagle.png");
                this.setCenter(location);
                this.setShape(new GameRectangle(56, 43));
                accel = new double[2]; accel[0] = 400; accel[1] = 500;
                this.setMaxAccel(accel);
                this.setDragCoefficient(3);
                this.enemyType = type;
                this.setGravity(75);
                break;
            case 2:
                startingImage = ImageLoader.loadImage("swarm.png");
                this.setCenter(location);
                this.setShape(new GameRectangle(16, 9));
                accel = new double[2]; accel[0] = 250; accel[1] = 0;
                this.setMaxAccel(accel);
                this.setDragCoefficient(3);
                this.enemyType = type;
                this.setGravity(1000);
                this.initializeHitpoints(5);
                break;
            case 3:
                startingImage = ImageLoader.loadImage("armadillo.png");
                this.setCenter(location);
                this.setShape(new GameRectangle(105, 53));
                accel = new double[2]; accel[0] = 250; accel[1] = 0;
                this.setMaxAccel(accel);
                this.setDragCoefficient(5);
                this.enemyType = type;
                this.setGravity(1000);
                this.initializeHitpoints(30);
                break;
            case 4:
                startingImage = ImageLoader.loadImage("goblin2.png");
                this.setCenter(location);
                this.setShape(new GameRectangle(18, 51));
                accel = new double[2]; accel[0] = 450; accel[1] = 0;
                this.setMaxAccel(accel);
                this.setDragCoefficient(3);
                this.enemyType = type;
                this.setGravity(1000);
                this.initializeHitpoints(20);
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
