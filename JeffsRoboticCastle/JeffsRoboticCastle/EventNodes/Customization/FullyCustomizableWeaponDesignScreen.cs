﻿using Castle.EventNodes.World;
using Castle.WeaponDesign;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// This screen lets the user type in the Stats of the weapon they want, and tries to calculate a good cost to charge for it
// This screen is unused now because users actually don't like just typing in their weapon Stats
// Users prefer to assemble their weapons out of discrete components instead
#if true
namespace Castle.EventNodes.Customization
{
    class FullyCustomizableWeaponDesignScreen : Screen
    {
        // public
        public FullyCustomizableWeaponDesignScreen(Size screenSize, GamePlayer player)
        {
            // setup drawing
            base.Initialize(screenSize);

            this.player = player;

            this.setBackgroundImage(ImageLoader.loadImage("blueprint.png", screenSize));
            this.addSubviews();
            this.setupWeaponFactory();
            this.quickBuildWeapon();
        }
        public void setupWeaponFactory()
        {
            this.weaponFactory = new WeaponFactory();
            this.weaponFactory.addDefaultWeapons();
        }
        public void TimerTick(double duration)
        {
            if (this.demoWorld != null)
            {
                this.demoWorld.TimerTick(duration);
                if (this.demoWorld.isPlayerSuccessful())
                {
                    this.demoWorld = null;
                }
            }

        }
        public void update()
        {
            // update the weapon's cost if needed
            this.calculateCost();
            // update the display of purchased weapons if needed
            this.statusDisplay.update();
            //this.currentMoney.Content = this.player.getMoney().ToString();
        }


        public WorldEventNode MakeDemoNode()
        {
            // make the world
            WorldLoader worldLoader = new WorldLoader(this.getSize(), this.getSize(), true);

            this.updateCurrentWeapon();
            BasicWeapon basicWeapon = new BasicWeapon(this.templateWeapon.Stats);
            WeaponStats weaponStats = basicWeapon.WithAugments(new List<WeaponAugmentTemplate>());
            List<Weapon> demoWeapons = new List<Weapon>() { new Weapon(weaponStats) };

            LevelPlayer levelPlayer = new LevelPlayer(this.player, new double[] { 100, 30 }, demoWeapons);
            WorldLoader demoWorld = worldLoader;

            // put the player in the world
            demoWorld.addItemAndDisableUnloading(levelPlayer);
            demoWorld.addItem(Enemy.NewEnemy(new double[] { 400, 30 }, 0));

            // don't need another demo
            this.WantsDemo = false;

            WorldEventNode eventNode = new WorldEventNode(worldLoader, levelPlayer, this.getSize());
            // Allow the user to press Escape to exit
            eventNode.EscapeEnabled = true;
            return eventNode;
        }
        // creates and adds a lot of text boxes to type into
        void addSubviews()
        {
            // add a bunch of controls for the creation of new weapons
            double labelWidth = this.getSize().Width / 8;
            double labelHeight = this.getSize().Height / 35;
            //double verticalPadding1 = 5;
            double verticalSpacing = labelHeight / 8;
            double horizontalSpacing = labelWidth / 2;

            // attributes of the weapon itself, rather than the projectiles or explosions
            Label weaponLabel = new Label();
            weaponLabel.Content = "Weapon";
            this.addControl(weaponLabel, this.getSize().Width * 3 / 16, verticalSpacing, labelWidth, labelHeight);

            /*
            Label weaponCostLabel = new Label();
            weaponCostLabel.Content = "Weapon Cost";
            addControl(weaponCostLabel, getLeft(weaponLabel), getBottom(weaponLabel) + 1, labelWidth, labelHeight);
            weaponCost = new Label();
            addControl(weaponCost, getLeft(weaponCostLabel), getBottom(weaponCostLabel), labelWidth, labelHeight);
            */

            Label ownersVelocityScaleLabel = new Label();
            ownersVelocityScaleLabel.Content = "Scale for owner's velocity";
            addControl(ownersVelocityScaleLabel, getLeft(weaponLabel), getBottom(weaponLabel) + verticalSpacing, labelWidth, labelHeight);
            ownersVelocityScale = new TextBox();
            addControl(ownersVelocityScale, getLeft(ownersVelocityScaleLabel), getBottom(ownersVelocityScaleLabel) + verticalSpacing, labelWidth, labelHeight);

            Label warmupTimeLabel = new Label();
            warmupTimeLabel.Content = "Warmup (sec)";
            addControl(warmupTimeLabel, getLeft(ownersVelocityScale), getBottom(ownersVelocityScale) + verticalSpacing, labelWidth, labelHeight);
            warmupTime = new TextBox();
            addControl(warmupTime, getLeft(warmupTimeLabel), getBottom(warmupTimeLabel) + verticalSpacing, labelWidth, labelHeight);


            Label cooldownTimeLabel = new Label();
            cooldownTimeLabel.Content = "Cooldown (sec)";
            addControl(cooldownTimeLabel, getLeft(warmupTime), getBottom(warmupTime) + verticalSpacing, labelWidth, labelHeight);
            cooldownTime = new TextBox();
            addControl(cooldownTime, getLeft(cooldownTimeLabel), getBottom(cooldownTimeLabel) + verticalSpacing, labelWidth, labelHeight);

            /*
            Label automaticLabel = new Label();
            automaticLabel.Content = "Automatic";
            addControl(automaticLabel, getLeft(weaponCostLabel), getBottom(cooldownTimeLabel) + verticalSpacing, labelWidth, labelHeight);
            automatic = new CheckBox();
            addControl(automatic, getLeft(weaponCost), getBottom(cooldownTime) + verticalSpacing, labelWidth, labelHeight);

            Label stickyTriggerLabel = new Label();
            stickyTriggerLabel.Content = "Sticky Trigger";
            addControl(stickyTriggerLabel, getLeft(weaponCostLabel), getBottom(automaticLabel) + verticalSpacing, labelWidth, labelHeight);
            stickyTrigger = new CheckBox();
            addControl(stickyTrigger, getLeft(weaponCost), getBottom(automatic) + verticalSpacing, labelWidth, labelHeight);
            */

#if SWITCH_TIMES
        Label switchToTimeLabel = new Label();
        switchToTimeLabel.Content = "Switch-to-Time";
        switchToTime = new TextBox();

        Label switchFromTimeLabel = new Label();
        switchFromTimeLabel.Content = "Switch-from-Time";
        switchFromTime = new TextBox();

        addControl(switchToTimeLabel, getLeft(weaponCostLabel), getBottom(automaticLabel) + verticalSpacing, labelWidth, labelHeight);
        addControl(switchToTime, getLeft(weaponCost), getBottom(automatic) + verticalSpacing, labelWidth, labelHeight);
        addControl(switchFromTimeLabel, getLeft(weaponCostLabel), getBottom(switchToTimeLabel) + verticalSpacing, labelWidth, labelHeight);
        addControl(switchFromTime, getLeft(weaponCost), getBottom(switchToTime) + verticalSpacing, labelWidth, labelHeight);
#endif
            Label maxAmmoLabel = new Label();
            maxAmmoLabel.Content = "Max Ammo";
            maxAmmo = new TextBox();
#if SWITCH_TIMES
        addControl(maxAmmoLabel, getLeft(weaponCostLabel), getBottom(switchFromTimeLabel) + verticalSpacing, labelWidth, labelHeight);
        addControl(maxAmmo, getLeft(weaponCost), getBottom(switchFromTime) + verticalSpacing, labelWidth, labelHeight);
#else
            addControl(maxAmmoLabel, getLeft(cooldownTime), getBottom(cooldownTime) + verticalSpacing, labelWidth, labelHeight);
            addControl(maxAmmo, getLeft(maxAmmoLabel), getBottom(maxAmmoLabel) + verticalSpacing, labelWidth, labelHeight);
#endif

            /*
            // Attributes of the weapon itself
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

            /*
            Label ammoRechargeRateLabel = new Label();
            ammoRechargeRateLabel.Content = "Ammo Recharge/sec";
            addControl(ammoRechargeRateLabel, getLeft(weaponCostLabel), getBottom(maxAmmoLabel) + verticalSpacing, labelWidth, labelHeight);
            ammoRechargeRate = new TextBox();
            addControl(ammoRechargeRate, getLeft(weaponCost), getBottom(maxAmmo) + verticalSpacing, labelWidth, labelHeight);

            Label ammoPerBoxLabel = new Label();
            ammoPerBoxLabel.Content = "Ammo/box";
            addControl(ammoPerBoxLabel, getLeft(weaponCostLabel), getBottom(ammoRechargeRateLabel) + verticalSpacing, labelWidth, labelHeight);
            ammoPerBox = new TextBox();
            addControl(ammoPerBox, getLeft(weaponCost), getBottom(ammoRechargeRate) + verticalSpacing, labelWidth, labelHeight);

            
            Label fireWhileInactiveLabel = new Label();
            fireWhileInactiveLabel.Content = "Fire while Inactive";
            addControl(fireWhileInactiveLabel, getLeft(weaponCostLabel), getBottom(ammoPerBoxLabel) + verticalSpacing, labelWidth, labelHeight);
            fireWhileInactive = new CheckBox();
            addControl(fireWhileInactive, getLeft(weaponCost), getBottom(ammoPerBox) + verticalSpacing, labelWidth, labelHeight);

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
            */

            /*
            // attributes of the projectile
            TextBox remainingFlightTime;
            TextBox numExplosionsRemaining; // how many more times it can explode before being removed
            TextBox penetration;
            TextBox homingAccel;
            TextBox boomerangAccel;
            CheckBox homeOnProjectiles;
            CheckBox homeOnCharacters;
            */

            Label projectileLabel = new Label();
            projectileLabel.Content = "Projectile";
            addControl(projectileLabel, getRight(weaponLabel) + horizontalSpacing, getTop(weaponLabel), labelWidth, labelHeight);

            Label projectileImageLabel = new Label();
            projectileImageLabel.Content = "Image Name";
            addControl(projectileImageLabel, getLeft(projectileLabel), getBottom(projectileLabel) + verticalSpacing, labelWidth, labelHeight);
            projectileImage = new TextBox();
            addControl(projectileImage, getLeft(projectileImageLabel), getBottom(projectileImageLabel) + verticalSpacing, labelWidth, labelHeight);

            Label projectileRadiusLabel = new Label();
            projectileRadiusLabel.Content = "Radius";
            addControl(projectileRadiusLabel, getLeft(projectileImage), getBottom(projectileImage) + verticalSpacing, labelWidth, labelHeight);
            projectileRadius = new TextBox();
            addControl(projectileRadius, getLeft(projectileRadiusLabel), getBottom(projectileRadiusLabel) + verticalSpacing, labelWidth, labelHeight);

            Label projectileXLabel = new Label();
            projectileXLabel.Content = "Offset (X)";
            addControl(projectileXLabel, getLeft(projectileRadius), getBottom(projectileRadius) + verticalSpacing, labelWidth, labelHeight);
            projectileX = new TextBox();
            addControl(projectileX, getLeft(projectileXLabel), getBottom(projectileXLabel) + verticalSpacing, labelWidth, labelHeight);

            Label projectileYLabel = new Label();
            projectileYLabel.Content = "Offset (Y)";
            addControl(projectileYLabel, getLeft(projectileX), getBottom(projectileX) + verticalSpacing, labelWidth, labelHeight);
            projectileY = new TextBox();
            addControl(projectileY, getLeft(projectileYLabel), getBottom(projectileYLabel) + verticalSpacing, labelWidth, labelHeight);

            Label projectileVXLabel = new Label();
            projectileVXLabel.Content = "Velocity (X)";
            addControl(projectileVXLabel, getLeft(projectileY), getBottom(projectileY) + verticalSpacing, labelWidth, labelHeight);
            projectileVX = new TextBox();
            addControl(projectileVX, getLeft(projectileVXLabel), getBottom(projectileVXLabel) + verticalSpacing, labelWidth, labelHeight);

            Label projectileVYLabel = new Label();
            projectileVYLabel.Content = "Velocity (Y)";
            addControl(projectileVYLabel, getLeft(projectileVX), getBottom(projectileVX) + verticalSpacing, labelWidth, labelHeight);
            projectileVY = new TextBox();
            addControl(projectileVY, getLeft(projectileVYLabel), getBottom(projectileVYLabel) + verticalSpacing, labelWidth, labelHeight);

            Label projectileDragLabel = new Label();
            projectileDragLabel.Content = "Drag Coefficient";
            addControl(projectileDragLabel, getLeft(projectileVY), getBottom(projectileVY) + verticalSpacing, labelWidth, labelHeight);
            projectileDrag = new TextBox();
            addControl(projectileDrag, getLeft(projectileDragLabel), getBottom(projectileDragLabel) + verticalSpacing, labelWidth, labelHeight);

            Label projectileGravityLabel = new Label();
            projectileGravityLabel.Content = "Gravity";
            addControl(projectileGravityLabel, getLeft(projectileDrag), getBottom(projectileDrag) + verticalSpacing, labelWidth, labelHeight);
            projectileGravity = new TextBox();
            addControl(projectileGravity, getLeft(projectileGravityLabel), getBottom(projectileGravityLabel) + verticalSpacing, labelWidth, labelHeight);

            Label remainingFlightTimeLabel = new Label();
            remainingFlightTimeLabel.Content = "Max Flight time";
            addControl(remainingFlightTimeLabel, getLeft(projectileGravity), getBottom(projectileGravity) + verticalSpacing, labelWidth, labelHeight);
            remainingFlightTime = new TextBox();
            addControl(remainingFlightTime, getLeft(remainingFlightTimeLabel), getBottom(remainingFlightTimeLabel) + verticalSpacing, labelWidth, labelHeight);

            Label numExplosionsRemainingLabel = new Label();
            numExplosionsRemainingLabel.Content = "Max # Explosions";
            addControl(numExplosionsRemainingLabel, getLeft(remainingFlightTime), getBottom(remainingFlightTime) + verticalSpacing, labelWidth, labelHeight);
            numExplosionsRemaining = new TextBox();
            addControl(numExplosionsRemaining, getLeft(numExplosionsRemainingLabel), getBottom(numExplosionsRemainingLabel) + verticalSpacing, labelWidth, labelHeight);

            Label penetrationLabel = new Label();
            penetrationLabel.Content = "Penetration fraction";
            addControl(penetrationLabel, getLeft(numExplosionsRemaining), getBottom(numExplosionsRemaining) + verticalSpacing, labelWidth, labelHeight);
            penetration = new TextBox();
            addControl(penetration, getLeft(penetrationLabel), getBottom(penetrationLabel) + verticalSpacing, labelWidth, labelHeight);

            Label homingAccelLabel = new Label();
            homingAccelLabel.Content = "Homing Accel";
            addControl(homingAccelLabel, getLeft(penetration), getBottom(penetration) + verticalSpacing, labelWidth, labelHeight);
            homingAccel = new TextBox();
            addControl(homingAccel, getLeft(homingAccelLabel), getBottom(homingAccelLabel) + verticalSpacing, labelWidth, labelHeight);

            Label boomerangAccelLabel = new Label();
            boomerangAccelLabel.Content = "Boomerang Accel";
            addControl(boomerangAccelLabel, getLeft(homingAccel), getBottom(homingAccel) + verticalSpacing, labelWidth, labelHeight);
            boomerangAccel = new TextBox();
            addControl(boomerangAccel, getLeft(boomerangAccelLabel), getBottom(boomerangAccelLabel) + verticalSpacing, labelWidth, labelHeight);

            Label homeOnProjectilesLabel = new Label();
            homeOnProjectilesLabel.Content = "Home on Projectiles";
            addControl(homeOnProjectilesLabel, getLeft(boomerangAccel), getBottom(boomerangAccel) + verticalSpacing, labelWidth, labelHeight);
            homeOnProjectiles = new CheckBox();
            addControl(homeOnProjectiles, getLeft(homeOnProjectilesLabel), getBottom(homeOnProjectilesLabel) + verticalSpacing, labelWidth, labelHeight);

            Label homeOnPlayersLabel = new Label();
            homeOnPlayersLabel.Content = "Home on Players";
            addControl(homeOnPlayersLabel, getLeft(homeOnProjectiles), getBottom(homeOnProjectiles) + verticalSpacing, labelWidth, labelHeight);
            homeOnCharacters = new CheckBox();
            homeOnCharacters.IsChecked = true;
            addControl(homeOnCharacters, getLeft(homeOnPlayersLabel), getBottom(homeOnPlayersLabel) + verticalSpacing, labelWidth, labelHeight);

            /*
            // attributes of the explosion
            TextBox explosionDuration;
            CheckBox friendlyFire;
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
            addControl(explosionRadiusLabel, getLeft(explosionImage), getBottom(explosionImage) + verticalSpacing, labelWidth, labelHeight);
            explosionRadius = new TextBox();
            addControl(explosionRadius, getLeft(explosionRadiusLabel), getBottom(explosionRadiusLabel) + verticalSpacing, labelWidth, labelHeight);

            Label explosionDurationLabel = new Label();
            explosionDurationLabel.Content = "Duration";
            addControl(explosionDurationLabel, getLeft(explosionRadius), getBottom(explosionRadius) + verticalSpacing, labelWidth, labelHeight);
            explosionDuration = new TextBox();
            addControl(explosionDuration, getLeft(explosionDurationLabel), getBottom(explosionDurationLabel) + verticalSpacing, labelWidth, labelHeight);

            Label friendlyFireLabel = new Label();
            friendlyFireLabel.Content = "Friendly Fire";
            addControl(friendlyFireLabel, getLeft(explosionDuration), getBottom(explosionDuration) + verticalSpacing, labelWidth, labelHeight);
            friendlyFire = new CheckBox();
            addControl(friendlyFire, getLeft(friendlyFireLabel), getBottom(friendlyFireLabel) + verticalSpacing, labelWidth, labelHeight);

            Label knockbackLabel = new Label();
            knockbackLabel.Content = "Knockback Accel";
            addControl(knockbackLabel, getLeft(friendlyFire), getBottom(friendlyFire) + verticalSpacing, labelWidth, labelHeight);
            knockBack = new TextBox();
            addControl(knockBack, getLeft(knockbackLabel), getBottom(knockbackLabel) + verticalSpacing, labelWidth, labelHeight);


            /*
            // Attributes of the stun
            */

            Label stunLabel = new Label();
            stunLabel.Content = "Stun";
            addControl(stunLabel, getRight(explosionLabel) + horizontalSpacing, getTop(explosionLabel), labelWidth, labelHeight);

            Label damagePerSecondLabel = new Label();
            damagePerSecondLabel.Content = "Damage per Second";
            addControl(damagePerSecondLabel, getLeft(stunLabel), getBottom(stunLabel) + verticalSpacing, labelWidth, labelHeight);
            damagePerSecond = new TextBox();
            addControl(damagePerSecond, getLeft(damagePerSecondLabel), getBottom(damagePerSecondLabel) + verticalSpacing, labelWidth, labelHeight);

            Label timeMultiplierLabel = new Label();
            timeMultiplierLabel.Content = "Time Multiplier";
            addControl(timeMultiplierLabel, getLeft(damagePerSecond), getBottom(damagePerSecond) + verticalSpacing, labelWidth, labelHeight);
            timeMultiplier = new TextBox();
            addControl(timeMultiplier, getLeft(timeMultiplierLabel), getBottom(timeMultiplierLabel) + verticalSpacing, labelWidth, labelHeight);

            Label ammoDrainLabel = new Label();
            ammoDrainLabel.Content = "Ammo Drain per Sec";
            //addControl(ammoDrainLabel, getLeft(damagePerSecondLabel), getBottom(timeMultiplierLabel) + verticalSpacing, labelWidth, labelHeight);
            ammoDrain = new TextBox();
            //addControl(ammoDrain, getLeft(damagePerSecond), getBottom(timeMultiplier) + verticalSpacing, labelWidth, labelHeight);

            Label stunDurationLabel = new Label();
            stunDurationLabel.Content = "Duration";
            addControl(stunDurationLabel, getLeft(timeMultiplier), getBottom(timeMultiplier) + verticalSpacing, labelWidth, labelHeight);
            stunDuration = new TextBox();
            addControl(stunDuration, getLeft(stunDurationLabel), getBottom(stunDurationLabel) + verticalSpacing, labelWidth, labelHeight);

            // Some utility buttons

            Button quickBuildWeaponButton = new Button();
            quickBuildWeaponButton.Click += new RoutedEventHandler(quickBuildWeapon);
            quickBuildWeaponButton.Content = "Quick-build weapon";
            this.addControl(quickBuildWeaponButton, getRight(projectileGravity) + horizontalSpacing, getTop(projectileGravity), labelWidth, labelHeight);

            Button demoButton = new Button();
            demoButton.Click += new RoutedEventHandler(requestWeaponDemo);
            demoButton.Content = "Demo";
            this.addControl(demoButton, getLeft(quickBuildWeaponButton), getTop(numExplosionsRemaining), labelWidth, labelHeight);

            Button purchaseButton = new Button();
            purchaseButton.Click += new RoutedEventHandler(purchaseCurrentWeapon);
            purchaseButton.Content = "Get";
            this.addControl(purchaseButton, getLeft(quickBuildWeaponButton), getTop(homingAccel), labelWidth, labelHeight);

            Button doneButton = new Button();
            doneButton.Click += new RoutedEventHandler(requestToExit);
            doneButton.Content = "Done";
            this.addControl(doneButton, getLeft(quickBuildWeaponButton), getTop(homeOnProjectiles), labelWidth, labelHeight);

            /*
            Label currentMoneyLabel = new Label();
            currentMoneyLabel.Content = "Current Money";
            addControl(currentMoneyLabel, getLeft(weaponLabel) - labelWidth, getBottom(weaponLabel), labelWidth, labelHeight);
            currentMoney = new Label();
            addControl(currentMoney, getLeft(currentMoneyLabel), getBottom(currentMoneyLabel), labelWidth, labelHeight);
            */
        }
        // adds the textbox to the screen
        void addControl(TextBox box, double x, double y, double width, double height)
        {
            box.TextChanged += new TextChangedEventHandler(textBoxChanged);
            base.addControl(box, x, y, width, height);
        }
        // adds a checkbox to the screen
        void addControl(CheckBox box, double x, double y, double width, double height)
        {
            box.Checked += new RoutedEventHandler(checkBoxChanged);
            box.Unchecked += new RoutedEventHandler(checkBoxChanged);
            base.addControl(box, x, y, width, height);
        }
        // This function gets called whenever a text box changes
        void textBoxChanged(object sender, EventArgs e)
        {
            this.costUpdated = false;
        }
        // This function gets called whenever a check box changes
        void checkBoxChanged(object sender, EventArgs e)
        {
            this.costUpdated = false;
        }
        // put some reasonable guesses into the text boxes based on what's already present
        void fillInValues()
        {
            if (maxAmmo.Text == "")
                maxAmmo.Text = "5";
            if (ammoRechargeRate.Text == "")
                ammoRechargeRate.Text = ".1";
            if (ammoPerBox.Text == "")
                ammoPerBox.Text = ".1";
            if (warmupTime.Text == "")
                warmupTime.Text = ".1";
            if (cooldownTime.Text == "")
                cooldownTime.Text = ".9";
#if SWITCH_TIMES
        if (switchToTime.Text == "")
            switchToTime.Text = "1";
        if (switchFromTime.Text == "")
            switchFromTime.Text = "1";
#endif

            // attributes of the projectile
            if (projectileImage.Text == "")
                projectileImage.Text = "fan.png";
            if (projectileRadius.Text == "")
                projectileRadius.Text = "10";
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
            if (projectileGravity.Text == "")
                projectileGravity.Text = "0";
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
            if (ammoDrain.Text == "")
                ammoDrain.Text = "0";
            if (stunDuration.Text == "")
                stunDuration.Text = "0";
        }
        void quickBuildWeapon(object sender, EventArgs e)
        {
            quickBuildWeapon();
        }
        void quickBuildWeapon()
        {
            this.prebuildWeapon(this.quickbuildWeaponIndex);
            //this.weaponCost.Content = "0"; // templateWeapon.getCost().ToString();
            this.quickbuildWeaponIndex++;
            if (this.quickbuildWeaponIndex >= this.weaponFactory.getNumWeapons())
                quickbuildWeaponIndex = 0;
        }
        // this gets called when the user requests to try out the weapon
        void requestWeaponDemo(object sender, EventArgs e)
        {
            this.WantsDemo = true;
        }
        // calculate the cost of the weapon and update the onscreen label
        void calculateCost(object sender, EventArgs e)
        {
            calculateCost();
        }
        // calculate the cost of the weapon and update the onscreen label
        void calculateCost()
        {
            if (!(this.costUpdated))
            {
                this.updateCurrentWeapon();
                //this.templateWeapon.calculateCost();
                this.costUpdated = true;
            }
            //this.weaponCost.Content = "0"; // this.templateWeapon.getCost().ToString();
        }
        // have the player pay for and receive the current weapon
        void purchaseCurrentWeapon(object sender, EventArgs e)
        {
            this.calculateCost();
            //if (this.player.spendMoney(0))
            {
                BasicWeapon newWeapon = new BasicWeapon(templateWeapon.Stats);
                this.player.WeaponConfigurations.Insert(0, new WeaponDesign.WeaponConfiguration(newWeapon, new List<WeaponAugment>()));
            }
            // update the user's money
            //this.currentMoney.Content = this.player.getMoney().ToString();
        }

        // when the user wants to leave this screen
        void requestToExit(object sender, EventArgs e)
        {
            this.Done = true;
        }

        // update the attributes of the current weapon based on the text fields
        void updateCurrentWeapon()
        {
            WeaponStats stats = new WeaponStats();
            templateWeapon = new Weapon(stats);
            // attributes of the weapon
            stats.MaxAmmo = parseDouble(maxAmmo);
            stats.WarmupTime = parseDouble(warmupTime);
            stats.CooldownTime = parseDouble(cooldownTime);
#if SWITCH_TIMES
        templateWeapon.setSwitchToTime(parseDouble(switchToTime));
        templateWeapon.setSwitchFromTime(parseDouble(switchFromTime));
#endif
            stats.OwnersVelocityScale = parseDouble(ownersVelocityScale);

            Projectile templateProjectile = new Projectile();
            double[] tempVector = new double[2];
            tempVector[0] = parseDouble(projectileX);
            tempVector[1] = parseDouble(projectileY);
            templateProjectile.setCenter(tempVector);
            tempVector[0] = parseDouble(projectileVX);
            tempVector[1] = parseDouble(projectileVY);
            templateProjectile.setVelocity(tempVector);
            templateProjectile.setDragCoefficient(parseDouble(projectileDrag));
            templateProjectile.setGravity(parseDouble(projectileGravity));
            templateProjectile.setBitmap(ImageLoader.loadImage(projectileImage.Text));
            templateProjectile.setShape(new GameCircle(parseDouble(projectileRadius)));
            templateProjectile.setRemainingFlightTime(parseDouble(remainingFlightTime));
            templateProjectile.setNumExplosionsRemaining(parseInt(numExplosionsRemaining));
            templateProjectile.setPenetration(parseDouble(penetration));
            templateProjectile.setHomingAccel(parseDouble(homingAccel));
            templateProjectile.setBoomerangAccel(parseDouble(boomerangAccel));
            templateProjectile.enableHomingOnProjectiles(homeOnProjectiles.IsChecked.Value);
            templateProjectile.enableHomingOnCharacters(homeOnCharacters.IsChecked.Value);
            stats.TemplateProjectile = templateProjectile;

            Explosion templateExplosion = new Explosion();
            templateExplosion.setBitmap(ImageLoader.loadImage(explosionImage.Text));
            templateExplosion.setShape(new GameCircle(parseDouble(explosionRadius)));
            templateExplosion.setDuration(parseDouble(explosionDuration));
            templateExplosion.setFriendlyFireEnabled(friendlyFire.IsChecked.Value);
            templateExplosion.setKnockbackAccel(parseDouble(knockBack));
            templateProjectile.setTemplateExplosion(templateExplosion);

            Stun templateStun = new Stun();
            templateStun.setTimeMultiplier(parseDouble(timeMultiplier));
            templateStun.setDamagePerSecond(parseDouble(damagePerSecond));
            templateStun.setAmmoDrain(parseDouble(ammoDrain));
            templateStun.setDuration(parseDouble(stunDuration));
            templateExplosion.setTemplateStun(templateStun);

            templateWeapon = new Weapon(stats);
        }
        // create set text fields' values to the values for the given weapon type
        void prebuildWeapon(int type)
        {
            WeaponFactory factory = new WeaponFactory();
            factory.addDefaultWeapons();
            this.templateWeapon = factory.makeWeapon(type);
            this.updateFieldsFromWeapon();
            this.costUpdated = true;
        }
        void updateFieldsFromWeapon()
        {
            // attributes of the weapon
            maxAmmo.Text = templateWeapon.getMaxAmmo().ToString();
            warmupTime.Text = templateWeapon.getWarmupTime().ToString();
            cooldownTime.Text = templateWeapon.getCooldownTime().ToString();
#if SWITCH_TIMES
        switchToTime.Text = templateWeapon.getSwitchToTime().ToString();
        switchFromTime.Text = templateWeapon.getSwitchFromTime().ToString();
#endif
            ownersVelocityScale.Text = templateWeapon.Stats.OwnersVelocityScale.ToString();

            // attributes of the projectile
            Projectile templateProjectile = templateWeapon.Stats.TemplateProjectile;
            projectileX.Text = templateProjectile.getCenter()[0].ToString();
            projectileY.Text = templateProjectile.getCenter()[1].ToString();
            projectileVX.Text = templateProjectile.getVelocity()[0].ToString();
            projectileVY.Text = templateProjectile.getVelocity()[1].ToString();
            projectileDrag.Text = templateProjectile.getDragCoefficient().ToString();
            projectileGravity.Text = templateProjectile.getGravity().ToString();
            projectileImage.Text = templateProjectile.getImage().Source.ToString();
            projectileRadius.Text = ((templateProjectile.getShape().getWidth() + templateProjectile.getShape().getHeight()) / 4).ToString();
            remainingFlightTime.Text = templateProjectile.getRemainingFlightTime().ToString();
            numExplosionsRemaining.Text = templateProjectile.getNumExplosionsRemaining().ToString();
            penetration.Text = templateProjectile.getPenetration().ToString();
            homingAccel.Text = templateProjectile.getHomingAccel().ToString();
            boomerangAccel.Text = templateProjectile.getBoomerangAccel().ToString();
            homeOnProjectiles.IsChecked = templateProjectile.shouldHomeOnProjectiles();
            homeOnCharacters.IsChecked = templateProjectile.shouldHomeOnCharacters();

            // attributes of the explosion
            Explosion templateExplosion = templateProjectile.getTemplateExplosion();
            explosionImage.Text = templateExplosion.getImage().Source.ToString();
            explosionRadius.Text = ((templateExplosion.getShape().getWidth() + templateExplosion.getShape().getHeight()) / 4).ToString();
            explosionDuration.Text = templateExplosion.getDuration().ToString();
            friendlyFire.IsChecked = templateExplosion.isFriendlyFireEnabled();
            knockBack.Text = templateExplosion.getKnockbackAccel().ToString();

            // attributes of the stun
            Stun templateStun = templateExplosion.getTemplateStun();
            timeMultiplier.Text = templateStun.getTimeMultiplier().ToString();
            damagePerSecond.Text = templateStun.getDamagePerSecond().ToString();
            ammoDrain.Text = templateStun.getAmmoDrain().ToString();
            stunDuration.Text = templateStun.getDuration().ToString();

            // now update anything else that might need it
            //this.calculateCost();
            //this.weaponCost.Content = "0";
        }
        public bool WantsDemo;
        public bool Done;


        GamePlayer player;
        Label currentMoney;
        WorldScreen demoWorld;
        //LevelPlayer demoPlayer;
        CharacterStatusDisplay statusDisplay;
        int quickbuildWeaponIndex;
        bool costUpdated;
        // attributes of the weapon itself, rather than the projectiles or explosions
        //Label weaponCost;
        TextBox maxAmmo;
        TextBox ammoRechargeRate;
        TextBox ammoPerBox;
        TextBox warmupTime;
        TextBox cooldownTime;
#if SWITCH_TIMES
    TextBox switchToTime;
    TextBox switchFromTime;
#endif
        CheckBox automatic;
        CheckBox stickyTrigger;
        TextBox ownersVelocityScale;
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
        TextBox projectileGravity;
        TextBox remainingFlightTime;
        TextBox numExplosionsRemaining; // how many more times it can explode before being removed
        TextBox penetration;
        TextBox homingAccel;
        TextBox boomerangAccel;
        CheckBox homeOnProjectiles;
        CheckBox homeOnCharacters;

        // attributes of the explosion
        TextBox explosionImage;
        TextBox explosionRadius;
        TextBox explosionDuration;
        CheckBox friendlyFire;
        TextBox knockBack;

        // attributes of the stun
        TextBox timeMultiplier;
        TextBox ammoDrain;
        TextBox damagePerSecond;
        TextBox stunDuration;

        Weapon templateWeapon;

        WeaponFactory weaponFactory;
    }
}

#endif