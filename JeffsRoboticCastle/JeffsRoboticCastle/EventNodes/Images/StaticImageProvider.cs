using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// loads a specific image
namespace Castle.EventNodes.Menus
{
    class StaticImageProvider : ValueConverter<Size, System.Windows.Controls.Image>
    {
        public StaticImageProvider(String fileName)
        {
            this.FileName = fileName;
        }
        public String FileName;

        public System.Windows.Controls.Image convert(Size size)
        {
            if (this.FileName == null)
            {
                return null;
            }
            return ImageLoader.loadImage(this.FileName, size);
        }
    }
}
