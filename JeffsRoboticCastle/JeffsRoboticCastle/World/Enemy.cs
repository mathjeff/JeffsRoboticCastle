using System.Windows;
using System.Windows.Media.Imaging;
class Enemy : Character
{
    public Enemy()
    {
    }
    public Enemy(BitmapImage image, Point center, GameShape shape, double[] accel, double hitpoints, double jumpSpeed, int intelligence)
    {
        this.setBitmap(image);
        this.setCenter(new double[]{center.X, center.Y});
        this.setDragCoefficient(4);
        this.setShape(shape);
        this.setMaxAccel(accel);
        this.initializeHitpoints(hitpoints);
        this.setJumpSpeed(jumpSpeed);
        this.setGravity(1000);
        this.setContactDamagePerSecond(0);
        this.setImageOffset(new double[2]);
        this.setBrain(new AI(intelligence));
        this.setTeamNum(1);
    }
    // constructor for an enemy
	static Enemy NewEnemy(double[] location, int type)
    {
        BitmapImage startingImage = null;
        double[] accel;
        Enemy enemy = new Enemy();
        enemy.setTeamNum(1);
        // #define DESIGN_HERE
        switch (type)
        {
            case 0:
                startingImage = ImageLoader.loadImage("archer.png");
                enemy.setCenter(location);
                enemy.setShape(new GameRectangle(26, 41));
                accel = new double[2]; accel[0] = 510; accel[1] = 0;
                enemy.setMaxAccel(accel);
                enemy.setDragCoefficient(3);
                enemy.enemyType = type;
                enemy.setGravity(1000);
                enemy.initializeHitpoints(15);
                enemy.setJumpSpeed(950);
                enemy.setContactDamagePerSecond(0);
                break;
            case 1:
                startingImage = ImageLoader.loadImage("eagle.png");
                enemy.setCenter(location);
                enemy.setShape(new GameRectangle(56, 43));
                accel = new double[2]; accel[0] = 480; accel[1] = 500;
                enemy.setMaxAccel(accel);
                enemy.setDragCoefficient(3);
                enemy.enemyType = type;
                enemy.setGravity(75);
                enemy.initializeHitpoints(9);
                enemy.setJumpSpeed(500);
                enemy.setContactDamagePerSecond(0);
                break;
            case 2:
                startingImage = ImageLoader.loadImage("horseman.png");
                enemy.setCenter(location);
                enemy.setShape(new GameRectangle(80, 93));
                accel = new double[2]; accel[0] = 360; accel[1] = 0;
                enemy.setMaxAccel(accel);
                enemy.setDragCoefficient(1);
                enemy.enemyType = type;
                enemy.setGravity(1000);
                enemy.initializeHitpoints(11);
                enemy.setJumpSpeed(800);
                enemy.setContactDamagePerSecond(0);
                break;
            case 3:
                startingImage = ImageLoader.loadImage("swarm.png");
                enemy.setCenter(location);
                enemy.setShape(new GameRectangle(16, 9));
                accel = new double[2]; accel[0] = 350; accel[1] = 0;
                enemy.setMaxAccel(accel);
                enemy.setDragCoefficient(3);
                enemy.enemyType = type;
                enemy.setGravity(1000);
                enemy.initializeHitpoints(5);
                enemy.setJumpSpeed(500);
                enemy.setContactDamagePerSecond(0);
                break;
            case 4:
                startingImage = ImageLoader.loadImage("armadillo.png");
                enemy.setCenter(location);
                enemy.setShape(new GameRectangle(157, 79));
                accel = new double[2]; accel[0] = 280; accel[1] = 0;
                enemy.setMaxAccel(accel);
                enemy.setDragCoefficient(4);
                enemy.enemyType = type;
                enemy.setGravity(1000);
                enemy.initializeHitpoints(30);
                enemy.setJumpSpeed(550);
                enemy.setArmor(1);
                enemy.setContactDamagePerSecond(0);
                break;
            case 5:
                startingImage = ImageLoader.loadImage("chariot.png");
                enemy.setCenter(location);
                enemy.setShape(new GameRectangle(102, 61));
                accel = new double[2]; accel[0] = 220; accel[1] = 0;
                enemy.setMaxAccel(accel);
                enemy.setDragCoefficient(.5);
                enemy.enemyType = type;
                enemy.setGravity(1000);
                enemy.initializeHitpoints(11);
                enemy.setJumpSpeed(500);
                enemy.setContactDamagePerSecond(0);
                break;
            case 6:
                startingImage = ImageLoader.loadImage("goblin2.png");
                enemy.setCenter(location);
                enemy.setShape(new GameRectangle(18, 51));
                accel = new double[2]; accel[0] = 540; accel[1] = 0;
                enemy.setMaxAccel(accel);
                enemy.setDragCoefficient(3);
                enemy.enemyType = type;
                enemy.setGravity(1000);
                enemy.initializeHitpoints(15);
                enemy.setJumpSpeed(950);
                enemy.setContactDamagePerSecond(2);
                break;
            default:
                break;
        }
        enemy.setBitmap(startingImage);
        enemy.setImageOffset(new double[2]);
        enemy.setBrain(new AI(1));
        return enemy;
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
