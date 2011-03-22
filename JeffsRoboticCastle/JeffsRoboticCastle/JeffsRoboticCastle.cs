using System;
using System.Windows.Controls;

class JeffsRoboticCastle
{
// public
	// constructor
	public JeffsRoboticCastle(Canvas newCanvas, int screenWidth, int screenHeight)
    {
        //MediaElement mediaPlayer = new MediaElement();
        //mediaPlayer.Source = new System.Uri("09WhoAmILivingFor_.m4a", UriKind.Relative);
        //mediaPlayer.LoadedBehavior = MediaState.Play;
        //newCanvas.Children.Add(mediaPlayer);

        this.canvas = newCanvas;
	    this.setupDrawing(screenWidth, screenHeight);
	    //this.userCamera = new Camera(RectangleF(0, 0, (float)windowWidth, (float)windowHeight), RectangleF(0, 0, (float)windowWidth, (float)windowHeight));
	    this.setupWorld();
        this.setupCharacterStatusDisplay();
        //this.audioPlayer = new AudioPlayer(newCanvas);
        //this.audioPlayer.setSoundFile("09 Who Am I Living For_.m4a");
    }
    public void start()
    {
        //this.audioPlayer.play();
    }
    public void stop()
    {
        this.audioPlayer.stop();
    }
    // image drawing
    // advance the game by one time unit
	public void timerTick(double numSeconds)
    {
        if (numSeconds > 0)
        {
            this.worldLoader.timerTick(numSeconds);
            this.worldLoader.scrollTo(this.player);
            this.statusDisplay.update();
        }
    }
    // user commands
	// movement
    // controls for the player
    public void playerJump()
    {
        this.player.jump();
    }
    public void movePlayerLeft()
    {
        double[] newV = new double[2]; newV[0] = -10000; newV[1] = 0;
        this.player.setTargetVelocity(newV);
    }

    public void movePlayerRight()
    {
        double[] newV = new double[2]; newV[0] = 10000; newV[1] = 0;
        this.player.setTargetVelocity(newV);
    }

    public void stopMovingPlayerLeft()
    {
	    if (this.player.getTargetVelocity()[0] < 0)
		    this.player.setTargetVelocity(new double[2]);
    }
    public void stopMovingPlayerRight()
    {
	    if (this.player.getTargetVelocity()[0] > 0)
		    this.player.setTargetVelocity(new double[2]);
    }
    // weapons
    public void selectWeapon1()
    {
        this.player.selectWeaponSubtreeAtIndex(0);
    }
    public void selectWeapon2()
    {
        this.player.selectWeaponSubtreeAtIndex(1);
    }
    public void selectWeapon3()
    {
        this.player.selectWeaponSubtreeAtIndex(2);
    }
    public void selectWeapon4()
    {
        this.player.selectWeaponSubtreeAtIndex(3);
    }
/*    public void cyclePlayerWeaponForward()
    {
	    //this.player.cycleWeaponForward();
        this.player.selectRightWeaponTree();
    }
    public void cyclePlayerWeaponBackward()
    {
	    //this.player.cycleWeaponBackward();
        this.player.selectLeftWeaponTree();
    }
*/    
    public void resetPlayerWeapon()
    {
        this.player.gotoWeaponTreeRoot();
    }
    public void playerPressTrigger(bool pressed)
    {
	    this.player.pressTrigger(pressed);
    }

private
	void setupDrawing(int screenWidth, int screenHeight)
    {
        // save screen size
        this.screenSize = new double[2];
        screenSize[0] = screenWidth;
        screenSize[1] = screenHeight;
        // determine the position for the world
        this.worldWindowPosition = new double[2];
        this.worldWindowPosition[0] = 0;
        this.worldWindowPosition[1] = 200;
        // determine the size for the world
        this.worldWindowSize = new double[2];
        this.worldWindowSize[0] = screenWidth - worldWindowPosition[0];
        this.worldWindowSize[1] = screenHeight - worldWindowPosition[1];
    }
	void setupWorld()
    {
        //System.Windows.Shapes.Rectangle worldRect = new System.Windows.Shapes.Rectangle();
        /*Canvas worldCanvas = new Canvas();
        worldCanvas.Width = worldWindowSize[0];
        worldCanvas.Height = worldWindowSize[1];
        worldCanvas.ClipToBounds = true;
        worldCanvas.RenderTransform = new System.Windows.Media.TranslateTransform(worldWindowPosition[0], worldWindowPosition[1]);
        this.canvas.Children.Add(worldCanvas);
        */
        //worldCanvas.
        //worldCanvas.
        // crealte an object to keep track of the world
        this.worldLoader = new WorldLoader(canvas, worldWindowPosition, worldWindowSize);

        // #define DESIGN_HERE
        // spawn the player
        double[] location = new double[2]; location[0] = 30; location[1] = 30;
        this.player = new Player(location);
        this.player.addWeapon(new Weapon(0));
        this.player.addWeapon(new Weapon(1));
        this.player.addWeapon(new Weapon(2));
        this.player.addWeapon(new Weapon(3));
        this.player.addWeapon(new Weapon(4));
        this.player.addWeapon(new Weapon(5));
        this.player.addWeapon(new Weapon(6));
        this.player.addWeapon(new Weapon(7));
        this.player.addWeapon(new Weapon(8));
        this.player.addWeapon(new Weapon(9));
        this.player.addWeapon(new Weapon(10));
        this.player.addWeapon(new Weapon(11));
        this.player.addWeapon(new Weapon(12));
        this.player.addWeapon(new Weapon(13));
        this.player.addWeapon(new Weapon(14));
        this.player.addWeapon(new Weapon(15));
        this.player.gotoWeaponTreeRoot();
        this.worldLoader.addItemAndDisableUnloading(this.player);
        //this.worldLoader.addItem(this.player);
    }
    void setupCharacterStatusDisplay()
    {
        double[] statusLocation = new double[2];
        statusLocation[0] = 0;
        statusLocation[1] = 0;
        double[] statusSize = new double[2];
        statusSize[0] = this.screenSize[0];
        statusSize[1] = this.worldWindowPosition[1];
        statusDisplay = new CharacterStatusDisplay(this.canvas, statusLocation, statusSize, this.player);
    }
    double[] worldWindowPosition;
    double[] worldWindowSize;
    double[] screenSize;
	//Camera userCamera;
	WorldLoader worldLoader;
	Player player;
    Canvas canvas;
    AudioPlayer audioPlayer;
    CharacterStatusDisplay statusDisplay;
};