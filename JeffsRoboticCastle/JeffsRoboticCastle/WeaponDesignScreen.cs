using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

class WeaponDesignScreen : MenuScreen
{
// public
    public WeaponDesignScreen(Canvas c, double[] screenSize)
    {
        this.initialize(c, screenSize);
    }
    public void setPlayer(Player subject)
    {
        this.player = subject;
        this.statusDisplay.followCharacter(subject);
        this.currentMoney.Content = this.player.getMoney().ToString();
    }
    public override void initialize(Canvas c, double[] screenSize)
    {
        // setup drawing
        base.initialize(c, screenSize);
        this.setBackgroundBitmap(ImageLoader.loadImage("blueprint.png"));
        this.templateWeapon = new Weapon(1);
        this.addSubviews();
        this.fillInValues();
        this.calculateCost();
    }
    public override void KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
    }
    public override void KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
    }
    public override Screen  timerTick(double numSeconds)
    {
        this.statusDisplay.update();
        return base.timerTick(numSeconds);
    }


// private
    void addSubviews()
    {
        /*
        // add text boxes
        System.Windows.Controls.TextBox box1 = new TextBox();
        box1.RenderTransform = new TranslateTransform(100, 100);
        box1.Width = 100;
        box1.Height = 20;
        this.getCanvas().Children.Add(box1);
        // add a "done" button
        System.Windows.Controls.Button doneButton = new Button();
        doneButton.Click += new System.Windows.RoutedEventHandler(requestToExit);
        doneButton.RenderTransform = new TranslateTransform(400, 400);
        doneButton.Width = 100;
        doneButton.Height = 20;
        doneButton.Content = "Done";
        this.getCanvas().Children.Add(doneButton);
        */

        // add a status display to show the current weapons
        double[] statusDisplaySize = new double[2];
        statusDisplaySize[0] = this.getSize()[0];
        statusDisplaySize[1] = 200;
        statusDisplay = new CharacterStatusDisplay(this.getCanvas(), new double[2], statusDisplaySize);

        // add a bunch of controls for the creation of new weapons
        double labelWidth = 200;
        double labelHeight = 25;
        //double verticalPadding1 = 5;
        double verticalSpacing = labelHeight + 3;
        double horizontalSpacing = 100;
        
        // attributes of the weapon itself, rather than the projectiles or explosions
        Label weaponLabel = new Label();
        weaponLabel.Content = "Weapon";
        this.addControl(weaponLabel, 300, statusDisplaySize[1], labelWidth, labelHeight);

        Label weaponCostLabel = new Label();
        weaponCostLabel.Content = "Weapon Cost";
        addControl(weaponCostLabel, getLeft(weaponLabel), getBottom(weaponLabel) + 1, labelWidth, labelHeight);
        weaponCost = new Label();
        addControl(weaponCost, getLeft(weaponCostLabel), getBottom(weaponCostLabel) + 2, labelWidth, labelHeight);

        Label addOwnersVelocityLabel = new Label();
        addOwnersVelocityLabel.Content = "Add owner's velocity";
        addControl(addOwnersVelocityLabel, getLeft(weaponCostLabel), getBottom(weaponCostLabel) + verticalSpacing, labelWidth, labelHeight);
        addOwnersVelocity = new CheckBox();
        addControl(addOwnersVelocity, getLeft(weaponCost), getBottom(weaponCost) + verticalSpacing, labelWidth, labelHeight);

        Label warmupTimeLabel = new Label();
        warmupTimeLabel.Content = "Warmup (sec)";
        addControl(warmupTimeLabel, getLeft(weaponCostLabel), getBottom(addOwnersVelocityLabel) + verticalSpacing, labelWidth, labelHeight);
        warmupTime = new TextBox();
        addControl(warmupTime, getLeft(weaponCost), getBottom(addOwnersVelocity) + verticalSpacing, labelWidth, labelHeight);


        Label cooldownTimeLabel = new Label();
        cooldownTimeLabel.Content = "Cooldown (sec)";
        addControl(cooldownTimeLabel, getLeft(weaponCostLabel), getBottom(warmupTimeLabel) + verticalSpacing, labelWidth, labelHeight);
        cooldownTime = new TextBox();
        addControl(cooldownTime, getLeft(weaponCost), getBottom(warmupTime) + verticalSpacing, labelWidth, labelHeight);

        Label automaticLabel = new Label();
        automaticLabel.Content = "Automatic";
        addControl(automaticLabel, getLeft(weaponCostLabel), getBottom(cooldownTimeLabel) + verticalSpacing, labelWidth, labelHeight);
        automatic = new CheckBox();
        addControl(automatic, getLeft(weaponCost), getBottom(cooldownTime) + verticalSpacing, labelWidth, labelHeight);

        Label switchToTimeLabel = new Label();
        switchToTimeLabel.Content = "Switch-to-Time";
        addControl(switchToTimeLabel, getLeft(weaponCostLabel), getBottom(automaticLabel) + verticalSpacing, labelWidth, labelHeight);
        switchToTime = new TextBox();
        addControl(switchToTime, getLeft(weaponCost), getBottom(automatic) + verticalSpacing, labelWidth, labelHeight);

        Label switchFromTimeLabel = new Label();
        switchFromTimeLabel.Content = "Switch-from-Time";
        addControl(switchFromTimeLabel, getLeft(weaponCostLabel), getBottom(switchToTimeLabel) + verticalSpacing, labelWidth, labelHeight);
        switchFromTime = new TextBox();
        addControl(switchFromTime, getLeft(weaponCost), getBottom(switchToTime) + verticalSpacing, labelWidth, labelHeight);

        Label maxAmmoLabel = new Label();
        maxAmmoLabel.Content = "Max Ammo";
        addControl(maxAmmoLabel, getLeft(weaponCostLabel), getBottom(switchFromTimeLabel) + verticalSpacing, labelWidth, labelHeight);
        maxAmmo = new TextBox();
        addControl(maxAmmo, getLeft(weaponCost), getBottom(switchFromTime) + verticalSpacing, labelWidth, labelHeight);

        Label ammoRechargeRateLabel = new Label();
        ammoRechargeRateLabel.Content = "Ammo Recharge/sec";
        addControl(ammoRechargeRateLabel, getLeft(weaponCostLabel), getBottom(maxAmmoLabel) + verticalSpacing, labelWidth, labelHeight);
        ammoRechargeRate = new TextBox();
        addControl(ammoRechargeRate, getLeft(weaponCost), getBottom(maxAmmo) + verticalSpacing, labelWidth, labelHeight);

        Label fireWhileInactiveLabel = new Label();
        fireWhileInactiveLabel.Content = "Fire while Inactive";
        addControl(fireWhileInactiveLabel, getLeft(weaponCostLabel), getBottom(ammoRechargeRateLabel) + verticalSpacing, labelWidth, labelHeight);
        fireWhileInactive = new CheckBox();
        addControl(fireWhileInactive, getLeft(weaponCost), getBottom(ammoRechargeRate) + verticalSpacing, labelWidth, labelHeight);

        Label cooldownWhileInactiveLabel = new Label();
        cooldownWhileInactiveLabel.Content = "Cooldown while Inactive";
        addControl(cooldownWhileInactiveLabel, getLeft(weaponCostLabel), getBottom(fireWhileInactiveLabel) + verticalSpacing, labelWidth, labelHeight);
        cooldownWhileInactive = new CheckBox();
        addControl(cooldownWhileInactive, getLeft(weaponCost), getBottom(fireWhileInactive) + verticalSpacing, labelWidth, labelHeight);

        Label rechargeWhileInactiveLabel = new Label();
        rechargeWhileInactiveLabel.Content = "Recharge while Inactive";
        addControl(rechargeWhileInactiveLabel, getLeft(weaponCostLabel), getBottom(cooldownWhileInactiveLabel) + verticalSpacing, labelWidth, labelHeight);
        rechargeWhileInactive = new CheckBox();
        addControl(rechargeWhileInactive, getLeft(weaponCost), getBottom(cooldownWhileInactive) + verticalSpacing, labelWidth, labelHeight);


        /*double cost;
        CheckBox addOwnersVelocity;
        TextBox warmupTime;
        TextBox cooldownTime;
        CheckBox automatic;
        TextBox switchToTime;
        TextBox switchFromTime;
        TextBox maxAmmo;
        TextBox ammoRechargeRate;
        CheckBox fireWhileInactive; // Tells whether this weapon can fire without being the current weapon
        CheckBox cooldownWhileInactive; // Tells whether this weapon can cooldown without being the current weapon
        CheckBox rechargeWhileInactive; // Tells whether this weapon can recharge ammo without being the current weapon
        */

        Label projectileLabel = new Label();
        projectileLabel.Content = "Projectile";
        addControl(projectileLabel, getRight(weaponLabel) + horizontalSpacing, getTop(weaponLabel), labelWidth, labelHeight);

        Label projectileImageLabel = new Label();
        projectileImageLabel.Content = "Image Name";
        addControl(projectileImageLabel, getLeft(projectileLabel), getTop(weaponCostLabel), labelWidth, labelHeight);
        projectileImage = new TextBox();
        addControl(projectileImage, getLeft(projectileLabel), getTop(weaponCost), labelWidth, labelHeight);
        
        Label projectileRadiusLabel = new Label();
        projectileRadiusLabel.Content = "Radius";
        addControl(projectileRadiusLabel, getLeft(projectileImageLabel), getBottom(projectileImageLabel) + verticalSpacing, labelWidth, labelHeight);
        projectileRadius = new TextBox();
        addControl(projectileRadius, getLeft(projectileImage), getBottom(projectileImage) + verticalSpacing, labelWidth, labelHeight);

        Label projectileXLabel = new Label();
        projectileXLabel.Content = "Offset (X)";
        addControl(projectileXLabel, getLeft(projectileRadiusLabel), getBottom(projectileRadiusLabel) + verticalSpacing, labelWidth, labelHeight);
        projectileX = new TextBox();
        addControl(projectileX, getLeft(projectileRadius), getBottom(projectileRadius) + verticalSpacing, labelWidth, labelHeight);

        Label projectileYLabel = new Label();
        projectileYLabel.Content = "Offset (Y)";
        addControl(projectileYLabel, getLeft(projectileXLabel), getBottom(projectileXLabel) + verticalSpacing, labelWidth, labelHeight);
        projectileY = new TextBox();
        addControl(projectileY, getLeft(projectileX), getBottom(projectileX) + verticalSpacing, labelWidth, labelHeight);

        Label projectileVXLabel = new Label();
        projectileVXLabel.Content = "Velocity (X)";
        addControl(projectileVXLabel, getLeft(projectileYLabel), getBottom(projectileYLabel) + verticalSpacing, labelWidth, labelHeight);
        projectileVX = new TextBox();
        addControl(projectileVX, getLeft(projectileY), getBottom(projectileY) + verticalSpacing, labelWidth, labelHeight);

        Label projectileVYLabel = new Label();
        projectileVYLabel.Content = "Velocity (Y)";
        addControl(projectileVYLabel, getLeft(projectileVXLabel), getBottom(projectileVXLabel) + verticalSpacing, labelWidth, labelHeight);
        projectileVY = new TextBox();
        addControl(projectileVY, getLeft(projectileVX), getBottom(projectileVX) + verticalSpacing, labelWidth, labelHeight);

        Label projectileDragLabel = new Label();
        projectileDragLabel.Content = "Drag Coefficient";
        addControl(projectileDragLabel, getLeft(projectileVYLabel), getBottom(projectileVYLabel) + verticalSpacing, labelWidth, labelHeight);
        projectileDrag = new TextBox();
        addControl(projectileDrag, getLeft(projectileVY), getBottom(projectileVY) + verticalSpacing, labelWidth, labelHeight);

        Label remainingFlightTimeLabel = new Label();
        remainingFlightTimeLabel.Content = "Max Flight time";
        addControl(remainingFlightTimeLabel, getLeft(projectileDragLabel), getBottom(projectileDragLabel) + verticalSpacing, labelWidth, labelHeight);
        remainingFlightTime = new TextBox();
        addControl(remainingFlightTime, getLeft(projectileDrag), getBottom(projectileDrag) + verticalSpacing, labelWidth, labelHeight);

        Label numExplosionsRemainingLabel = new Label();
        numExplosionsRemainingLabel.Content = "Max # Explosions";
        addControl(numExplosionsRemainingLabel, getLeft(remainingFlightTimeLabel), getBottom(remainingFlightTimeLabel) + verticalSpacing, labelWidth, labelHeight);
        numExplosionsRemaining = new TextBox();
        addControl(numExplosionsRemaining, getLeft(remainingFlightTime), getBottom(remainingFlightTime) + verticalSpacing, labelWidth, labelHeight);

        Label penetrationLabel = new Label();
        penetrationLabel.Content = "Penetration fraction";
        addControl(penetrationLabel, getLeft(remainingFlightTimeLabel), getBottom(numExplosionsRemainingLabel) + verticalSpacing, labelWidth, labelHeight);
        penetration = new TextBox();
        addControl(penetration, getLeft(remainingFlightTime), getBottom(numExplosionsRemaining) + verticalSpacing, labelWidth, labelHeight);

        Label homingAccelLabel = new Label();
        homingAccelLabel.Content = "Homing Accel";
        addControl(homingAccelLabel, getLeft(remainingFlightTimeLabel), getBottom(penetrationLabel) + verticalSpacing, labelWidth, labelHeight);
        homingAccel = new TextBox();
        addControl(homingAccel, getLeft(remainingFlightTime), getBottom(penetration) + verticalSpacing, labelWidth, labelHeight);

        Label boomerangAccelLabel = new Label();
        boomerangAccelLabel.Content = "Boomerang Accel";
        addControl(boomerangAccelLabel, getLeft(remainingFlightTimeLabel), getBottom(homingAccelLabel) + verticalSpacing, labelWidth, labelHeight);
        boomerangAccel = new TextBox();
        addControl(boomerangAccel, getLeft(remainingFlightTime), getBottom(homingAccel) + verticalSpacing, labelWidth, labelHeight);

        Label homeOnProjectilesLabel = new Label();
        homeOnProjectilesLabel.Content = "Home on Projectiles";
        addControl(homeOnProjectilesLabel, getLeft(remainingFlightTimeLabel), getBottom(boomerangAccelLabel) + verticalSpacing, labelWidth, labelHeight);
        homeOnProjectiles = new CheckBox();
        addControl(homeOnProjectiles, getLeft(remainingFlightTime), getBottom(boomerangAccel) + verticalSpacing, labelWidth, labelHeight);

        Label homeOnPlayersLabel = new Label();
        homeOnPlayersLabel.Content = "Home on Players";
        addControl(homeOnPlayersLabel, getLeft(remainingFlightTimeLabel), getBottom(homeOnProjectilesLabel) + verticalSpacing, labelWidth, labelHeight);
        homeOnPlayers = new CheckBox();
        addControl(homeOnPlayers, getLeft(remainingFlightTime), getBottom(homeOnProjectiles) + verticalSpacing, labelWidth, labelHeight);



        /*
        // attributes of the projectile
        TextBox remainingFlightTime;
        TextBox numExplosionsRemaining; // how many more times it can explode before being removed
        TextBox penetration;
        TextBox homingAccel;
        TextBox boomerangAccel;
        CheckBox homeOnProjectiles;
        CheckBox homeOnPlayers;
        */


        Label explosionLabel = new Label();
        explosionLabel.Content = "Explosion";
        addControl(explosionLabel, getRight(projectileLabel) + horizontalSpacing, getTop(projectileLabel), labelWidth, labelHeight);

        Label explosionImageLabel = new Label();
        explosionImageLabel.Content = "Image Name";
        addControl(explosionImageLabel, getRight(projectileImageLabel) + horizontalSpacing, getTop(projectileImageLabel), labelWidth, labelHeight);
        explosionImage = new TextBox();
        addControl(explosionImage, getRight(projectileImage) + horizontalSpacing, getTop(projectileImage), labelWidth, labelHeight);

        Label explosionRadiusLabel = new Label();
        explosionRadiusLabel.Content = "Radius";
        addControl(explosionRadiusLabel, getLeft(explosionImageLabel), getBottom(explosionImageLabel) + verticalSpacing, labelWidth, labelHeight);
        explosionRadius = new TextBox();
        addControl(explosionRadius, getLeft(explosionImage), getBottom(explosionImage) + verticalSpacing, labelWidth, labelHeight);

        Label explosionDurationLabel = new Label();
        explosionDurationLabel.Content = "Duration";
        addControl(explosionDurationLabel, getLeft(explosionImageLabel), getBottom(explosionRadiusLabel) + verticalSpacing, labelWidth, labelHeight);
        explosionDuration = new TextBox();
        addControl(explosionDuration, getLeft(explosionImage), getBottom(explosionRadius) + verticalSpacing, labelWidth, labelHeight);

        Label friendlyFireLabel = new Label();
        friendlyFireLabel.Content = "Friendly Fire";
        addControl(friendlyFireLabel, getLeft(explosionImageLabel), getBottom(explosionDurationLabel) + verticalSpacing, labelWidth, labelHeight);
        friendlyFire = new CheckBox();
        addControl(friendlyFire, getLeft(explosionImage), getBottom(explosionDuration) + verticalSpacing, labelWidth, labelHeight);


        /*
        // attributes of the explosion
        TextBox explosionDuration;
        CheckBox friendlyFire;
        */


        Label stunLabel = new Label();
        stunLabel.Content = "Stun";
        addControl(stunLabel, getRight(explosionLabel) + horizontalSpacing, getTop(explosionLabel), labelWidth, labelHeight);

        Label damagePerSecondLabel = new Label();
        damagePerSecondLabel.Content = "Damage per Second";
        addControl(damagePerSecondLabel, getLeft(stunLabel), getTop(explosionImageLabel), labelWidth, labelHeight);
        damagePerSecond = new TextBox();
        addControl(damagePerSecond, getLeft(stunLabel), getTop(explosionImage), labelWidth, labelHeight);

        Label timeMultiplierLabel = new Label();
        timeMultiplierLabel.Content = "Time Multiplier";
        addControl(timeMultiplierLabel, getLeft(damagePerSecondLabel), getBottom(damagePerSecondLabel) + verticalSpacing, labelWidth, labelHeight);
        timeMultiplier = new TextBox();
        addControl(timeMultiplier, getLeft(damagePerSecond), getBottom(damagePerSecond) + verticalSpacing, labelWidth, labelHeight);

        Label stunDurationLabel = new Label();
        stunDurationLabel.Content = "Duration";
        addControl(stunDurationLabel, getLeft(damagePerSecondLabel), getBottom(timeMultiplierLabel) + verticalSpacing, labelWidth, labelHeight);
        stunDuration = new TextBox();
        addControl(stunDuration, getLeft(damagePerSecond), getBottom(timeMultiplier) + verticalSpacing, labelWidth, labelHeight);




        /*
        // attributes of the stun
        TextBox timeMultiplier;
        TextBox damagePerSecond;
        TextBox duration;*/

        Button calculateButton = new Button();
        calculateButton.Click += new RoutedEventHandler(calculateCost);
        calculateButton.Content = "Calculate Cost";
        this.addControl(calculateButton, 900, 600, labelWidth, labelHeight);

        Button purchaseButton = new Button();
        purchaseButton.Click += new RoutedEventHandler(purchaseCurrentWeapon);
        purchaseButton.Content = "Purchase";
        this.addControl(purchaseButton, 900, 850, labelWidth, labelHeight);

        Button doneButton = new Button();
        doneButton.Click += new RoutedEventHandler(requestToExit);
        doneButton.Content = "Done";
        this.addControl(doneButton, 900, 900, labelWidth, labelHeight);

        Label currentMoneyLabel = new Label();
        currentMoneyLabel.Content = "Current Money";
        addControl(currentMoneyLabel, 50, 200, labelWidth, labelHeight);
        currentMoney = new Label();
        addControl(currentMoney, getLeft(currentMoneyLabel), getBottom(currentMoneyLabel), labelWidth, labelHeight);
    }
    // put some reasonable guesses into the text boxes based on what's already present
    void fillInValues()
    {
        if (maxAmmo.Text == "")
            maxAmmo.Text = "1";
        if (ammoRechargeRate.Text == "")
            ammoRechargeRate.Text = "0";
        if (warmupTime.Text == "")
            warmupTime.Text = "1";
        if (cooldownTime.Text == "")
            cooldownTime.Text = "1";
        if (switchToTime.Text == "")
            switchToTime.Text = "1";
        if (switchFromTime.Text == "")
            switchFromTime.Text = "1";


        // attributes of the projectile
        if (projectileImage.Text == "")
            projectileImage.Text = "fan.png";
        if (projectileRadius.Text == "")
            projectileRadius.Text = "100";
        if (projectileX.Text == "")
            projectileX.Text = "0";
        if (projectileY.Text == "")
            projectileY.Text = "0";
        if (projectileVX.Text == "")
            projectileVX.Text = "100";
        if (projectileVY.Text == "")
            projectileVY.Text = "0";
        if (projectileDrag.Text == "")
            projectileDrag.Text = "0";
        if (remainingFlightTime.Text == "")
            remainingFlightTime.Text = "10";
        if (numExplosionsRemaining.Text == "")
            numExplosionsRemaining.Text = "1";
        if (penetration.Text == "")
            penetration.Text = "0";
        if (homingAccel.Text == "")
            homingAccel.Text = "0";
        if (boomerangAccel.Text == "")
            boomerangAccel.Text = "0";

        // attributes of the explosion
        if (explosionImage.Text == "")
            explosionImage.Text = "fan.png";
        if (explosionRadius.Text == "")
            explosionRadius.Text = "100";
        if (explosionDuration.Text == "")
            explosionDuration.Text = "1";

        // attributes of the stun
        if (timeMultiplier.Text == "")
            timeMultiplier.Text = "1";
        if (damagePerSecond.Text == "")
            damagePerSecond.Text = "1";
        if (stunDuration.Text == "")
            stunDuration.Text = "0";
    }
    void calculateCost(object sender, EventArgs e)
    {
        calculateCost();
    }
    void calculateCost()
    {
        this.updateCurrentWeapon();
        this.weaponCost.Content = this.templateWeapon.calculateCost().ToString();
    }
    void purchaseCurrentWeapon(object sender, EventArgs e)
    {
        this.calculateCost();
        if (this.player.spendMoney(this.templateWeapon.getCost()))
        {
            this.player.addWeapon(templateWeapon);
            this.player.gotoWeaponTreeRoot();
        }
        // update the user's money
        this.currentMoney.Content = this.player.getMoney().ToString();
    }
    void updateCurrentWeapon()
    {
        templateWeapon = new Weapon();
        // attributes of the weapon
        templateWeapon.setMaxAmmo(parseDouble(maxAmmo));
        templateWeapon.refillAmmo();
        templateWeapon.setAmmoRechargeRate(parseDouble(ammoRechargeRate));
        templateWeapon.setWarmupTime(parseDouble(warmupTime));
        templateWeapon.setCooldownTime(parseDouble(cooldownTime));
        templateWeapon.setSwitchToTime(parseDouble(switchToTime));
        templateWeapon.setSwitchFromTime(parseDouble(switchFromTime));
        templateWeapon.setAutomatic(automatic.IsChecked.Value);
        templateWeapon.startWithOwnersVelocity(addOwnersVelocity.IsChecked.Value);
        templateWeapon.enableFiringWhileInactive(fireWhileInactive.IsChecked.Value);
        templateWeapon.enableCooldownWhileInactive(cooldownWhileInactive.IsChecked.Value);
        templateWeapon.enableRechargeWhileInactive(rechargeWhileInactive.IsChecked.Value);

        Projectile templateProjectile = new Projectile();
        double[] tempVector = new double[2];
        tempVector[0] = parseDouble(projectileX);
        tempVector[1] = parseDouble(projectileY);
        templateProjectile.setCenter(tempVector);
        tempVector[0] = parseDouble(projectileVX);
        tempVector[1] = parseDouble(projectileVY);
        templateProjectile.setVelocity(tempVector);
        templateProjectile.setDragCoefficient(parseDouble(projectileDrag));
        templateProjectile.setBitmap(ImageLoader.loadImage(projectileImage.Text));
        templateProjectile.setShape(new GameCircle(parseDouble(projectileRadius)));
        templateProjectile.setRemainingFlightTime(parseDouble(remainingFlightTime));
        templateProjectile.setNumExplosionsRemaining(parseInt(numExplosionsRemaining));
        templateProjectile.setPenetration(parseDouble(penetration));
        templateProjectile.setHomingAccel(parseDouble(homingAccel));
        templateProjectile.setBoomerangAccel(parseDouble(boomerangAccel));
        templateProjectile.enableHomingOnProjectiles(homeOnProjectiles.IsChecked.Value);
        templateProjectile.enableHomingOnCharacters(homeOnPlayers.IsChecked.Value);
        templateWeapon.setTemplateProjectile(templateProjectile);

        Explosion templateExplosion = new Explosion();
        templateExplosion.setBitmap(ImageLoader.loadImage(explosionImage.Text));
        templateExplosion.setShape(new GameCircle(parseDouble(explosionRadius)));
        templateExplosion.setDuration(parseDouble(explosionDuration));
        templateExplosion.setFriendlyFireEnabled(friendlyFire.IsChecked.Value);
        templateProjectile.setTemplateExplosion(templateExplosion);

        Stun templateStun = new Stun();
        templateStun.setTimeMultiplier(parseDouble(timeMultiplier));
        templateStun.setDamagePerSecond(parseDouble(damagePerSecond));
        templateStun.setDuration(parseDouble(stunDuration));
        templateExplosion.setTemplateStun(templateStun);
    }
    double parseDouble(TextBox field)
    {
        if (field.Text == "")
            field.Text = "0";
        return Double.Parse(field.Text);
    }
    int parseInt(TextBox field)
    {
        if (field.Text == "")
            field.Text = "0";
        return Int32.Parse(field.Text);
    }
    Player player;
    Label currentMoney;
    CharacterStatusDisplay statusDisplay;
    // attributes of the weapon itself, rather than the projectiles or explosions
    Label weaponCost;
    TextBox maxAmmo;
    TextBox ammoRechargeRate;
    TextBox warmupTime;
    TextBox cooldownTime;
    TextBox switchToTime;
    TextBox switchFromTime;
    CheckBox automatic;
    CheckBox addOwnersVelocity;
    CheckBox fireWhileInactive; // Tells whether this weapon can fire without being the current weapon
    CheckBox cooldownWhileInactive; // Tells whether this weapon can cooldown without being the current weapon
    CheckBox rechargeWhileInactive; // Tells whether this weapon can recharge ammo without being the current weapon


    // attributes of the projectile
    TextBox projectileImage;
    TextBox projectileRadius;
    TextBox projectileX;
    TextBox projectileY;
    TextBox projectileVX;
    TextBox projectileVY;
    TextBox projectileDrag;
    TextBox remainingFlightTime;
    TextBox numExplosionsRemaining; // how many more times it can explode before being removed
    TextBox penetration;
    TextBox homingAccel;
    TextBox boomerangAccel;
    CheckBox homeOnProjectiles;
    CheckBox homeOnPlayers;

    // attributes of the explosion
    TextBox explosionImage;
    TextBox explosionRadius;
    TextBox explosionDuration;
    CheckBox friendlyFire;

    // attributes of the stun
    TextBox timeMultiplier;
    TextBox damagePerSecond;
    TextBox stunDuration;

    Weapon templateWeapon;
}