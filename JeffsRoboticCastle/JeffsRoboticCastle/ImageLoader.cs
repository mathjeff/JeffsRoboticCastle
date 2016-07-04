using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;
using System.Windows;


public class ImageLoader
{
    public static BitmapImage loadImage(System.String fileName)
    {
        String testString = fileName;
        if (System.IO.File.Exists(testString))
        {
            fileName = testString;
        }
        else
        {
            fileName = "../../../Images/" + fileName;
        }
        if (!System.IO.File.Exists(fileName))
        {
            return null;
        }
        else
        {
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

    public static Image loadImage(String fileName, Size size)
    {
        BitmapImage bitmap = loadImage(fileName);

        Image newImage = new Image();
        newImage.Source = bitmap;
        newImage.Stretch = System.Windows.Media.Stretch.Fill;
        newImage.Width = size.Width;
        newImage.Height = size.Height;
        return newImage;
    }
}