using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using Castle.EventNodes;
using Castle.EventNodes.Menus;
using Castle.EventNodes.World;
using Castle.EventNodes.Customization;
using Castle.Menus;
using System.Windows;
using Castle.WeaponDesign;
using Castle;

class JeffsRoboticCastle
{
// public
	// constructor
	public JeffsRoboticCastle(Canvas newCanvas, Size screenSize)
    {
        this.screenSize = new Size(screenSize.Width, screenSize.Height);
        this.mainCanvas = newCanvas;
        this.setupWeapons();
        this.setupEnemies();
        this.setupPlayer();
        this.setupStory();
    }
    public void setupStory()
    {
        InfoEventNode welcome = InfoEventNode.FromImage("Welcome.png");
        this.showEvent(welcome);

        HelpEventNode help = new HelpEventNode();
        welcome.NextNode = help;



        PatronSelectionEventNode patronNode = new PatronSelectionEventNode(this.player, this.weaponAugmentFactory);
        help.NextNode = patronNode;

        RewardEventNode resourceNode = new RewardEventNode(1, this.player);
        patronNode.NextNode = resourceNode;


        ShopEventNode designer = new ShopEventNode(this.player);
        resourceNode.NextNode = designer;

        ShopEventNode currentNode = designer;
        for (int difficulty = 1; difficulty <= 10; difficulty++)
        {
            WorldEventNode world = this.createWorld(difficulty);
            currentNode.NextNode = world;

            RewardEventNode reward = new RewardEventNode(1, this.player);
            world.SuccessNode = reward;

            ShopEventNode shop = new ShopEventNode(this.player);
            reward.NextNode = shop;

            currentNode = shop;
        }

        InfoEventNode success = InfoEventNode.FromImage("victory.png");
        currentNode.NextNode = success;
    }
    private void advanceEnemyWeapons()
    {
        List<WeaponAugmentTemplate> augments = new List<WeaponAugmentTemplate>(this.weaponAugmentFactory.All);
        // Add a new type of weapon
        this.enemyWeapons.Add(new WeaponConfiguration(this.weaponAugmentFactory.BasicWeapon, new List<WeaponAugment>()));

        // Add a random augment to each weapon type
        foreach (WeaponConfiguration configuration in this.enemyWeapons)
        {
            int index = this.randomGenerator.Next(augments.Count);
            WeaponAugmentTemplate template = augments[index];
            configuration.Augments.Add(new WeaponAugment(template));
        }
    }
    private List<WeaponStats> getNewEnemyWeaponStats()
    {
        this.advanceEnemyWeapons();
        List<WeaponStats> stats = new List<WeaponStats>();
        foreach (WeaponConfiguration configuration in this.enemyWeapons)
        {
            stats.Add(configuration.GetStats());
        }
        return stats;
    }
    public void setupEnemies()
    {
        // Choose the items that the enemies get, and compute the stats of the resultant weapon
        /*List<WeaponAugmentTemplate> templates = new List<WeaponAugmentTemplate>(){
            this.weaponAugmentFactory.Damager, this.weaponAugmentFactory.Flier};
        WeaponStats weaponStats = this.weaponAugmentFactory.BasicWeapon.WithAugments(templates);
        */
        //this.allEnemyWeaponChoices = new List<WeaponStats>(){weaponStats};
        this.enemyWeapons = new List<WeaponConfiguration>();


        // reorder the list of WeaponStats randomly
        /*int i;
        Random generator = new Random();
        for (i = 0; i < this.allEnemyWeaponChoices.Count; i++)
        {
            int otherIndex = generator.Next(this.allEnemyWeaponChoices.Count - 1) + i;
            WeaponStats temp = this.allEnemyWeaponChoices[i];
            this.allEnemyWeaponChoices[i] = this.allEnemyWeaponChoices[otherIndex];
            this.allEnemyWeaponChoices[otherIndex] = temp;
        }*/
    }
    private WorldEventNode createWorld(int difficulty)
    {
        //List<WeaponStats> enemyWeapons = this.allEnemyWeaponChoices.GetRange(0, difficulty / 2 + 1);
        List<WeaponStats> enemyWeapons = this.getNewEnemyWeaponStats();
        return new WorldEventNode(this.player, difficulty, this.randomGenerator);
    }
    private void setupWeapons()
    {
        this.weaponAugmentFactory = new WeaponAugmentFactory();
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
    }
    private void setScreen(Screen screen)
    {
        Canvas newCanvas = screen.getCanvas();
        if (this.currentScreen != null)
            this.mainCanvas.Children.Remove(this.currentScreen.getCanvas());
        this.currentScreen = screen;
        this.mainCanvas.Children.Add(newCanvas);
    }

    // image drawing
    // advance the game by one time unit
	public void timerTick(double numSeconds)
    {
        EventNode currentEvent = this.currentEvent;
        EventNode newEvent = this.currentEvent.TimerTick(numSeconds);
        if (newEvent == null)
        {
            Application.Current.Shutdown();
            return;
        }
        if (newEvent != currentEvent)
            this.showEvent(newEvent);
        Screen newScreen = newEvent.GetScreen();
        if (newScreen != this.currentScreen)
            this.setScreen(newScreen);

    }
    

//private
    void setupPlayer()
    {
        // spawn the player
        this.player = new GamePlayer(140);
        BasicWeapon weapon;
        weapon = this.weaponAugmentFactory.BasicWeapon;
        player.WeaponConfigurations.Add(new WeaponConfiguration(weapon, new List<WeaponAugment>()));
    }

    //List<WeaponStats> allEnemyWeaponChoices;
    List<WeaponConfiguration> enemyWeapons;
    Canvas mainCanvas;
    Size screenSize;
    GamePlayer player;
    AudioPlayer audioPlayer;
    EventNode currentEvent;
    Screen currentScreen;
    WeaponAugmentFactory weaponAugmentFactory;
    Random randomGenerator = new Random();

    //LevelSelectionScreen levelSelectionScreen;
    int levelNumber;
};