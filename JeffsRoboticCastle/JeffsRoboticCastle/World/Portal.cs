using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Portal : Ghost
{
    public Portal()
    {
        this.setBitmap(ImageLoader.loadImage("Portal.png"));
        this.setShape(new GameCircle(50));
    }
    public override bool isAPortal()
    {
        return true;
    }
}