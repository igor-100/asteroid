using System.Collections.Generic;
using Asteroid.Configurations.ResourceEnums;
using Core.ResourceEnums;
namespace Configurations.Properties
{
    public class LevelProperties
    {
        public ELevels LevelPrefab;
        public List<(EEnemies enemy, int spawnWeight)> EnemiesToSpawn;
        public (float min, float max) InitialSpawnDelay;
        public int TotalSpawnsToGetToTheFinalLevel;
        public (float min, float max) FinalSpawnDelay;
    }
}
