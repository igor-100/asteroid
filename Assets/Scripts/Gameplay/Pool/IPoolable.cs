namespace Gameplay.Pool
{
    public interface IPoolable
    {
        void OnSpawned();
        void OnDespawned();
    }
}
