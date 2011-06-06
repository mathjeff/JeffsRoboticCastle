using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

class WorldScreen : Screen
{
// public
    public WorldScreen(Canvas c, double[] screenPosition, double[] screenSize)
    {
        this.initialize(c, screenPosition, screenSize);
    }
    public override void initialize(Canvas c, double[] screenPosition, double[] screenSize)
    {
        base.initialize(c, screenPosition, screenSize);
        this.createSubviews(screenPosition, screenSize);
    }
    public Canvas getWorldCanvas()
    {
        return this.worldCanvas;
    }
    public void followCharacter(Character newPlayer, WorldLoader newWorld)
    {
        this.player = newPlayer;
        this.statusDisplay.followCharacter(newPlayer);
        this.world = newWorld;
        this.levelIsOver = false;
        // scroll before showing the screen so that the screen doesn't suddenly move after the first frame
        this.world.scrollTo(this.player);
    }
    public override void show()
    {
        this.stopMovingPlayerLeft();
        this.stopMovingPlayerRight();
        base.show();
    }
// private
    void createSubviews(double[] position, double[] size)
    {
        // get the canvas on which to put stuff
        Canvas worldCanvas = this.getCanvas();

        // determine coordinates for the status display
        double[] statusDisplayPosition = new double[2];
        double[] statusDisplaySize = new double[2];
        statusDisplaySize[0] = size[0];
        statusDisplaySize[1] = 200;
        // create status display
        this.statusDisplay = new CharacterStatusDisplay(worldCanvas, statusDisplayPosition, statusDisplaySize);
        
        // window into the world
        double[] worldWindowPosition = new double[2];
        worldWindowPosition[0] = 0;
        worldWindowPosition[1] = statusDisplayPosition[1] + statusDisplaySize[1];
        worldWindowSize = new double[2];
        worldWindowSize[0] = size[0] - worldWindowPosition[0];
        worldWindowSize[1] = size[1] - worldWindowPosition[1];
        // create a canvas to draw on. The game will later ask for it and give it to the WorldLoader
        this.worldCanvas = this.makeCanvas(worldWindowPosition, worldWindowSize);
        this.getCanvas().Children.Add(this.worldCanvas);
    }
    public override Screen timerTick(double numSeconds)
    {
        if (numSeconds > 0)
            this.world.timerTick(numSeconds);
        this.world.scrollTo(this.player);
        this.statusDisplay.update();
        base.timerTick(numSeconds);
        if (this.world.characterTouchingPortal(this.player))
            this.levelIsOver = true;
        if (this.levelIsOver)
            return this.exitScreen;
        else
            return this;
    }
    public void setExitScreen(Screen newScreen)
    {
        this.exitScreen = newScreen;
    }
    // player controls
    public void movePlayerUp()
    {
        // jump if possible
        this.player.jump();
        // float upward if possible
        double[] newV = this.player.getTargetVelocity();
        newV[1] = 20000;
        this.player.setTargetVelocity(newV);
    }
    public void stopMovingPlayerUp()
    {
        double[] v = this.player.getTargetVelocity();
        if (v[1] > 0)
        {
            v[1] = 0;
            this.player.setTargetVelocity(v);
        }
    }
    public void movePlayerDown()
    {
        // float upward if possible
        double[] newV = this.player.getTargetVelocity();
        newV[1] = -2000;
        this.player.setTargetVelocity(newV);
    }
    public void stopMovingPlayerDown()
    {
        double[] v = this.player.getTargetVelocity();
        if (v[1] < 0)
        {
            v[1] = 0;
            this.player.setTargetVelocity(v);
        }
    }
    public void movePlayerLeft()
    {
        double[] newV = this.player.getTargetVelocity();
        newV[0] = -10000;
        this.player.setTargetVelocity(newV);
    }
    public void stopMovingPlayerLeft()
    {
        double[] v = this.player.getTargetVelocity();
        if (v[0] < 0)
        {
            v[0] = 0;
            this.player.setTargetVelocity(v);
        }
    }
    public void movePlayerRight()
    {
        double[] newV = this.player.getTargetVelocity();
        newV[0] = 10000;
        this.player.setTargetVelocity(newV);
    }
    public void stopMovingPlayerRight()
    {
        double[] v = this.player.getTargetVelocity();
        if (v[0] > 0)
        {
            v[0] = 0;
            this.player.setTargetVelocity(v);
        }
    }
    public override void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // #define DESIGN_HERE
        if (e.Key == Key.W)
        {
            //this.playerJump();
            this.movePlayerUp();
        }
        if (e.Key == Key.S)
        {
            //this.playerJump();
            this.movePlayerDown();
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
            //this.togglePause();
            this.resetPlayerWeapon();
            //this.game.playerPressTrigger(true);
        }
        if ((e.Key == Key.Escape) && (this.isEscapeEnabled()))
        {
            this.levelIsOver = true;
        }
        if (e.Key == Key.Enter)
        {
            this.togglePause();
            //this.resetPlayerWeapon();
        }
        if (e.Key == Key.RightShift)
        {
            this.pause();
        }
        if (e.Key == Key.LeftShift)
        {
            this.unPause();
        }
        base.KeyDown(sender, e);
    }
    public override void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
        // #define DESIGN_HERE
        if (e.Key == Key.W)
        {
            this.stopMovingPlayerUp();
        }
        if (e.Key == Key.W)
        {
            this.stopMovingPlayerDown();
        }
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
    // weapons
    public void selectWeapon1()
    {
        Player convertedPlayer = (Player)player;
        if (convertedPlayer != null)
            convertedPlayer.selectWeaponSubtreeAtIndex(0);
    }
    public void selectWeapon2()
    {
        Player convertedPlayer = (Player)player;
        if (convertedPlayer != null)
            convertedPlayer.selectWeaponSubtreeAtIndex(1);
    }
    public void selectWeapon3()
    {
        Player convertedPlayer = (Player)player;
        if (convertedPlayer != null)
            convertedPlayer.selectWeaponSubtreeAtIndex(2);
    }
    public void selectWeapon4()
    {
        Player convertedPlayer = (Player)player;
        if (convertedPlayer != null)
            convertedPlayer.selectWeaponSubtreeAtIndex(3);
    }
    public void resetPlayerWeapon()
    {
        Player convertedPlayer = (Player)player;
        if (convertedPlayer != null)
            convertedPlayer.gotoWeaponTreeRoot();
    }
    public void playerPressTrigger(bool pressed)
    {
        this.player.pressTrigger(pressed);
    }


    public double[] getWorldWindowSize()
    {
        return this.worldWindowSize;
    }
    double[] worldWindowSize;
    /*
    double[] statusDisplayPosition;
    double[] statusDisplaySize;
    double[] worldWindowPosition;
    */
    CharacterStatusDisplay statusDisplay;
    // the canvas to draw the world on
    Canvas worldCanvas;
    // the character that the user controls
    Character player;
    // the world that the player is in
    WorldLoader world;
    // the screen that will be shown when the level exits
    Screen exitScreen;
    // whether the user has satisfied the criteria to exit the level
    bool levelIsOver;
    // whether the pushing the escape button should exit the level
    bool escapeEnabled;
}