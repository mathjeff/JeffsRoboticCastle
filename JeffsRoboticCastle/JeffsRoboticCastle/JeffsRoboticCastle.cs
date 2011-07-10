using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

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
        this.allEnemyWeaponChoices = new List<Weapon>();
        WeaponFactory factory = new WeaponFactory();
        factory.addDefaultWeapons();
        int i;
        for (i = 0; i < factory.getNumWeapons(); i++)
        {
            this.allEnemyWeaponChoices.Add(factory.makeWeapon(i));
        }
        this.currentEnemyWeaponChoices = new List<Weapon>();
        this.setupPlayer();
	    this.setupDrawing(screenWidth, screenHeight);
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
    // image drawing
    // advance the game by one time unit
	public void timerTick(double numSeconds)
    {
        // update the screen
        Screen newScreen;
        if (this.isWorldRunning())
        {
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
                    this.player.addMoney(800);
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
                    if (levelNumber > 9)
                        levelNumber = 9;
                    this.setupWorld(this.levelNumber);
                    this.player.addMoney((this.levelNumber + 1) * 800);
                }
            }
        }
        // transition to the next screen if necessary
        this.setCurrentScreen(newScreen);
    }
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
        this.player.gotoWeaponTreeRoot();
    }
	void setupWorld(int levelNum)
    {
        // create an object to keep track of the world
        if (this.worldLoader != null)
        {
            this.worldLoader.destroy();
        }
        // create a list of all of the weapons that enemies haven't been allowed to use yet
        List<Weapon> newWeaponChoices = new List<Weapon>();
        foreach (Weapon currentWeapon in this.allEnemyWeaponChoices)
        {
            if (!currentEnemyWeaponChoices.Contains(currentWeapon))
                newWeaponChoices.Add(currentWeapon);
        }
        // add a few new weapons for the enemies to use
        Random generator = new Random();
        while ((currentEnemyWeaponChoices.Count < levelNum) && (newWeaponChoices.Count > 0))
        {
            int index = generator.Next(newWeaponChoices.Count);
            currentEnemyWeaponChoices.Add(newWeaponChoices[index]);
            newWeaponChoices.RemoveAt(index);
        }
        this.worldLoader = new WorldLoader(this.worldScreen.getWorldCanvas(), this.worldScreen.getWorldWindowSize(), levelNum, currentEnemyWeaponChoices);

        this.worldLoader.addItemAndDisableUnloading(this.player);
        this.worldScreen.followCharacter(this.player, this.worldLoader);
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
    Canvas mainCanvas;
    double[] screenSize;
    //Camera userCamera;
	WorldLoader worldLoader;
    List<Weapon> currentEnemyWeaponChoices;
    List<Weapon> allEnemyWeaponChoices;
	Player player;
    AudioPlayer audioPlayer;
    WorldScreen worldScreen;
    Screen currentScreen;
    LevelSelectionScreen levelSelectionScreen;
    int levelNumber;
};