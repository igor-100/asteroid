using System;
using System.Collections.Generic;
namespace Gameplay.Pool
{
    public class Pool<T> : IPool<T> where T : class
    {
        public int FreeCount => Free.Count;

        protected readonly List<T> Free = new();

        private readonly Func<T> factory;

        public Pool(Func<T> create)
        {
            if (ReferenceEquals(create, null))
                throw new ArgumentNullException(nameof(create));

            factory = create;
        }

        public void Prewarm(int targetCount)
        {
            if (Free.Capacity < targetCount)
                Free.Capacity = targetCount;

            for (var i = 0; i < targetCount && Free.Count < targetCount; i++)
                Despawn(factory());
        }

        public T Spawn()
        {
            var obj = SpawnInternal();
            OnSpawned(obj);

            return obj;
        }

        protected virtual T SpawnInternal()
        {
            T obj;

            if (Free.Count > 0)
            {
                obj = Free[^1];
                Free.RemoveAt(Free.Count - 1);
            }
            else
                obj = factory();

            return obj;
        }

        public virtual void Despawn(T target)
        {
            if (ReferenceEquals(target, null))
                throw new ArgumentNullException(nameof(target));

            OnDespawned(target);
            Free.Add(target);
        }

        protected virtual void OnSpawned(T target)
        {
            if (target is IPoolable poolable)
                poolable.OnSpawned();
        }

        protected virtual void OnDespawned(T target)
        {
            if (target is IPoolable poolable)
                poolable.OnDespawned();
        }
    }
}
