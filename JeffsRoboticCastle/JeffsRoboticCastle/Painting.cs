using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

class Painting : GameObject
{
    public Painting(double[] location, int type)
    {
        BitmapImage startingBitmap = null;
        switch (type)
        {
            case 0:
                startingBitmap = ImageLoader.loadImage("Painting1.png");
                this.setShape(new GameRectangle(100, 100));
                break;
            case 1:
                startingBitmap = ImageLoader.loadImage("Wallpaper.png");
                this.setShape(new GameRectangle(1024, 1024));
                break;
            default:
                System.Diagnostics.Trace.WriteLine("undefined painting type");
                this.setShape(new GameRectangle(100, 100));
                break;
        }
        this.setBitmap(startingBitmap);
        this.setCenter(location);
        this.setImageOffset(new double[2]);
        this.setGravity(0);
        this.setDragCoefficient(0);
        this.setZIndex(0);
    }

}
