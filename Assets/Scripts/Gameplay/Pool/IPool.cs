namespace Gameplay.Pool
{
    public interface IPool<T>
    {
        T Spawn();
        void Prewarm(int targetCount);
        void Despawn(T target);
    }
}
