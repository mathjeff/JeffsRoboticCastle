using Castle.WeaponDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

// Creates worlds (technically WorldLoader objects) of the desired difficulty levels
namespace Castle.World
{
    class WorldFactory
    {
        public WorldFactory(Random randomGenerator)
        {
            this.randomGenerator = randomGenerator;
        }
        public WorldLoader Build(double difficulty, Size realityBubbleSize)
        {
            return this.makeWorld(this.chooseWorldStats(difficulty), realityBubbleSize);
        }

        private List<ChallengeStats> chooseWorldStats(double difficulty)
        {
            // To prevent the user from getting bored from screen to screen,
            // each level is made up of blocks having individual stats (themes)
            // To prevent the user from getting bored from level to level,
            // each level has its own individual stats (themes)

            // Choose overall approximate difficulties in each category for the level
            List<double> levelAttributes = new List<double>();
            levelAttributes.Add(this.randomGenerator.Next()); // density of enemies in world
            levelAttributes.Add(this.randomGenerator.Next()); // hitpoints of each enemy
            levelAttributes.Add(this.randomGenerator.Next()); // intelligence of each enemy
            levelAttributes.Add(this.randomGenerator.Next()); // level of the weapon of each enemy
            levelAttributes.Add(this.randomGenerator.Next()); // enemy acceleration

            levelAttributes = this.normalize(levelAttributes, 1);
            levelAttributes.Add(1); // size of each block: different between blocks but same across levels

            // Choose the difficulty of each block
            //int numBlocks = (int)(levelAttributes[0] * 6 + 2); // always at least 2 blocks
            int numBlocks = (int)difficulty;
            List<double> blockAverages = levelAttributes;
            //List<double> blockAverages = levelAttributes.GetRange(1, levelAttributes.Count - 1);
            List<double> blockDifficulties = new List<double>();
            for (int i = 0; i < numBlocks; i++)
            {
                blockDifficulties.Add(this.randomGenerator.Next());
            }
            blockDifficulties = this.normalize(blockDifficulties, difficulty);

            // Make a bunch of block stats based on the difficulty of each block and the overall level themes
            List<ChallengeStats> blocks = new List<ChallengeStats>();
            for (int i = 0; i < numBlocks; i++)
            {
                // Make some block stats weighted towards the average stats for this level
                List<double> blockStats = new List<double>();
                for (int j = 0; j < blockAverages.Count; j++)
                {
                    blockStats.Add(blockAverages[j] * this.randomGenerator.Next());
                }
                // Reweight the difficulty of this block to be the intended value
                blockStats = this.normalize(blockStats, blockDifficulties[i]);
                ChallengeStats block = new ChallengeStats();
                if (blockStats.Count != 6)
                {
                    throw new Exception("Internal error - mismatched numer of fields");
                }
                block.Width = 3000 * (1 + blockStats[5]);
                double enemyDensity = 0.001 * (blockStats[0] + 1); // always at least 1 enemy every 1000 pixels
                block.NumEnemies = (int)(block.Width * enemyDensity + 1); // always at least 1 enemy
                block.EnemyHitpoints = 0.01 + blockStats[1];
                block.EnemyIntelligence = blockStats[2];
                block.EnemyWeaponLevel = Math.Max((int)Math.Log(blockStats[3], 2), 0);
                block.EnemyAcceleration = blockStats[4] * 200;
                blocks.Add(block);
            }
            return blocks;
        }

        private WorldLoader makeWorld(List<ChallengeStats> blockStats, Size realityBubbleSize)
        {
            double worldHeight = 2000;
            double worldWidth = 0;
            foreach (ChallengeStats stats in blockStats)
            {
                worldWidth += stats.Width;
            }
            Size size = new Size(worldWidth, worldHeight);;
            WorldLoader worldLoader = new WorldLoader(size, realityBubbleSize);
            double blockX = 0;
            Dictionary<int, WeaponStats> weaponsByLevel = new Dictionary<int, WeaponStats>();
            foreach (ChallengeStats stats in blockStats)
            {
                double blockWidth = stats.Width;
                int numEnemies = stats.NumEnemies;
                BasicWeapon basicWeapon = new BasicWeapon();

                int weaponLevel = (int)stats.EnemyWeaponLevel;
                if (!weaponsByLevel.ContainsKey(weaponLevel))
                    weaponsByLevel[weaponLevel] = this.makeWeapon(weaponLevel);
                WeaponStats weaponStats = weaponsByLevel[weaponLevel];

                for (int i = 0; i < stats.NumEnemies; i++)
                {
                    double enemyX = this.randomGenerator.NextDouble() * blockWidth + blockX;
                    double enemyY = this.randomGenerator.NextDouble() * worldHeight / 4;
                    int enemyIntelligence = (int)stats.EnemyIntelligence;
                    enemyIntelligence = Math.Max(Math.Min(enemyIntelligence, 1), 0);
                    double enemyHitpoints = stats.EnemyHitpoints;

                    Enemy enemy = new Enemy(ImageLoader.loadImage("archer.png"),
                        new Point(enemyX, enemyY),
                        new GameRectangle(26, 41),
                        new double[] { stats.EnemyAcceleration, 0 },
                        stats.EnemyHitpoints,
                        950,
                        enemyIntelligence);
                    worldLoader.addItem(enemy);
                }

                blockX += blockWidth;

            }
            return worldLoader;
        }

        private WeaponStats makeWeapon(int numAugments)
        {
            BasicWeapon basicWeapon = this.weaponAugmentFactory.BasicWeapon;
            List<WeaponAugmentTemplate> augmentTemplates = new List<WeaponAugmentTemplate>();
            List<WeaponAugmentTemplate> choices = new List<WeaponAugmentTemplate>(this.weaponAugmentFactory.All);
            for (int i = 0; i < numAugments; i++)
            {
                int index = this.randomGenerator.Next(choices.Count);
                WeaponAugmentTemplate template = choices[index];
                augmentTemplates.Add(template);
            }
            WeaponStats stats = basicWeapon.WithAugments(augmentTemplates);
            return stats;
        }
        // exponentially-distributed random
        private double exponentialRandom()
        {
            double result = 0.25;
            for (int i = 0; i < 5; i++)
            {
                if (this.randomGenerator.Next(2) == 0)
                    return result;
                result *= 2;
            }
            return result;
        }


        // Returns a list of values whose average is as requested
        private List<double> normalize(List<double> values, double average)
        {
            double sum = 0;
            foreach (double val in values)
            {
                sum += val;
            }
            if (sum == 0)
                sum = 1;
            double multiplier = values.Count * average / sum;
            List<double> results = new List<double>();
            foreach(double val in values)
            {
                results.Add(val * multiplier);
            }
            return results;
        }
        private Random randomGenerator;
        private WeaponAugmentFactory weaponAugmentFactory = new WeaponAugmentFactory();
    }
}
