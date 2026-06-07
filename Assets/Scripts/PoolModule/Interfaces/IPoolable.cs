using UnityEngine;

namespace Duskvern
{
    public interface IPoolable
    {
        void OnSpawn();

        void OnDeSpawn();
    }

    public class Delay : IPoolable
    {
        public GameObject clone;
        public float delayTime;
        public void OnSpawn() { }
        public void OnDeSpawn()
        {
            delayTime = 0;
            clone = null;
        }
    }
}
