using Core.ResourceEnums;
namespace Configurations.Properties
{
    public class WeaponProperties
    {
        public EProjectiles ProjectileType;
        public float ProjectileSpeed;
        public float CooldownBetweenShotsTime;
        
        public bool IsEndless;
        public int AmmoSize;
        public float ReloadTime;

        public bool IsToDisappearAfterTime;
        public float DisappearTime;
    }

    public enum EWeapon
    {
        Gun,
        Laser
    }
}
