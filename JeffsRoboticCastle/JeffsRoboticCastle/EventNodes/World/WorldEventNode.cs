using Castle.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Castle.EventNodes.World
{
    class WorldEventNode : EventNode
    {
        public WorldEventNode(GamePlayer player, int difficulty, Random randomGenerator)
        {
            this.player = player;
            this.difficulty = difficulty;
            this.randomGenerator = randomGenerator;
        }
        public EventNode SuccessNode;
        //public EventNode FailureNode;
        public void Show(Size size)
        {
            WorldFactory worldFactory = new WorldFactory(this.randomGenerator);
            this.worldLoader = worldFactory.Build(this.difficulty, size);
            LevelPlayer levelPlayer = this.player.PrepareForNewLevel();
            this.worldLoader.addItemAndDisableUnloading(levelPlayer);
            this.screen = new WorldScreen(size, levelPlayer, this.worldLoader);
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
        private GamePlayer player;
        WorldLoader worldLoader;
        int difficulty;
        Random randomGenerator;

    }
}
