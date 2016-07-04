using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PickupItem : Ghost
{
// public
    public PickupItem(double[] location)
    {
        this.setBitmap(ImageLoader.loadImage("AmmoBox2.png"));
        this.setShape(new GameRectangle(50, 32));
        this.setCenter(location);
    }
    public override bool isAPickupItem()
    {
        return true;
    }
// private

}