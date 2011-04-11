using System.Windows.Media.Imaging;
using System.Collections.Generic;

class Player : Character
{
// public
    // constructor
	public Player(double[] location)
    {
	    BitmapImage startingImage = ImageLoader.loadImage("player1.png");
	    this.setCenter(location);
        double[] offset = new double[2]; offset[0] = 0; offset[1] = 0;
	    this.setImageOffset(offset);
	    this.setGravity(1000);
#if true
	    this.setShape(new GameRectangle(30, 43));
#else
	    this.setShape(new GameCircle(21));
#endif
        this.setBitmap(startingImage);
        double[] accel = new double[2]; accel[0] = 500; accel[1] = 0;
	    this.setMaxAccel(accel);
	    this.setDragCoefficient(3);
	    this.setTeamNum(2);
        this.initializeHitpoints(20);
        this.weaponTreeBranchFactor = 3;
        this.weaponSubTrees = new List<List<Weapon>>();
    }
    public void selectWeaponSubtreeAtIndex(int index)
    {
        // adjust the minimum and maximum indices
        if (lowWeaponIndex == highWeaponIndex)
        {
            this.gotoWeaponTreeRoot();
        }
        else
        {
            int countPerTree = (highWeaponIndex - lowWeaponIndex) / weaponTreeBranchFactor + 1;
            lowWeaponIndex = ((highWeaponIndex + 1) * index + lowWeaponIndex * (weaponTreeBranchFactor - index)) / weaponTreeBranchFactor;
            highWeaponIndex = lowWeaponIndex + countPerTree - 1;
            // If we've chosen exactly one weapon,
            if (lowWeaponIndex == highWeaponIndex)
            {
                // then switch to that weapon
                this.setWeaponIndex(lowWeaponIndex);
            }
            updateWeaponTrees();
        }
    }
    public void resetForLevel()
    {
        // recharge all the weapons
        int i, count;
        count = this.getNumWeapons();
        Weapon currentWeapon;
        for (i = 0; i < count; i++)
        {
            currentWeapon = this.getWeaponAtIndex(i);
            currentWeapon.resetCooldown();
            currentWeapon.refillAmmo();
        }
        // return to the starting location
        double[] position = new double[2];
        position[0] = 0;
        position[1] = this.getShape().getHeight() / 2;
        this.setCenter(position);
        this.setVelocity(new double[2]);
        this.setHitpoints(this.getMaxHitpoints());
        this.gotoWeaponTreeRoot();
        this.clearStuns();
    }
    // private
    void updateWeaponTrees()
    {
        this.weaponSubTrees.Clear();
        int i, j;
        int tempLowIndex, tempHighIndex;
        int countPerTree = (highWeaponIndex - lowWeaponIndex) / weaponTreeBranchFactor + 1;
        for (i = 0; i < this.weaponTreeBranchFactor; i++)
        {
            this.weaponSubTrees.Add(new List<Weapon>());
            if (highWeaponIndex >= 0)
            {
                tempLowIndex = ((highWeaponIndex + 1) * i + lowWeaponIndex * (weaponTreeBranchFactor - i)) / weaponTreeBranchFactor;
                //tempHighIndex = (highWeaponIndex * (i + 1) + lowWeaponIndex * (weaponTreeBranchFactor - (i + 1))) / weaponTreeBranchFactor;
                tempHighIndex = tempLowIndex + countPerTree - 1;
                for (j = tempLowIndex; j <= tempHighIndex; j++)
                {
                    this.weaponSubTrees[i].Add(this.getWeaponAtIndex(j));
                }
            }
        }
    }
    public override List<Weapon> getWeaponTreeAtIndex(int index)
    {
        return this.weaponSubTrees[index];
    }
    public override int getWeaponTreeBranchFactor()
    {
        return this.weaponTreeBranchFactor;
    }
    public void gotoWeaponTreeRoot()
    {
        this.lowWeaponIndex = 0;
        this.highWeaponIndex = this.getNumWeapons() - 1;
        this.updateWeaponTrees();
    }
    //int[] weaponTreeLowIndices;
    //int[] weaponTreeHighIndices;
    int lowWeaponIndex;
    int highWeaponIndex;
    List<List<Weapon> > weaponSubTrees;
    int weaponTreeBranchFactor;
    //List<Weapon> rightWeaponTree;
    
};