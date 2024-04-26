using Core.ResourceEnums;
namespace Configurations.Properties
{
    public class WeaponProperties
    {
        public EProjectiles ProjectilePrefab;
        public float ProjectileSpeed;
        public float CooldownBetweenShotsTime;
        public EHitTypes HitType; 
        
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

    public enum EHitTypes
    {
        Hit,
        Destroy,
    }
}
