using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

class WorldScreen : Screen
{
// public
    public WorldScreen(Size size, LevelPlayer player, WorldLoader world) : base(size)
    {
        base.Initialize(new Point(), size);
        this.player = player;
        this.levelIsComplete = false;
        this.world = world;
        this.camera = new Camera(new WorldBox(size), new WorldBox(size));
        this.createSubviews(new Point(), size);
        this.world.RegisterForUpdates(this);
    }
    public void Show()
    {
        this.stopMovingPlayerLeft();
        this.stopMovingPlayerRight();
    }

    // Called by the World class to make objects visible
    public void ShowObject(GameObject o)
    {
        Image image = o.getImage();
        TransformGroup transforms = new TransformGroup();
        transforms.Children.Add(o.getRenderTransform()); // adjustment of the object compared to the screen
        transforms.Children.Add(this.camera.Transform); // adjustment of the camera compared to the world
        image.RenderTransform = transforms;
        // now add the image to the screen
        this.worldCanvas.Children.Add(image);
    }

    // Called by the World class to make objects go away
    public void HideObject(GameObject o)
    {
        this.worldCanvas.Children.Remove(o.getImage());
    }

// private
    void createSubviews(Point position, Size size)
    {
        // get the canvas on which to put stuff
        Canvas screenCanvas = this.getCanvas();
        

        // determine coordinates for the status display
        Point statusDisplayPosition = new Point();
        Size statusDisplaySize = new Size();
        statusDisplaySize.Width = size.Width;
        double statusHeight = 200;
        statusDisplaySize.Height = statusHeight;
        // create status display
        this.statusDisplay = new CharacterStatusDisplay(this.player, screenCanvas, statusDisplayPosition, statusDisplaySize);
        
        // window into the world
        Point worldWindowPosition = new Point(0, statusDisplayPosition.X + statusDisplaySize.Height);
        worldWindowSize = new Size(size.Width - worldWindowPosition.X, size.Height - worldWindowPosition.Y);
        // create a canvas to draw on. The game will later ask for it and give it to the WorldLoader
        this.worldCanvas = this.makeCanvas(worldWindowPosition, worldWindowSize);
        screenCanvas.Children.Add(this.worldCanvas);
        Canvas.SetTop(this.worldCanvas, statusHeight);

        this.camera = new Camera(new WorldBox(0, worldWindowSize.Width, 0, worldWindowSize.Height),
            new WorldBox(0, worldWindowSize.Width, statusHeight, worldWindowSize.Height));

    }
    public Boolean isPlayerSuccessful()
    {
        return this.levelIsComplete;
    }
    public void TimerTick(double numSeconds)
    {
        if (numSeconds > 0)
        {
            this.camera.scrollTo(this.player);
            this.world.RecenterRealityBubble(this.camera.getWorldBox());
            this.world.timerTick(numSeconds);
        }
        this.statusDisplay.update();
        if (this.world.characterTouchingPortal(this.player))
            this.levelIsComplete = true;
    }
    // player controls
    public void movePlayerUp()
    {
        // jump if possible
        this.player.jump();
        // float upward if possible
        this.player.setTargetVY(10000);
    }
    public void stopMovingPlayerUp()
    {
        double? v = this.player.getTargetVY();
        if ((v != null) && (v > 0))
        {
            this.player.setTargetVY(null);
        }
    }
    public void movePlayerDown()
    {
        this.player.setTargetVY(-2000);
    }
    public void stopMovingPlayerDown()
    {
        double? v = this.player.getTargetVY();
        if ((v != null) && (v < 0))
            this.player.setTargetVY(null);
    }
    public void movePlayerLeft()
    {
        this.player.setTargetVX(-2000);
    }
    public void stopMovingPlayerLeft()
    {
        double? v = this.player.getTargetVX();
        if ((v != null) && (v < 0))
            this.player.setTargetVX(0);
    }
    public void movePlayerRight()
    {
        this.player.setTargetVX(2000);
    }
    public void stopMovingPlayerRight()
    {
        double? v = this.player.getTargetVX();
        if ((v != null) && (v > 0))
            this.player.setTargetVX(0);
    }
    public override void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // movement
        if (e.Key == Key.W)
            this.movePlayerUp();
        if (e.Key == Key.S)
            this.movePlayerDown();
        if (e.Key == Key.A)
            this.movePlayerLeft();
        if (e.Key == Key.D)
            this.movePlayerRight();
        // firing
        if (e.Key == Key.L)
            this.playerPressTrigger(0, true);
        if (e.Key == Key.OemSemicolon)
            this.playerPressTrigger(1, true);
        if (e.Key == Key.OemQuotes)
            this.playerPressTrigger(2, true);
        // timing
        if ((e.Key == Key.Escape) && (this.isEscapeEnabled()))
            this.levelIsComplete = true;
        if (e.Key == Key.Space)
            this.togglePause();

        base.KeyDown(sender, e);
    }
    public override void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.W)
            this.stopMovingPlayerUp();
        if (e.Key == Key.W)
            this.stopMovingPlayerDown();
        if (e.Key == Key.A)
            this.stopMovingPlayerLeft();
        if (e.Key == Key.D)
            this.stopMovingPlayerRight();
        if (e.Key == Key.L)
            this.playerPressTrigger(0, false);
        if (e.Key == Key.OemSemicolon)
            this.playerPressTrigger(1, false);
        if (e.Key == Key.OemQuotes)
            this.playerPressTrigger(2, false);
        base.KeyUp(sender, e);
    }
    public void pause()
    {
        this.world.pause();
    }
    public void unPause()
    {
        this.world.unPause();
    }
    public void togglePause()
    {
        this.world.togglePause();
    }

    public void setEscapeEnabled(bool enabled)
    {
        this.escapeEnabled = enabled;
    }
    public bool isEscapeEnabled()
    {
        return this.escapeEnabled;
    }
    public void playerPressTrigger(int weaponIndex, bool pressed)
    {
        this.player.pressTrigger(weaponIndex, pressed);
    }


    public Size getWorldWindowSize()
    {
        return this.worldWindowSize;
    }
    Size worldWindowSize;
    CharacterStatusDisplay statusDisplay;
    // the canvas to draw the world on
    Canvas worldCanvas;
    // the character that the user controls
    LevelPlayer player;
    // the world that the player is in
    WorldLoader world;
    // the screen that will be shown when the level exits
    Screen exitScreen;
    // whether the user has satisfied the criteria to exit the level
    bool levelIsComplete;
    // whether the pushing the escape button should exit the level
    bool escapeEnabled;
    Camera camera;
}