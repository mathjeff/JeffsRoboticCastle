using Castle.EventNodes.Menus;
using Castle.EventNodes.World;
using Castle.Language;
using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

// Lets the user view and reconfigure their weapons
namespace Castle.EventNodes.Customization
{
    class ShopEventNode : EventNode
    {
        public ShopEventNode(GamePlayer player)
        {
            this.player = player;
        }
        public void Show(Size size)
        {
            this.size = size;
            this.designScreen = new WeaponDesignScreen(size, this.player);
            this.showMenu();
        }
        public Screen GetScreen()
        {
            return this.currentScreen;
        }
        private void showScreen(Screen screen)
        {
            this.currentScreen = screen;
        }
        public EventNode TimerTick(double numSeconds)
        {
            if (this.currentScreen == this.menuScreen)
            {
                if (this.menuScreen.Chosen)
                {
                    if (this.menuScreen.Choice == "edit")
                        this.showSelection();
                    else if (this.menuScreen.Choice == "test")
                    {
                        this.showScreen(this.makeWorldScreen());
                    }
                    else if (this.menuScreen.Choice == "exit")
                    {
                        this.cleanup();
                        return this.NextNode;
                    }
                    this.menuScreen.Chosen = false;
                }
            }
            else if (this.currentScreen == this.selectionScreen)
            {
                if (this.selectionScreen.Chosen)
                {
                    if (selectionScreen.Choice == null)
                    {
                        this.showMenu();
                    }
                    else
                    {
                        WeaponConfiguration weaponConfiguration = selectionScreen.Choice;
                        this.designScreen.Show(weaponConfiguration);
                        this.showScreen(designScreen);
                    }
                    this.selectionScreen.Chosen = false;
                }
            }
            else if (this.currentScreen == this.designScreen)
            {
                if (this.designScreen.Done)
                    this.showSelection();
            }
            else if (this.currentScreen == this.worldScreen)
            {
                this.worldScreen.TimerTick(numSeconds);
                if (this.worldScreen.isPlayerSuccessful())
                {
                    this.showMenu();
                }
            }
            return this;
        }
        private void showMenu()
        {
            Dictionary<WeaponAugmentTemplate, List<WeaponAugment>> unassignedAugments = this.player.GetUnassignedWeaponAugments();
            int numUnassignedAugments = 0;
            foreach (KeyValuePair<WeaponAugmentTemplate, List<WeaponAugment>> entry in unassignedAugments)
            {
                numUnassignedAugments += entry.Value.Count;
            }
            this.menuScreen = new SelectionScreen<string>(size, "Choose an Action",
                new Dictionary<String, String>(){
                {"View/Edit Weapons (you have " +
                    LanguageUtils.FormatQuantity(numUnassignedAugments, "unassigned weapon augment")
                    + ")", "edit"},
                {"Try out your Weapons", "test"},
                {"Continue your Journey","exit"}
            });
            this.showScreen(this.menuScreen);
        }
        private void showSelection()
        {
            this.selectionScreen = new SelectionScreen<WeaponConfiguration>(size,
                "Choose a weapon to view/edit",
                new ChoiceBuilder<WeaponConfiguration>(player.WeaponConfigurations)
                .And("Back", null));
            this.showScreen(this.selectionScreen);
        }
        private WorldScreen makeWorldScreen()
        {
            WeaponAugmentFactory factory = new WeaponAugmentFactory();
            List<WeaponAugmentTemplate> templates = new List<WeaponAugmentTemplate>(){
                            factory.Damager, factory.Flier};
            WeaponStats weaponStats = factory.BasicWeapon.WithAugments(templates);

            List<WeaponStats> enemyWeapons = new List<WeaponStats>() { weaponStats };

            Size size = this.size;
            WorldLoader worldLoader = new WorldLoader(size, 2, enemyWeapons);
            LevelPlayer levelPlayer = this.player.PrepareForNewLevel();
            worldLoader.addItemAndDisableUnloading(levelPlayer);
            this.worldScreen = new WorldScreen(size, levelPlayer, worldLoader);
            this.worldScreen.setEscapeEnabled(true);
            return this.worldScreen;
        }
        private void cleanup()
        {
            this.designScreen = null;
            this.selectionScreen = null;
            this.menuScreen = null;
            this.currentScreen = null;
            this.worldScreen = null;
        }
        public EventNode NextNode;
        private GamePlayer player;

        private WeaponDesignScreen designScreen;
        private SelectionScreen<WeaponConfiguration> selectionScreen;
        private SelectionScreen<String> menuScreen;
        private WorldScreen worldScreen;
        private Screen currentScreen;
        Size size;
        
    }
}
