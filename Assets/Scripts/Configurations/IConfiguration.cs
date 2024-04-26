using System.Collections.Generic;
using Asteroid.Configurations.ResourceEnums;
using Configurations.Properties;
namespace Configurations
{
    public interface IConfiguration
    {
        PlayerProperties PlayerProperties { get; }
        LevelProperties LevelProperties { get; }
        WeaponProperties GetWeapon(EWeapon weapon);
        EnemyProperties GetEnemy(EEnemies eEnemies);
        IReadOnlyDictionary<EEnemies, EnemyProperties> Enemies { get; }
    }
}
