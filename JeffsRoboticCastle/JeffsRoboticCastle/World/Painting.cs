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
        this.setZIndex(0);
        switch (type)
        {
            case 0:
                startingBitmap = ImageLoader.loadImage("Painting1.png");
                this.setShape(new GameRectangle(300, 300));
                this.getImage().Stretch = System.Windows.Media.Stretch.Uniform;
                break;
            case 1:
                startingBitmap = ImageLoader.loadImage("Wallpaper.png");
                this.setShape(new GameRectangle(1024, 1024));
                this.setZIndex(-1);
                break;
            case 2:
                startingBitmap = ImageLoader.loadImage("Blueprint.png");
                this.setShape(new GameRectangle(168, 72));
                break;
            case 3:
                startingBitmap = ImageLoader.loadImage("Farm.png");
                this.setShape(new GameRectangle(168, 72));
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
    }
    public override bool isAPainting()
    {
        return true;
    }

}
