// Represents a rectangular region on the screen. If the game becomes 3D then it will represent a box
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;
using System.Windows;
using System.Collections.Generic;

// The WorldBox class represents a multidimensional rectangular prism of floats (an area of the world)
// Currently, it is two dimensions, which makes it a rectangle.
class WorldBox
{
// public
    public WorldBox(double x1, double x2, double y1, double y2)
    {
	    this.lowCoordinates = new double[2];
	    this.highCoordinates = new double[2];
	    this.lowCoordinates[0] = x1;
	    this.highCoordinates[0] = x2;
	    this.lowCoordinates[1] = y1;
	    this.highCoordinates[1] = y2;
    }
    public WorldBox(WorldBox original)
    {
        this.lowCoordinates = new double[original.lowCoordinates.Length];
        this.highCoordinates = new double[original.highCoordinates.Length];
        original.lowCoordinates.CopyTo(this.lowCoordinates, 0);
        original.highCoordinates.CopyTo(this.highCoordinates, 0);
    }
    public WorldBox(Size size) : this(0, size.Width, 0, size.Height)
    {
    }
    public double getLowCoordinate(int index)
    {
	    return this.lowCoordinates[index];
    }
    public double getHighCoordinate(int index)
    {
	    return this.highCoordinates[index];
    }
    public double getSize(int index)
    {
        return this.highCoordinates[index] - this.lowCoordinates[index];
    }
    public double getCenter(int index)
    {
        return (this.highCoordinates[index] + this.lowCoordinates[index]) / 2;
    }
    public double[] getCenter()
    {
        double[] center = new double[this.lowCoordinates.Length];
        int i;
        for (i = 0; i < center.Length; i++)
        {
            center[i] = this.getCenter(i);
        }
        return center;
    }
    public void shift(int dimension, double value)
    {
        this.lowCoordinates[dimension] += value;
        this.highCoordinates[dimension] += value;
    }
    public void enlarge(int index, double value)
    {
        this.lowCoordinates[index] -= value;
        this.highCoordinates[index] += value;
    }
    private
	double[] lowCoordinates;
	double[] highCoordinates;
};

// Represents a rectangular region on the screen. If the game becomes 3D then it will represent a box
// It only stores integers and is intended for use with the WorldSearcher
class IndexBox
{
// public
    public IndexBox(int x1, int x2, int y1, int y2)
    {
	    this.lowCoordinates = new int[2];
	    this.highCoordinates = new int[2];
	    this.lowCoordinates[0] = x1;
	    this.highCoordinates[0] = x2;
	    this.lowCoordinates[1] = y1;
	    this.highCoordinates[1] = y2;
    }
    public int getLowCoordinate(int index)
    {
	    return this.lowCoordinates[index];
    }
    public int getHighCoordinate(int index)
    {
	    return this.highCoordinates[index];
    }
    public int getSize(int index)
    {
	    return this.highCoordinates[index] - this.lowCoordinates[index];
    }
    // tells how many blocks are included inside this box
    // It will always be an integer, but it could be a pretty big integer, so we us a double instead of an integer
    public double countNumBlocks()
    {
        int i;
        double count = 1;
        for (i = 0; i < lowCoordinates.Length; i++)
        {
            count *= (highCoordinates[i] - lowCoordinates[i] + 1);
        }
        return count;
    }
    // tells whether there are any blocks in common between the two boxes
    public bool intersects(IndexBox other)
    {
        int i;
        for (i = 0; i < this.lowCoordinates.Length; i++)
        {
            if (lowCoordinates[i] > other.highCoordinates[i])
                return false;
            if (highCoordinates[i] < other.lowCoordinates[i])
                return false;
        }
        return true;
    }
    public bool contains(int[] coordinates)
    {
        int i;
        for (i = 0; i < this.lowCoordinates.Length; i++)
        {
            if (lowCoordinates[i] > coordinates[i])
                return false;
            if (highCoordinates[i] < coordinates[i])
                return false;
        }
        return true;
    }
    public bool equals(IndexBox other)
    {
        int i;
        for (i = 0; i < this.lowCoordinates.Length; i++)
        {
            if (lowCoordinates[i] != other.lowCoordinates[i])
                return false;
            if (highCoordinates[i] != other.highCoordinates[i])
                return false;
        }
        return true;
    }
    private
	int[] lowCoordinates;
	int[] highCoordinates;
};

// Flags to represent types
enum SHAPE
{
    GAME_SHAPE = 0,
    GAME_RECTANGLE = 1,
    GAME_CIRCLE = 2
}

// The basic shape class from which circles and rectangles inherit
class GameShape
{
// public
    public virtual double getHeight()
    {
	    return 0;
    }
    public virtual double getWidth()
    {
	    return 0;
    }
	public virtual double[] moveTo(GameCircle other, double[] offset, double[] move)
    {
        return null;
    }
	public virtual double[] moveTo(GameRectangle other, double[] offset, double[] move)
    {
        return null;
    }
    public virtual double[] moveTo(GameShape other, double[] offset, double[] move)
    {
	    if (move[0] == 0 && move[1] == 0)
		    return move;
	    // figure out the class of the shape and call the correct function accordingly
	    switch (other.getType())
	    {
	    case SHAPE.GAME_CIRCLE:
		    return this.moveTo((GameCircle)other, offset, move);
        case SHAPE.GAME_RECTANGLE:
		    return this.moveTo((GameRectangle)other, offset, move);
	    default:
		    return null;
	    }
    }
    public virtual double distanceTo(GameShape other, double[] offset)
    {
        //double[] deltas = new double[offset.Length];
        double delta;
        double dist = 0;
        // offset in the x-dimension
        delta = Math.Abs(offset[0]);
        if (this.getType() == SHAPE.GAME_RECTANGLE)
            delta -= this.getWidth() / 2;
        if (other.getType() == SHAPE.GAME_RECTANGLE)
            delta -= other.getWidth() / 2;
        if (delta < 0)
            delta = 0;
        dist += delta * delta;
        // offset in the y-dimension
        delta = Math.Abs(offset[1]);
        if (this.getType() == SHAPE.GAME_RECTANGLE)
            delta -= this.getWidth() / 2;
        if (other.getType() == SHAPE.GAME_RECTANGLE)
            delta -= other.getWidth() / 2;
        if (delta < 0)
            delta = 0;
        dist += delta * delta;
        dist = Math.Sqrt(dist);
        // subtract off the circular radius
        if (this.getType() == SHAPE.GAME_CIRCLE)
        {
            dist -= ((GameCircle)this).getRadius();
        }
        if (other.getType() == SHAPE.GAME_CIRCLE)
        {
            dist -= ((GameCircle)other).getRadius();
        }
        if (dist < 0)
            dist = 0;
        return dist;
    }
    public virtual bool intersects(GameCircle other, double[] offset)
    {
        return false;
    }
    public virtual bool intersects(GameRectangle other, double[] offset)
    {
        return false;
    }
    public virtual bool intersects(GameShape other, double[] offset)
    {
        // figure out the class of the shape and call the correct function accordingly
        switch (other.getType())
        {
            case SHAPE.GAME_CIRCLE:
                return this.intersects((GameCircle)other, offset);
            case SHAPE.GAME_RECTANGLE:
                return this.intersects((GameRectangle)other, offset);
            default:
                return false;
        }
    }
    public virtual SHAPE getType()
    {
	    return SHAPE.GAME_SHAPE;
    }
};

// A rectangle
class GameRectangle : GameShape
{
//public
	public GameRectangle(double newWidth, double newHeight)
    {
        this.width = newWidth;
        this.height = newHeight;
    }
	public override double getWidth()
    {
        return this.width;
    }
	public override double getHeight()
    {
        return this.height;
    }
    // Returns the maximum amount of distance that this can move in the move direction, given another rectangle at offset
	public override double[] moveTo(GameRectangle other, double[] offset, double[] move)
    {
	    GameRectangle r1 = this;
	    GameRectangle r2 = other;
	    //double length = Math.Sqrt(move[0] * move[0] + move[1] * move[1]);
	    double width = (r1.getWidth() + r2.getWidth()) / 2;
	    double height = (r1.getHeight() + r2.getHeight()) / 2;

	    // if they can't collide in this direction then any length is okay
	    if ((Math.Abs(offset[0]) >= width) && (offset[0] * move[0] <= 0))
		    return move;
	    if ((Math.Abs(offset[1]) >= height) && (offset[1] * move[1] <= 0))
		    return move;

	    // figure out if any edges do collide in this direction
	    double dir1 = (width - offset[0]) * move[1] - (height - offset[1]) * move[0];
	    double dir2 = (width - offset[0]) * move[1] - (-height - offset[1]) * move[0];
	    double dir3 = (-width - offset[0]) * move[1] - (height - offset[1]) * move[0];
	    double dir4 = (-width - offset[0]) * move[1] - (-height - offset[1]) * move[0];
	    // If no edges collide then any length is okay
	    if ((dir1 >= 0) && (dir2 >= 0) && (dir3 >= 0) && (dir4 >= 0))
		    return move;
	    if ((dir1 <= 0) && (dir2 <= 0) && (dir3 <= 0) && (dir4 <= 0))
		    return move;

	    double[] allowedMove = new double[2];
	    // figure out whether the top/bottom edges, or the left/right edges will collide
	    if ((Math.Abs(offset[0]) - width) * Math.Abs(move[1]) <= (Math.Abs(offset[1]) - height) * Math.Abs(move[0]))
	    {
		    // check the top/bottom edges and find the closer collision
            if (offset[1] > 0)
            {
                // make sure it is stable with respect to rounding error
                allowedMove[1] = Math.Min(offset[1] - height, move[1]);
            }
            else
            {
                // make sure it is stable with respect to rounding error
                allowedMove[1] = Math.Max(offset[1] + height, move[1]);
            }
		    // don't force them to move further than desired
		    if (Math.Abs(allowedMove[1]) >= Math.Abs(move[1]))
			    return move;
		    // The reason for the following check is to make it stable with respect to rounding error. The condition should be always true but in reality may not be
		    if ((Math.Abs(offset[0]) >= width) || (Math.Abs(offset[1]) >= height))
			    allowedMove[0] = allowedMove[1] * move[0] / move[1];
		    else
			    allowedMove[0] = -allowedMove[1] * move[0] / move[1];
	    }
	    else
	    {
		    // check the left/right edges and find the closer collision
            if (offset[0] > 0)
            {
                // make sure it is stable with respect to rounding error
                allowedMove[0] = Math.Min(offset[0] - width, move[0]);
            }
            else
            {
                // make sure it is stable with respect to rounding error
                allowedMove[0] = Math.Max(offset[0] + width, move[0]);
            }
		    // don't force them to move further than desired
		    if (Math.Abs(allowedMove[0]) >= Math.Abs(move[0]))
			    return move;
		    // The reason for the following check is to make it stable with respect to rounding error. The condition should be always true but in reality may not be
		    if ((Math.Abs(offset[0]) >= width) || (Math.Abs(offset[1]) >= height))
			    allowedMove[1] = allowedMove[0] * move[1] / move[0];
		    else
			    allowedMove[1] = -allowedMove[0] * move[1] / move[0];
	    }
	    return allowedMove;
    }
    // Returns the maximum amount of distance that this can move in the move direction, given a circle at offset
    public override double[] moveTo(GameCircle other, double[] offset, double[] move)
    {
	    // This collision checker happens to be symetric
	    double[] allowedMove = other.moveTo(this, offset, move);
	    return allowedMove;
    }
    // Tells whether this intersects another rectangle at offset
    public override bool intersects(GameRectangle other, double[] offset)
    {
        if (Math.Abs(offset[0]) >= (this.getWidth() + other.getWidth()) / 2)
            return false;
        if (Math.Abs(offset[1]) >= (this.getHeight() + other.getHeight()) / 2)
            return false;
        return true;
    }
    // Tells whether this intersects a circle at offset
    public override bool intersects(GameCircle other, double[] offset)
    {
        // This checker is symetric
        return other.intersects(this, offset);
    }

    public override SHAPE getType()
    {
	    return SHAPE.GAME_RECTANGLE;
    }

private
	double width, height;
};

// A circle
class GameCircle : GameShape
{
//public
	public GameCircle(double newRadius)
    {
        this.radius = newRadius;
    }
    public double getRadius()
    {
        return this.radius;
    }
	public override double getHeight()
    {
        return this.radius * 2;
    }
	public override double getWidth()
    {
        return this.radius * 2;
    }
    // Returns the maximum amount of distance that this can move in the move direction, given a rectangle at offset
    public override double[] moveTo(GameRectangle other, double[] offset, double[] move)
    {
	    GameCircle c = this;
	    GameRectangle r = other;
	    double[] mirroredMove = new double[move.Length];
        double[] mirroredOffset = new double[move.Length];
	    int i;
	    // flip the coordinate system so that the offset is positive in all dimensions
	    for (i = 0; i < move.Length; i++)
	    {
		    if (offset[i] >= 0)
		    {
			    mirroredOffset[i] = offset[i];
			    mirroredMove[i] = move[i];
		    }
		    else
		    {
			    mirroredOffset[i] = -offset[i];
			    mirroredMove[i] = -move[i];
		    }
	    }
	    //System.Collections.Generic.List<double[]> intersections;
	    //double[] tempIntersection;
	    // figure out where it may intersect the left/right wall
	    double width = r.getWidth() / 2 + c.getRadius();
	    if (mirroredMove[0] > 0 && mirroredOffset[0] >= width)
	    {
		    // calculate where it will intersect this line
		    double[] allowedMove = new double[2];		
		    allowedMove[0] = mirroredOffset[0] - width;
		    // check that it will go far enough to collide
		    if (allowedMove[0] < mirroredMove[0])
		    {
			    allowedMove[1] = allowedMove[0] * mirroredMove[1] / mirroredMove[0];
			    // make sure that it actually intersects the line segment and not just the line
			    if (Math.Abs(mirroredOffset[1] - allowedMove[1]) <= r.getHeight() / 2)
			    {
				    // reflect it back correctly and return
				    if (offset[0] < 0)
					    allowedMove[0] *= -1;
				    if (offset[1] < 0)
					    allowedMove[1] *= -1;
				    return allowedMove;
			    }
		    }
	    }
	    double height = r.getHeight() / 2 + c.getRadius();
	    if (mirroredMove[1] > 0 && mirroredOffset[1] >= height)
	    {
		    // calculate where it will intersect this line
		    double[] allowedMove = new double[2];		
		    allowedMove[1] = mirroredOffset[1] - height;
		    // check that it will go far enough to collide
		    if (allowedMove[1] < mirroredMove[1])
		    {
			    allowedMove[0] = allowedMove[1] * mirroredMove[0] / mirroredMove[1];
			    // make sure that it actually intersects the line segment and not just the line
			    if (Math.Abs(mirroredOffset[0] - allowedMove[0]) <= r.getWidth() / 2)
			    {
				    // reflect it back correctly and return
				    if (offset[0] < 0)
					    allowedMove[0] *= -1;
				    if (offset[1] < 0)
					    allowedMove[1] *= -1;
				    return allowedMove;
			    }
		    }
	    }
	    // figure out where it may intersect the corner arc
	    double length = Math.Sqrt(move[0] * move[0] + move[1] * move[1]);
	    double signedWidth = r.getWidth() / 2;
	    double signedHeight = r.getHeight() / 2;
	    for (i = 0; i < 2; i++)
	    {
		    double lateralOffset = (mirroredOffset[0] - signedWidth) * mirroredMove[1] / length - (mirroredOffset[1] - signedHeight) * mirroredMove[0] / length;
		    // first check that the movement line will reach the arc
		    if (Math.Abs(lateralOffset) <= c.getRadius())
		    {
			    double forwardOffset = (mirroredOffset[0] - signedWidth) * mirroredMove[0] / length + (mirroredOffset[1] - signedHeight) * mirroredMove[1] / length;
			    // Next check that the movement line is facing the right direction
			    if (forwardOffset >= 0)
			    {
				    double collisionDist = forwardOffset - Math.Sqrt(c.getRadius() * c.getRadius() - lateralOffset * lateralOffset);
				    // Make sure that it is hitting the outside of the arc
				    if ((Math.Abs(mirroredOffset[0] - mirroredMove[0] * collisionDist / length) >= r.getWidth() / 2) &&
					    (Math.Abs(mirroredOffset[1] - mirroredMove[1] * collisionDist / length) >= r.getHeight() / 2))
				    {
					    // If we get here then it is, in fact, moving toward the corner of the rectangle from the outside. Now decide if it wants to move that far
					    // if it doesn't want to move that far, then it can just keep going
					    if (collisionDist >= length)
					    {
						    return move;
					    }
					    else
					    {
						    // If we get here, it means is would collide with the arc and we need to shorten the length
						    double[] allowedMove = new double[2];
						    allowedMove[0] = move[0] * collisionDist / length;
						    allowedMove[1] = move[1] * collisionDist / length;
						    return allowedMove;
					    }
				    }
			    }
		    }
		    // Now alter the coordinates to check the other corner, and repeat
		    if ((mirroredOffset[0] - signedWidth) * mirroredMove[0] >= (mirroredOffset[1] - signedHeight) * mirroredMove[1])
		    {
			    // switch vertically because we're closer to the left/right edge
			    signedHeight *= -1;
		    }
		    else
		    {
			    // switch vertically because we're closer to the left/right edge
			    signedWidth *= -1;
		    }
	    }
	    // If we get here, there are no collisions and so any move is fine
	    return move;
    }
    // Returns the largest move that this can make in the move direction, given another circle at offset
    public override double[] moveTo(GameCircle other, double[] offset, double[] move)
    {
	    GameCircle c1 = this;
	    GameCircle c2 = other;
	    double radius = c1.getRadius() + c2.getRadius();
	    double desiredLength = Math.Sqrt(move[0] * move[0] + move[1] * move[1]);
	    double distFromPath = Math.Abs(offset[0] * move[1] - offset[1] * move[0]) / desiredLength;
	    if ((distFromPath >= radius) || (offset[0] * move[0] + offset[1] * move[1] <= 0))
	    {
		    // if we get here then the circles never collide and so any move is okay
		    return move;	// return the same move to indicate that this move is okay
	    }
	    double distToClosestPoint = Math.Sqrt((offset[0] * offset[0] + offset[1] * offset[1]) - distFromPath * distFromPath);
	    double allowedLength = distToClosestPoint - Math.Sqrt(radius * radius - distFromPath * distFromPath);
        if (allowedLength >= desiredLength)
        {
            // if we get here then the desired move is not long enough to collide
            return move;
        }
	    double[] result = new double[2];	// create a new array to indicate that this move is different
	    result[0] = move[0] * allowedLength / desiredLength;
	    result[1] = move[1] * allowedLength / desiredLength;
	    return result;
    }
    // Tells whether a GameCircle would intersect another at offset
    public override bool intersects(GameCircle other, double[] offset)
    {
        double totalRadius = this.radius + other.radius;
        if (offset[0] * offset[0] + offset[1] * offset[1] < totalRadius * totalRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Tells whether a GameCircle would intersect a GameRectangle at offset
    public override bool intersects(GameRectangle other, double[] offset)
    {
        double deltaX = Math.Abs(offset[0]) - other.getWidth() / 2;
        if (deltaX < 0)
            deltaX = 0;
        double deltaY = Math.Abs(offset[1]) - other.getHeight() / 2;
        if (deltaY < 0)
            deltaY = 0;
        if (deltaX * deltaX + deltaY * deltaY < this.radius * this.radius)
            return true;
        else
            return false;
    }

    public override SHAPE getType()
    {
        return SHAPE.GAME_CIRCLE;
    }
private
	double radius;
};

// The basic class of object in the game, which has an image and collision detection
class GameObject
{
//public
    // default constructor
    public GameObject()
    {
        this.setRenderable(true);
        this.initialize();
    }
    public GameObject(bool enableRendering)
    {
        this.setRenderable(enableRendering);
        this.initialize();
    }
    void initialize()
    {
        if (this.isRenderable())
        {
            // add the image to the screen
            this.renderTransform = new MatrixTransform();
            this.zIndex = 1;
            this.image = new Image();
            image.Stretch = Stretch.Fill;
            this.imageOffset = new double[2];
        }
        this.center = new double[2];
        this.velocity = new double[2];
        this.resetTimeMultiplier();
        if (this.isStunnable())
            this.stuns = new System.Collections.Generic.Dictionary<Explosion, Stun>(1);
        //this.activations = new System.Collections.Generic.List<int>();
    }
    // copy constructor
    public GameObject(GameObject original)
    {
        this.initialize();
        this.copyFrom(original);
    }
    public GameObject(GameObject original, bool enableRendering)
    {
        this.setRenderable(enableRendering);
        this.initialize();
        this.copyFrom(original);
    }
    public void copyFrom(GameObject original)
    {
        this.setShape(original.shape);
        if (this.isRenderable())
        {
            this.setBitmap(original.getBitmap());
            this.zIndex = original.zIndex;
            this.setImageOffset(original.getImageOffset());
        }
        this.setCenter(original.getCenter());
        this.setVelocity(original.getVelocity());
        this.hitpoints = original.hitpoints;
        this.maxHitpoints = original.maxHitpoints;
        this.teamNum = original.teamNum;
        this.gravity = original.gravity;
        this.setDragCoefficient(original.getDragCoefficient());
    }
    // tells whether this object can make itself move
    public virtual bool isMovable()
    {
        return false;
    }

    public double[] getCenter()
    {
	    return this.center;
    }
    public void setCenter(double[] newCenter)
    {
        center[0] = newCenter[0];
        center[1] = newCenter[1];	    
        this.updateImageCoordinates();
    }
    public void setZIndex(int z)
    {
        this.zIndex = z;
        this.updateImageCoordinates();
    }
    public double getLeft()
    {
        return this.center[0] - this.shape.getWidth() / 2;
    }
    public double getRight()
    {
        return this.center[0] + this.shape.getWidth() / 2;
    }
    public double getTop()
    {
        return this.center[1] + this.shape.getHeight() / 2;
    }
    public double getBottom()
    {
        return this.center[1] - this.shape.getHeight() / 2;
    }
    public double[] getImageOffset()
    {
	    return this.imageOffset;
    }
    public void setImageOffset(double[] newOffset)
    {
	    this.imageOffset = newOffset;
    }
    public BitmapImage getBitmap()
    {
	    return this.bitmap;
    }
    public void setBitmap(BitmapImage newBitmap)
    {
        // save the bitmap in case we need it later
	    this.bitmap = newBitmap;
        this.image.Source = bitmap;
        if (this.shape != null)
        {
            this.image.Width = this.shape.getWidth();
            this.image.Height = this.shape.getHeight();
        }
        this.updateImageCoordinates();
    }
    public System.Windows.Controls.Image getImage()
    {
        return this.image;
    }
    public void updateImageCoordinates()
    {
        if (this.bitmap != null)
        {
            // The world coordinate system has (0,0) at the bottom left
            // The screen coordinate system has (0,0) at the top left
            // The GameObject handles the reflection part and the world handles the translation
            Canvas.SetLeft(this.image, this.getCenter()[0] - image.Width / 2 + this.getImageOffset()[0]);
            Canvas.SetTop(this.image, -this.getCenter()[1] - image.Height / 2 - this.getImageOffset()[1]);
            Canvas.SetZIndex(this.image, this.zIndex);
            if (this.isFacingRight())
            {
                this.renderTransform.Matrix = new Matrix(1, 0, 0, 1, 0, 0);
            }
            else
            {
                this.renderTransform.Matrix = new Matrix(-1, 0, 0, 1, this.image.Width, 0);
            }
        }
    }
    public bool isRenderable()
    {
        return this.renderable;
    }
    public void setRenderable(bool value)
    {
        this.renderable = value;
    }
    public Transform getRenderTransform()
    {
        return this.renderTransform;
    }
    /*public void setImage(Image newImage)
    {
        this.image = newImage;
    }*/
    public double[] getVelocity()
    {
	    return this.velocity;
    }
    public void setVelocity(double[] newVelocity)
    {
        //if (!(newVelocity[0] <= 0) && !(newVelocity[0] >= 0))
        //    newVelocity = newVelocity;
        this.velocity[0] = newVelocity[0];
        this.velocity[1] = newVelocity[1];
	    //this.velocity = newVelocity;
    }
    public double getGravity()
    {
	    return this.gravity;
    }
    public void setGravity(double newGravity)
    {
	    this.gravity = newGravity;
    }
    public double getDragCoefficient()
    {
	    return this.dragCoefficient;
    }
    public void setDragCoefficient(double newDrag)
    {
	    this.dragCoefficient = newDrag;
    }
    public bool isFacingRight()
    {
        return !this.facingLeft;
    }
    public bool isFacingLeft()
    {
        return this.facingLeft;
    }
    public void setFacingLeft(bool value)
    {
        if (this.facingLeft != value)
        {
            this.facingLeft = value;
            this.updateImageCoordinates();
        }
    }
    public void setFacingRight(bool value)
    {
        if (this.facingLeft == value)
        {
            this.facingLeft = !value;
            this.updateImageCoordinates();
        }
    }
    public GameShape getShape()
    {
	    return this.shape;
    }
    public void setShape(GameShape newShape)
    {
	    this.shape = newShape;
        this.image.Width = newShape.getWidth();
        this.image.Height = newShape.getHeight();
    }
    public double getHitpoints()
    {
        return this.hitpoints;
    }
    public void setHitpoints(double newHitpoints)
    {
        this.hitpoints = newHitpoints;
    }
    public double getMaxHitpoints()
    {
        return this.maxHitpoints;
    }
    public void initializeHitpoints(double value)
    {
        this.hitpoints = this.maxHitpoints = value;
    }
    public bool isAlive()
    {
        if (this.hitpoints >= 0)
            return true;
        else
            return false;
    }
    public virtual void updateVelocity(double numSeconds)
    {
        // now calculate accordingly
	    double[] v = this.getVelocity();
        int i;
        double[] stableVelocity = new double[2];
        double scale;
        double[] newV = new double[2];
        double[] g = new double[2];
        g[0] = 0;
        g[1] = -this.gravity;
#if true
        if (this.getDragCoefficient() != 0)
        {
            for (i = 0; i < 2; i++)
            {
                // At time equals infinity, velocity * drag = gravity
                // dv/dt = gravity - velocity * drag;
                // So v = (gravity/drag) + c * e^(-drag*t)
                // The stable velocity is (gravity / drag)
                // Compute the asymptotic value
                stableVelocity[i] = g[i] / this.getDragCoefficient();
                // Compute the current distance to the asymptote
                scale = v[i] - stableVelocity[i];
                // Compute the new distance from the asymptote
                newV[i] = stableVelocity[i] + scale * Math.Exp(-this.getDragCoefficient() * numSeconds);
            }
        }
        else
        {
            for (i = 0; i < 2; i++)
            {
                newV[i] = v[i] + g[i] * numSeconds;
            }
        }
        this.setVelocity(newV);
#else

        // Apply gravity
	    v[1] -= (float)(this.gravity * numSeconds);

	    // Apply air resistance
	    double drag;
	    drag = numSeconds * this.getDragCoefficient();
	    /*if (drag > 1)
		    drag = 1;*/
	    v[0] *= (float)(1 - drag);
	    v[1] *= (float)(1 - drag);


	    // Save the new velocity
	    this.setVelocity(v);
#endif
    }

    // get the bounding box for this object where it currently is
    public WorldBox getBoundingBox()
    {
	    WorldBox box = new WorldBox(this.center[0] - this.getShape().getWidth() / 2, this.center[0] + this.getShape().getWidth() / 2,
		    this.center[1] - this.getShape().getHeight() / 2, this.center[1] + this.getShape().getHeight() / 2);
	    return box;
    }
    // get the bounding box for where this object would be if it were to make the given move
    public WorldBox getBoundingBoxShifted(double[] move)
    {
	    WorldBox currentBox = this.getBoundingBox();
	    WorldBox shiftedBox = new WorldBox(currentBox.getLowCoordinate(0) + move[0], currentBox.getHighCoordinate(0) + move[0],
		    currentBox.getLowCoordinate(1) + move[1], currentBox.getHighCoordinate(1) + move[1]);
	    return shiftedBox;
    }
    // get the bounding box of all the positions the box would 
    public WorldBox getBoundingBoxForMove(double[] move)
    {
	    double x1, x2, y1, y2;
	    if (move[0] >= 0)
	    {
		    x1 = this.center[0] - this.getShape().getWidth() / 2;
		    x2 = this.center[0] + this.getShape().getWidth() / 2 + move[0];
	    }
	    else
	    {
		    x1 = this.center[0] - this.getShape().getWidth() / 2 + move[0];
		    x2 = this.center[0] + this.getShape().getWidth() / 2;
	    }
	    if (move[1] >= 0)
	    {
		    y1 = this.center[1] - this.getShape().getHeight() / 2;
		    y2 = this.center[1] + this.getShape().getHeight() / 2 + move[1];
	    }
	    else
	    {
		    y1 = this.center[1] - this.getShape().getHeight() / 2 + move[1];
		    y2 = this.center[1] + this.getShape().getHeight() / 2;
	    }
	    WorldBox box = new WorldBox(x1, x2, y1, y2);
	    return box;
    }

    public double[] moveTo(GameObject other, double[] move)
    {
	    GameObject g1 = this;
	    GameObject g2 = other;
	    GameShape s1 = g1.getShape();
	    GameShape s2 = g2.getShape();
	    double[] offset = new double[2];
	    offset[0] = g2.getCenter()[0] - g1.getCenter()[0];
	    offset[1] = g2.getCenter()[1] - g1.getCenter()[1];
        // calculate the maximum allowable move
        double[] result = s1.moveTo(s2, offset, move);
        // figure out whether this is the same move or an altered one
        if (move[0] == result[0] && move[1] == result[1])
            return move;
        return result;
    }
    // tells how far apart two GameObjects are
    public double distanceTo(GameObject other)
    {
        GameObject g1 = this;
        GameObject g2 = other;
        GameShape s1 = g1.getShape();
        GameShape s2 = g2.getShape();
        double[] offset = new double[2];
        offset[0] = g2.getCenter()[0] - g1.getCenter()[0];
        offset[1] = g2.getCenter()[1] - g1.getCenter()[1];
        return s1.distanceTo(s2, offset);
    }
    // tells whether two GameObjects are currently overlapping
    public bool intersects(GameObject other)
    {
        GameObject g1 = this;
        GameObject g2 = other;
        GameShape s1 = g1.getShape();
        GameShape s2 = g2.getShape();
        double[] offset = new double[2];
        offset[0] = g2.getCenter()[0] - g1.getCenter()[0];
        offset[1] = g2.getCenter()[1] - g1.getCenter()[1];
        return s1.intersects(s2, offset);
    }
    // returns a vector indicating how far the object would move in the given amount of time (assuming no collisions)
    public double[] getMove(double numSeconds)
    {
        // now calculate accordingly
	    double[] move = new double[2];
	    move[0] = this.velocity[0] * numSeconds;
	    move[1] = this.velocity[1] * numSeconds;
        if (double.IsInfinity(move[0]) || double.IsInfinity(move[1]))
        {
            throw new ArithmeticException("Move cannot be infinite");
        }

        return move;
    }
    public virtual void setColliding(GameObject other)
    {
        // if it is just a GameObject then it doesn't need to remember whether it is colliding
        return;
    }
    public virtual bool isColliding()
    {
        // if it is just a GameObject then it doesn't need to remember whether it is colliding
        return false;
    }
    public virtual void setCollisionLocation(double[] location)
    {
        // if it is just a GameObject then it doesn't need to remember where it is colliding
        return;
    }
    public virtual double[] getCollisionLocation()
    {
        // if it is just a GameObject then it doesn't need to remember where it is colliding
        return null;
    }
    public virtual void setCollisionMove(double[] move)
    {
        // if it is just a GameObject then it doesn't need to remember where it is colliding
        return;
    }
    public virtual void setTeamNum(int value)
    {
	    this.teamNum = value;
    }
    public int getTeamNum()
    {
	    return this.teamNum;
    }
    public virtual bool isAProjectile()
    {
        return false;
    }
    public virtual bool isAnExplosion()
    {
        return false;
    }
    public virtual bool isACharacter()
    {
        return false;
    }
    public virtual bool isAPlatform()
    {
        return false;
    }
    public virtual bool isAPainting()
    {
        return false;
    }
    public virtual bool isAGhost()
    {
        return false;
    }
    public virtual bool isAPortal()
    {
        return false;
    }
    public virtual bool isAPickupItem()
    {
        return false;
    }
    public virtual void takeDamage(double damagePerSec, double numSeconds)
    {
        // damage is not scaled by the stasis field
        // subtract the armor per second and take damage equal to the result
        this.hitpoints -= (damagePerSec * numSeconds);
        if (this.hitpoints >= this.maxHitpoints)
            this.hitpoints = this.maxHitpoints;
    }
    public virtual void applyStun(Stun newStun)
    {
        // only process the stun if this object type cares about it
        if (this.stuns != null)
        {
            // figure out what created this stun
            Explosion source = newStun.getCreator();
            Stun previous = null;
            if (this.stuns.TryGetValue(source, out previous))
            {
                // if we were already stunned by this source then replace the previous stun
                previous = newStun;
            }
            else
            {
                // if we were not already stunned by this source then add this stun
                this.stuns.Add(source, newStun);
            }
        }
    }
    public virtual void processStuns(double numSeconds)
    {
        if (this.stuns != null)
        {
            this.resetTimeMultiplier();
            double duration;
            System.Collections.ArrayList finishedStuns = new System.Collections.ArrayList(1);

            // adjust the time multiplier and take damage
            foreach (Stun currentStun in this.stuns.Values)
            {
                this.scaleTimeMultiplier(currentStun.getTimeMultiplier());
                duration = currentStun.getRemainingDuration(numSeconds);
                this.takeDamage(currentStun.getDamagePerSecond(), duration);
                if (currentStun.isFinished(numSeconds))
                    finishedStuns.Add(currentStun);
            }

            // accelerate
            double[] velocity = this.getVelocity();
            double multiplier = numSeconds;
            foreach (Stun currentStun in this.stuns.Values)
            {
                velocity[0] += currentStun.getAccel()[0] * multiplier;
                velocity[1] += currentStun.getAccel()[1] * multiplier;
            }
            // update velocity
            this.setVelocity(velocity);
            foreach (Stun currentStun in finishedStuns)
            {
                this.stuns.Remove(currentStun.getCreator());
            }
        }
    }
    public virtual bool isStunnable()
    {
        // anything that is just a GameObject doesn't get stunned or damaged
        return false;
    }
    public void clearStuns()
    {
        this.stuns.Clear();
    }
    public double getTimeMultiplier()
    {
        return this.slowerTimeMultiplier * this.fasterTimeMultiplier;
        //return this.timeMultiplier;
    }
    public void scaleTimeMultiplier(double scale)
    {
        if (double.IsInfinity(scale)) {
            throw new ArgumentException("time multiplier cannot be infinite");
        }
        this.fasterTimeMultiplier = Math.Max(this.fasterTimeMultiplier, scale);
        this.slowerTimeMultiplier = Math.Min(this.slowerTimeMultiplier, scale);
        //this.timeMultiplier *= scale;
    }
    public void resetTimeMultiplier()
    {
        this.slowerTimeMultiplier = this.fasterTimeMultiplier = 1;
        //this.timeMultiplier = 1;
    }
    // finds the object that is closest to this one, among objects in the candidate list
    public GameObject findClosest(System.Collections.Generic.List<GameObject> candidates)
    {
        GameObject closest = null;
        double minDist = 0;
        double tempDist;
        foreach (GameObject other in candidates)
        {
            tempDist = this.distanceTo(other);
            if ((closest == null) || (tempDist < minDist))
            {
                minDist = tempDist;
                closest = other;
            }
        }
        return closest;
    }
    public virtual double getContactDamagePerSecond()
    {
        return 0;
    }
    //public System.Collections.Generic.List<int> activations;
    public System.Collections.Generic.IEnumerable<Stun> getStuns()
    {
        return this.stuns.Values;
    }
    
// private
	// location
	double[] center;
    int zIndex;
	// image
	BitmapImage bitmap;
	System.Windows.Controls.Image image;
	// image positioning
	double[] imageOffset;
	// position
	double[] velocity;
	// Acceleration due to gravity. -1 is normal and indicates falling toward the bottom of the screen.
	double gravity;
	// The shape that the object is, such as circle or rectangle
	GameShape shape;
	// Motion is repeatedly decreased by numSeconds * speed * dragCoefficient
	double dragCoefficient;
	int teamNum;
    double hitpoints;
    double maxHitpoints;
    // When the GameObject is inside a time bubble, the timeMultiplier gives the ratio of local time to world time. Larger numbers are faster times
    double fasterTimeMultiplier = 1; // the component of the timeMultiplier that increases speed
    double slowerTimeMultiplier = 1; // the component of the timeMultiplier that decreases speed
    Dictionary<Explosion, Stun> stuns;
    MatrixTransform renderTransform;
    bool facingLeft;
    bool renderable;
};
