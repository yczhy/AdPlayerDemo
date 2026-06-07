using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    public static class PoolUtil
    {
        private static PoolModule poolModule;

        public static void Init(List<PoolContainerParam> _poolContainerParams = null)
        {
            poolModule = new PoolModule().Init(_poolContainerParams);
        }

        public static void OnUpdate(float _deltaTime)
        {
            poolModule.OnUpdate(_deltaTime);
        }

        public static GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            return poolModule.Pop(_prefab, localPosition, localRotation, localScale, parent, worldPositionStays);
        }

        public static void Push(GameObject _obj, float _delay = 0f)
        {
            poolModule.Push(_obj, _delay);
        }

        public static void AddPool(GameObject _prefab)
        {
            poolModule.AddPool(_prefab);
        }

        public static void AddPool(GameObject _prefab, PoolContainerParam _poolContainerParam)
        {
            poolModule.AddPool(_prefab, _poolContainerParam);
        }

        public static void Clear(GameObject _prefab)
        {
            poolModule.ClearContainerElments(_prefab);
        }

        public static void ClearAll()
        {
            poolModule.ClearAllContainerElments();
        }
    }
}