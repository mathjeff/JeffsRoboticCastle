using System.Windows.Media.Imaging;
using System;


public class ImageLoader
{
    public static BitmapImage loadImage(System.String fileName)
    {
        fileName = "../../../Images/" + fileName;
        BitmapImage image = new BitmapImage();
        image.BeginInit();
        image.UriSource = new Uri(fileName, UriKind.Relative);
        image.CacheOption = BitmapCacheOption.OnLoad;
        //image.Width = image.PixelWidth;
        //image.Height = image.PixelHeight;
        image.EndInit();
        return image;
    }
}
