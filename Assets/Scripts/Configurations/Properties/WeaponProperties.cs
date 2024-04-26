using Core.ResourceEnums;
namespace Configurations.Properties
{
    public class WeaponProperties
    {
        public EProjectiles ProjectilePrefab;
        public float ProjectileSpeed;
        public float CooldownBetweenShotsTime;
        public EHitTypes HitType; 
        
        public int AmmoSize;
        public float ReloadTime;

        public bool CannotBeDestroyed;
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
