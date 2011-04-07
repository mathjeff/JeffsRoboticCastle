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
            return this.exitScreen;
        else
            return this;
    }
    public void setExitScreen(Screen newScreen)
    {
        this.exitScreen = newScreen;
    }
    public void playerJump()
    {
        this.player.jump();
    }
    public void movePlayerLeft()
    {
        double[] newV = new double[2]; newV[0] = -10000; newV[1] = 0;
        this.player.setTargetVelocity(newV);
    }
    public override void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
        base.KeyDown(sender, e);
    }
    public override void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
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
        base.KeyUp(sender, e);
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
    Canvas worldCanvas;
    Character player;
    WorldLoader world;
    Screen exitScreen;
}