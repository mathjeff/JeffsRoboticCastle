using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Castle.EventNodes.World
{
    class WorldEventNode : EventNode
    {
        public WorldEventNode(Player player, List<Weapon> enemyWeapons)
        {
            this.player = player;
            this.enemyWeapons = enemyWeapons;
        }
        public EventNode SuccessNode;
        //public EventNode FailureNode;
        public void Show(Size size)
        {
            this.worldLoader = new WorldLoader(size, 1, this.enemyWeapons);
            this.worldLoader.addItemAndDisableUnloading(this.player);
            this.screen = new WorldScreen(size, this.player, this.worldLoader);
        }
        public Screen GetScreen()
        {
            return this.screen;
        }
        public EventNode TimerTick(double numSeconds)
        {
            this.screen.TimerTick(numSeconds);
            if (this.screen.isPlayerSuccessful())
            {
                this.cleanup();
                return this.SuccessNode;
            }
            return this; // Currently we allow the player to continue to watch the world when they lose
        }
        private void cleanup()
        {
            this.screen = null; // enable garbage-collection of anything that was part of the screen
            this.worldLoader.destroy();
        }
        private WorldScreen screen;
        private Player player;
        private List<Weapon> enemyWeapons;
        WorldLoader worldLoader;

    }
}
