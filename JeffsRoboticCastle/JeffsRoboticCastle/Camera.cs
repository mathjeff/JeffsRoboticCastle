using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

class Camera
{
    public Camera(WorldBox worldRect, WorldBox screenRect)
    {
        this.worldBox = worldRect;
        this.screenBox = screenRect;
        // The padding is a measure of the minimum amount of world we always show on each side of the user
        this.padding = new double[2];
        padding[0] = worldRect.getSize(0) / 6;
        padding[1] = worldRect.getSize(1) / 12;
    }
    public WorldBox getWorldBox()
    {
        return this.worldBox;
    }
    // saves into t the transform required to convert from world coordinates to screen coordinates
    public void saveTransformIn(MatrixTransform t)
    {
        t.Matrix = new Matrix(1, 0, 0, 1, screenBox.getLowCoordinate(0) - worldBox.getLowCoordinate(0),
            screenBox.getHighCoordinate(1) + worldBox.getLowCoordinate(1));
        // currently we don't bother calculating the stretch factor because it should be 1 in each dimension
        // To do a more complicated transform, use a MatrixTransform
        //TranslateTransform t = new TranslateTransform(screenRect.Left - worldRect.Left, screenRect.Top + worldRect.Top);
        //return null;
    }
    // scrolls to the given object and returns a bool telling whether it moved
    public bool scrollTo(GameObject o)
    {
        bool moved = false;
        if (this.worldBox.getLowCoordinate(0) + padding[0] > o.getLeft())
        {
            this.worldBox.shift(0, o.getLeft() - (worldBox.getLowCoordinate(0) + padding[0]));
            moved = true;
        }
        else
        {
            if (worldBox.getHighCoordinate(0) - padding[0] < o.getRight())
            {
                this.worldBox.shift(0, o.getRight() - (worldBox.getHighCoordinate(0) - padding[0]));
                moved = true;
            }
        }
        if (this.worldBox.getLowCoordinate(1) + padding[1] > o.getBottom())
        {
            this.worldBox.shift(1, o.getBottom() - (worldBox.getLowCoordinate(1) + padding[1]));
            moved = true;
        }
        else
        {
            if (worldBox.getHighCoordinate(1) - padding[1] < o.getTop())
            {
                this.worldBox.shift(1, o.getTop() - (worldBox.getHighCoordinate(1) - padding[0]));
                moved = true;
            }
        }
        return moved;
    }
    // variables
    WorldBox worldBox;
    WorldBox screenBox;
    double[] padding;
    
}
