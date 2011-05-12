using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CollisionRequest
{
    public CollisionRequest()
    {
    }
    // the collision shape
    public void setObject(GameObject o)
    {
        this.movingObject = o;
    }
    public GameObject getObject()
    {
        return this.movingObject;
    }
    public void growToTouch(GameObject other)
    {
        if (other != null)
        {
            // create a new collision box, centered on the current object
            GameObject collisionBox = new GameObject();
            collisionBox.setCenter(this.movingObject.getCenter());
            collisionBox.setShape(new GameRectangle(0, 0));
            collisionBox.setTeamNum(this.movingObject.getTeamNum());
            double dist = collisionBox.distanceTo(other);
            // Increase the radius appropriately
            collisionBox.setShape(new GameRectangle(this.movingObject.getShape().getWidth() + 2 * dist, this.movingObject.getShape().getHeight() + 2 * dist));
            collisionBox.setTeamNum(this.movingObject.getTeamNum());
            // use the new collision box instead
            this.setObject(collisionBox);
            this.hasAHint = true;
        }
    }
    public bool hasASizeHint()
    {
        return this.hasAHint;
    }
    // object alignments
    public void setRequestAllies(bool value)
    {
        this.requestsAllies = value;
    }
    public bool shouldRequestAllies()
    {
        return this.requestsAllies;
    }
    public void setRequestGaia(bool value)
    {
        this.requestsGaia = value;
    }
    public bool shouldRequestGaia()
    {
        return this.requestsGaia;
    }
    public void setRequestEnemies(bool value)
    {
        this.requestsEnemies = value;
    }
    public bool shouldRequestEnemies()
    {
        return this.requestsEnemies;
    }

    // move
    public void setCollisionMove(double[] move)
    {
        this.collisionMove = move;
    }
    public double[] getCollisionMove()
    {
        return this.collisionMove;
    }

    // object types
    public void setRequestCharacters(bool value)
    {
        this.requestsCharacters = value;
    }
    public bool shouldRequestCharacters()
    {
        return this.requestsCharacters;
    }
    public void setRequestTerrain(bool value)
    {
        this.requestsTerrain = value;
    }
    public bool shouldRequestTerrain()
    {
        return this.requestsTerrain;
    }
    public void setRequestProjectiles(bool value)
    {
        this.requestsProjectiles = value;
    }
    public bool shouldRequestProjectiles()
    {
        return this.requestsProjectiles;
    }
    public void setRequestExplosions(bool value)
    {
        this.requestsExplosions = value;
    }
    public bool shouldRequestExplosions()
    {
        return this.requestsExplosions;
    }
    public void setRequestGhosts(bool value)
    {
        this.requestsGhosts = value;
    }
    public bool shouldRequestGhosts()
    {
        return this.requestsGhosts;
    }
    // private
    // the object in question
    GameObject movingObject;
    // object alignments
    bool requestsGaia;
    bool requestsAllies;
    bool requestsEnemies;
    // move
    double[] collisionMove;
    // object types
    bool requestsCharacters;
    bool requestsTerrain;
    bool requestsProjectiles;
    bool requestsExplosions;
    bool requestsGhosts;
    bool hasAHint;

}