using UnityEngine;

namespace Duskvern
{
    public interface IPoolable
    {
        bool InPool { get; set; }

        void Spawn();

        void DeSpawn();
    }
}
