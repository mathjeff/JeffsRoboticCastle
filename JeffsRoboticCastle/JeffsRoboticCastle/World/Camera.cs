using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

// A Camera represents a view of a certain area of a world, along with the transform required to display that area
class Camera
{
    public Camera(WorldBox worldRect, WorldBox screenRect)
    {
        this.worldBox = worldRect;
        this.screenBox = screenRect;
        this.Transform = new MatrixTransform();

        // The padding is a measure of the minimum amount of world we always show on each side of the user
        this.padding = new double[2];
        padding[0] = worldRect.getSize(0) / 6;
        padding[1] = worldRect.getSize(1) / 12;
    }
    public WorldBox getWorldBox()
    {
        return this.worldBox;
    }
    
    // scrolls to the given object and returns a bool telling whether it moved
    public bool scrollTo(GameObject o)
    {
        bool moved = false;
        double deltaX;
        deltaX = (o.getLeft() + o.getVelocity()[0] * .2) - (this.worldBox.getLowCoordinate(0) + padding[0]);
        if (deltaX < 0)
        {
            this.worldBox.shift(0, deltaX);
            moved = true;
        }
        else
        {
            deltaX = (o.getRight() + o.getVelocity()[0] * .2) - (this.worldBox.getHighCoordinate(0) - padding[0]);
            if (deltaX > 0)
            {
                this.worldBox.shift(0, deltaX);
                moved = true;
            }
        }
        double deltaY;
        deltaY = (o.getBottom() + o.getVelocity()[1] * .2) - (this.worldBox.getLowCoordinate(1) + padding[1]);
        if (deltaY < 0)
        {
            this.worldBox.shift(1, deltaY);
            moved = true;
        }
        else
        {
            deltaY = (o.getTop() + o.getVelocity()[1] * .2) - (this.worldBox.getHighCoordinate(1) - padding[1]);
            if (deltaY > 0)
            {
                this.worldBox.shift(1, deltaY);
                moved = true;
            }
        }

        if (moved)
        {
            // update the transform that we're supposed to update
            this.saveTransformIn(this.Transform);
        }

        // tell whether anything changed
        
        return moved;
    }
    // transformation to be applied to images to convert from worldRect to screenRect
    public MatrixTransform Transform;
    // saves into t the transform required to convert from world coordinates to screen coordinates
    private void saveTransformIn(MatrixTransform t)
    {
        t.Matrix = new Matrix(1, 0, 0, 1, screenBox.getLowCoordinate(0) - worldBox.getLowCoordinate(0),
            screenBox.getHighCoordinate(1) + worldBox.getLowCoordinate(1));
        // currently we don't bother calculating the stretch factor because it should be 1 in each dimension
    }
    // variables
    WorldBox worldBox;
    WorldBox screenBox;
    double[] padding;
    
}