using System;
using System.Windows.Controls;
using System.Windows.Input;

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

        this.mainCanvas = newCanvas;
        this.setupPlayer();
	    this.setupDrawing(screenWidth, screenHeight);
	    //this.userCamera = new Camera(RectangleF(0, 0, (float)windowWidth, (float)windowHeight), RectangleF(0, 0, (float)windowWidth, (float)windowHeight));
	    //this.setupWorld(1);
        //this.setupCharacterStatusDisplay();
        //this.audioPlayer = new AudioPlayer(newCanvas);
        //this.audioPlayer.setSoundFile("09 Who Am I Living For_.m4a");
    }
    // general game control
    public bool isWorldRunning()
    {
        if (this.currentScreen == this.worldScreen)
            return true;
        else
            return false;
    }
    public void start()
    {
        //this.audioPlayer.play();
    }
    public void stop()
    {
        this.audioPlayer.stop();
    }
    public void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // tell the screen that a button was pressed, in case it's a menu that cares about it
        this.currentScreen.KeyDown(sender, e);
        // tell the world that a button was pressed
        //if (this.isWorldRunning())
        //    this.worldKeyDown(sender, e);
    }
    public void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // tell the current screen that a button was released, in case it's a menu that cares about it
        this.currentScreen.KeyUp(sender, e);
        // tell the world that a button was released
        //if (this.isWorldRunning())
        //    this.worldKeyUp(sender, e);
    }
    // this function handles any necessary action that the world must take due to a keypress
    /*
    public void worldKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // #define DESIGN_HERE
        if (e.Key == Key.W)
        {
            this.playerJump();
        }
        if (e.Key == Key.A)
        {
            this.movePlayerLeft();
        }
        if (e.Key == Key.D)
        {
            this.movePlayerRight();
        }
        if (e.Key == Key.OemSemicolon)
        {
            //this.game.selectWeapon1();
            this.playerPressTrigger(true);
        }
        if (e.Key == Key.L)
        {
            //this.game.cyclePlayerWeaponBackward();
            this.selectWeapon1();
        }
        if (e.Key == Key.P)
        {
            //System.Diagnostics.Trace.WriteLine("trigger pressed");
            //this.game.playerPressTrigger(true);
            this.selectWeapon2();
        }
        if (e.Key == Key.OemQuotes)
        {
            //this.game.cyclePlayerWeaponForward();
            this.selectWeapon3();
        }
        if (e.Key == Key.Space)
        {
            this.resetPlayerWeapon();
            //this.game.playerPressTrigger(true);
        }
    }
    public void worldKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // #define DESIGN_HERE
        if (e.Key == Key.A)
        {
            this.stopMovingPlayerLeft();
        }
        if (e.Key == Key.D)
        {
            this.stopMovingPlayerRight();
        }
        if (e.Key == Key.OemSemicolon)
        {
            //System.Diagnostics.Trace.WriteLine("trigger released");
            this.playerPressTrigger(false);
        }
    }
    */
    // image drawing
    // advance the game by one time unit
	public void timerTick(double numSeconds)
    {
        // update the screen
        Screen newScreen;
        if (this.isWorldRunning())
        {
            /*
            if (numSeconds > 0)
            {
                // advance the world
                this.worldLoader.timerTick(numSeconds);
                // scroll the screen
                this.worldLoader.scrollTo(this.player);
            }
            */
            newScreen = this.currentScreen.timerTick(numSeconds);
            // check whether they just came from the level selection screen
            if (newScreen != worldScreen)
            {
                // if we get here then they just left the world through a portal or whatever
                if (this.levelNumber == 5)
                {
                    // if they finished all the levels then show the victory screen
                    MenuScreen victoryScreen = new MenuScreen(this.mainCanvas, this.screenSize);
                    victoryScreen.setBackgroundBitmap(ImageLoader.loadImage("victory.png"));
                    newScreen = victoryScreen;
                    this.player.resetForLevel();
                }
                else
                {
                    // if there are still more levels then create the new level
                    this.player.resetForLevel();
                    this.player.addMoney(1600);
                    this.levelNumber++;
                    this.setupWorld(levelNumber);
                }
            }
        }
        else
        {
            newScreen = this.currentScreen.timerTick(numSeconds);
            if (newScreen != currentScreen)
            {
                if (currentScreen == levelSelectionScreen)
                {
                    this.levelNumber = levelSelectionScreen.getChosenLevelNumber();
                    if (levelNumber < 1)
                        levelNumber = 1;
                    if (levelNumber > 5)
                        levelNumber = 5;
                    this.setupWorld(this.levelNumber);
                    this.player.addMoney(this.levelNumber * 1600);
                }
            }
        }
        // transition to the next screen if necessary
        this.setCurrentScreen(newScreen);
    }
    // user commands
	// movement
    // controls for the player
    /*
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
    */
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

//private
	void setupDrawing(int screenWidth, int screenHeight)
    {
        // save screen size
        screenSize = new double[2];
        screenSize[0] = screenWidth;
        screenSize[1] = screenHeight;
        double[] screenPosition = new double[2];
        /*
        // determine the position for the world
        this.worldWindowPosition = new double[2];
        this.worldWindowPosition[0] = 0;
        this.worldWindowPosition[1] = 200;
        // determine the size for the world
        this.worldWindowSize = new double[2];
        this.worldWindowSize[0] = screenWidth - worldWindowPosition[0];
        this.worldWindowSize[1] = screenHeight - worldWindowPosition[1];
        // create a screen to hold the world and heads-up-display
        this.worldCanvas = new Canvas();
        */
        this.worldScreen = new WorldScreen(this.mainCanvas, screenPosition, screenSize);
        // level selection screen letting the user jump to another level
        levelSelectionScreen = new LevelSelectionScreen(this.mainCanvas, screenSize);
        // menu screen explaining how everything works
        MenuScreen menuScreen = new MenuScreen(this.mainCanvas, screenSize);
        levelSelectionScreen.setNextScreen(menuScreen);
        // screen for designing weapons
        WeaponDesignScreen designScreen = new WeaponDesignScreen(this.mainCanvas, screenSize);
        menuScreen.setNextScreen(designScreen);
        designScreen.setPlayer(this.player);
        designScreen.setNextScreen(this.worldScreen);
        menuScreen.setBackgroundBitmap(ImageLoader.loadImage("Instructions.png"));
        //MenuScreen victoryScreen = new MenuScreen(this.mainCanvas, screenSize);
        this.worldScreen.setExitScreen(menuScreen);
        //victoryScreen.setBackgroundBitmap(ImageLoader.loadImage("Victory.png"));
        this.setCurrentScreen(levelSelectionScreen);
    }
    void setupPlayer()
    {
        // #define DESIGN_HERE
        // spawn the player
        double[] location = new double[2]; location[0] = 30; location[1] = 30;
        this.player = new Player(location);
        //this.player.addMoney(1600);
#if false
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
#endif
        this.player.gotoWeaponTreeRoot();
    }
	void setupWorld(int levelNum)
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
        // create an object to keep track of the world
        //this.worldLoader = new WorldLoader(worldCanvas, worldWindowPosition, worldWindowSize);
        if (this.worldLoader != null)
        {
            this.worldLoader.destroy();
        }
        this.worldLoader = new WorldLoader(this.worldScreen.getWorldCanvas(), this.worldScreen.getWorldWindowSize(), levelNum);

        this.worldLoader.addItemAndDisableUnloading(this.player);
        this.worldScreen.followCharacter(this.player, this.worldLoader);
        //this.worldLoader.addItem(this.player);
    }
    void setCurrentScreen(Screen newScreen)
    {
        if (newScreen != currentScreen)
        {
            if (this.currentScreen != null)
                this.currentScreen.hide();
            this.currentScreen = newScreen;
            if (this.currentScreen != null)
                this.currentScreen.show();
        }
    }
    /*void setupCharacterStatusDisplay()
    {
        double[] statusLocation = new double[2];
        statusLocation[0] = 0;
        statusLocation[1] = 0;
        double[] statusSize = new double[2];
        statusSize[0] = this.screenSize[0];
        statusSize[1] = this.worldWindowPosition[1];
        statusDisplay = new CharacterStatusDisplay(this.worldScreen.getCanvas(), statusLocation, statusSize);
    }*/
    Canvas mainCanvas;
    /*
    double[] worldWindowPosition;
    double[] worldWindowSize;
    Canvas worldCanvas;
    CharacterStatusDisplay statusDisplay;
    */
    double[] screenSize;
    //Camera userCamera;
	WorldLoader worldLoader;
	Player player;
    AudioPlayer audioPlayer;
    WorldScreen worldScreen;
    Screen currentScreen;
    LevelSelectionScreen levelSelectionScreen;
    int levelNumber;
};