using System.Windows.Media.Imaging;
class Platform : GameObject
{
public
    // constructor
	Platform(double[] location, int type)
    {
        BitmapImage startingBitmap = null;
        switch (type)
        {
            case 0:
                startingBitmap = ImageLoader.loadImage("stone.png");
	            this.setShape(new GameRectangle(100, 10));
                break;
            case 1:
                startingBitmap = ImageLoader.loadImage("wall.png");
                this.setShape(new GameRectangle(20, 100));
                break;
            default:
                System.Diagnostics.Trace.WriteLine("Error, undefined platform number");
                break;
        }
        this.setCenter(location);
        this.setBitmap(startingBitmap);
        this.setImageOffset(new double[2]);
        this.setGravity(0);
        this.setDragCoefficient(0);
    }
    public override bool isAPlatform()
    {
        return true;
    }
};
