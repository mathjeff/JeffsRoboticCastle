using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

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
    public void followCharacter(Character player)
    {
        this.statusDisplay.followCharacter(player);
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
        this.statusDisplay.update();
        return base.timerTick(numSeconds);
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
}