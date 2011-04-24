using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// the Ghost class represents a GameObject that does not have physics collision enabled
class Ghost : GameObject
{
    public Ghost()
    {
    }
    public Ghost(bool enableRendering)
    {
        this.setRenderable(enableRendering);
    }
    public override bool isAGhost()
    {
        return true;
    }
}
