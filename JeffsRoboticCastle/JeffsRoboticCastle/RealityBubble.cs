using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// To make it a feasible task to process the world, only parts of the world exist at at time
// The RealityBubble tells which parts of the world exist at the moment
class RealityBubble
{
//public:
    // creates a reality bubble
    public RealityBubble(WorldBox newActiveRegion, WorldBox newExistentRegion)
    {
        this.activeRegion = newActiveRegion;
        this.existentRegion = newExistentRegion;
    }
    public RealityBubble(RealityBubble original)
    {
        this.activeRegion = new WorldBox(original.activeRegion);
        this.existentRegion = new WorldBox(original.existentRegion);
    }
    public WorldBox getActiveRegion()
    {
        return this.activeRegion;
    }
    public WorldBox getExistentRegion()
    {
        return this.existentRegion;
    }
    public void centerOn(WorldBox other)
    {
        int i;
        double activeCenter, existenceCenter, desiredCenter;
        for (i = 0; i < 2; i++)
        {
            desiredCenter = (other.getHighCoordinate(i) + other.getLowCoordinate(i)) / 2;
            activeCenter = (activeRegion.getHighCoordinate(i) + activeRegion.getLowCoordinate(i)) / 2;
            activeRegion.shift(i, desiredCenter - activeCenter);
            existenceCenter = (existentRegion.getHighCoordinate(i) + existentRegion.getLowCoordinate(i)) / 2;
            existentRegion.shift(i, desiredCenter - existenceCenter);
        }
    }
    public 
//private:
    WorldBox activeRegion;
    WorldBox existentRegion;
};