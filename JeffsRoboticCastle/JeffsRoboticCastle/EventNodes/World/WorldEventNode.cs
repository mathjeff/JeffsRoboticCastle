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
            this.gamePlayer = player;
            this.difficulty = difficulty;
            this.randomGenerator = randomGenerator;
        }
        public WorldEventNode(WorldLoader worldLoader, LevelPlayer levelPlayer, Size size)
        {
            this.worldLoader = worldLoader;
            this.levelPlayer = levelPlayer;
            //this.screen = new WorldScreen(size, levelPlayer, this.worldLoader);
        }
        public EventNode SuccessNode;
        //public EventNode FailureNode;
        public void Show(Size size)
        {
            if (this.levelPlayer == null)
            {
                this.levelPlayer = this.gamePlayer.PrepareForNewLevel();
            }
            if (this.worldLoader == null)
            {
                WorldFactory worldFactory = new WorldFactory(this.randomGenerator);
                this.worldLoader = worldFactory.Build(this.difficulty, size);
                this.worldLoader.addItemAndDisableUnloading(levelPlayer);
            }
            this.screen = new WorldScreen(size, this.levelPlayer, this.worldLoader);
            if (this.EscapeEnabled)
                this.screen.setEscapeEnabled(true);
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
        public bool EscapeEnabled { get; set; }
        private void cleanup()
        {
            this.screen = null; // enable garbage-collection of anything that was part of the screen
            this.worldLoader.destroy();
        }
        WorldScreen screen;
        LevelPlayer levelPlayer;
        GamePlayer gamePlayer;
        WorldLoader worldLoader;
        int difficulty;
        Random randomGenerator;

    }
}
