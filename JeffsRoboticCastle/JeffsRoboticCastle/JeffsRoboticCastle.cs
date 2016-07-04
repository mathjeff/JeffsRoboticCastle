using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using Castle.EventNodes;
using Castle.EventNodes.Menus;
using Castle.EventNodes.World;
using Castle.EventNodes.Editing;
using Castle.Menus;
using System.Windows;

class JeffsRoboticCastle
{
// public
	// constructor
	public JeffsRoboticCastle(Canvas newCanvas, Size screenSize)
    {
        this.screenSize = new Size(screenSize.Width, screenSize.Height);
        this.mainCanvas = newCanvas;
        this.setupEnemies();
        this.setupPlayer();
        this.setupStory();
    }
    public void setupStory()
    {
        InfoEventNode welcome = InfoEventNode.FromImage("Instructions.png");
        this.showEvent(welcome);
        InfoEventNode help = InfoEventNode.FromText("here is some text");
        welcome.NextNode = help;
        WorldEventNode world = this.createWorld(0);
        help.NextNode = world;
        RewardEventNode reward = new RewardEventNode(800, this.player);
        world.SuccessNode = reward;
        WeaponDesignEventNode designer = new WeaponDesignEventNode(this.player);
        reward.NextNode = designer;
        world.SuccessNode = designer;
        WorldEventNode world2 = this.createWorld(1);
        designer.NextNode = world2;
        InfoEventNode success = InfoEventNode.FromImage("victory.png");
        world2.SuccessNode = success;
    }
    public void setupEnemies()
    {
        this.allEnemyWeaponChoices = new List<Weapon>();
        WeaponFactory factory = new WeaponFactory();
        factory.addDefaultWeapons();
        int i;
        for (i = 0; i < factory.getNumWeapons(); i++)
        {
            this.allEnemyWeaponChoices.Add(factory.makeWeapon(i));
        }
        // reorder the weapon list randomly
        Random generator = new Random();
        for (i = 0; i < this.allEnemyWeaponChoices.Count; i++)
        {
            int otherIndex = generator.Next(this.allEnemyWeaponChoices.Count);
            Weapon temp = this.allEnemyWeaponChoices[i];
            this.allEnemyWeaponChoices[i] = this.allEnemyWeaponChoices[otherIndex];
            this.allEnemyWeaponChoices[otherIndex] = temp;
        }
    }
    private WorldEventNode createWorld(int difficulty)
    {
        List<Weapon> enemyWeapons = this.allEnemyWeaponChoices.GetRange(0, difficulty / 2 + 1);
        return new WorldEventNode(this.player, enemyWeapons);
    }

    public void setupMusic()
    {
        //MediaElement mediaPlayer = new MediaElement();
        //mediaPlayer.Source = new System.Uri("09WhoAmILivingFor_.m4a", UriKind.Relative);
        //mediaPlayer.LoadedBehavior = MediaState.Play;
        //newCanvas.Children.Add(mediaPlayer);
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
        this.currentScreen.KeyDown(sender, e);
    }
    public void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        this.currentScreen.KeyUp(sender, e);
    }
    private void showEvent(EventNode newEvent)
    {
        this.currentEvent = newEvent;
        if (newEvent == null)
        {
            Application.Current.Shutdown();
            return;
        }
        newEvent.Show(this.screenSize);
        Screen newScreen = newEvent.GetScreen();
        if (newScreen != this.currentScreen)
            this.setScreen(newScreen);
    }
    private void setScreen(Screen screen)
    {
        Canvas newCanvas = screen.getCanvas();
        if (this.currentScreen != null)
            this.mainCanvas.Children.Remove(this.currentScreen.getCanvas());
        this.currentScreen = screen;
        this.mainCanvas.Children.Add(newCanvas);
        //UIElement child = newCanvas.Children[0];
        //newCanvas.Children.Remove(child);
        //this.mainCanvas.Children.Add(child);
        //this.mainCanvas.Children.Add(ImageLoader.loadImage("Instructions.png", new Size(300, 300)));
    }

    // image drawing
    // advance the game by one time unit
	public void timerTick(double numSeconds)
    {
        EventNode currentEvent = this.currentEvent;
        EventNode newEvent = this.currentEvent.TimerTick(numSeconds);
        if (newEvent != currentEvent)
            this.showEvent(newEvent);

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
    void setupPlayer()
    {
        // spawn the player
        double[] location = new double[2]; location[0] = 30; location[1] = 30;
        this.player = new Player(location);
        this.player.addMoney(1600);
        this.player.gotoWeaponTreeRoot();
    }

    List<Weapon> allEnemyWeaponChoices;
    Canvas mainCanvas;
    Size screenSize;
    Player player;
    AudioPlayer audioPlayer;
    EventNode currentEvent;
    Screen currentScreen;

    //LevelSelectionScreen levelSelectionScreen;
    int levelNumber;
};