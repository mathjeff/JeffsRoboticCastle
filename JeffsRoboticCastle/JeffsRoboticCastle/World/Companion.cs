using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Companion : Character
{
    public void setFriendToFollow(Character friend)
    {
        this.friendToFollow = friend;
    }
    public override void updateVelocity(double numSeconds)
    {
        if (friendToFollow != null)
        {
            // Occasionally, this companion teleports closer to the friend that it is following

        }

        base.updateVelocity(numSeconds);
    }
    // private
    Character friendToFollow;

}