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
            if (!CheckInit())
            {
                return;
            }

            poolModule.OnUpdate(_deltaTime);
        }

        public static GameObject Pop(GameObject _prefab)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab);
        }

        public static GameObject Pop(GameObject _prefab, Transform parent, bool worldPositionStays = false)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab, parent, worldPositionStays);
        }

        public static GameObject Pop(GameObject _prefab, Vector3 localPosition)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab, localPosition);
        }

        public static GameObject Pop(GameObject _prefab, Vector3 localPosition, Transform parent,
            bool worldPositionStays = false)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab, localPosition, parent, worldPositionStays);
        }

        public static GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab, localPosition, localRotation);
        }

        public static GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Transform parent, bool worldPositionStays = false)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab, localPosition, localRotation, parent, worldPositionStays);
        }

        public static GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab, localPosition, localRotation, localScale);
        }

        public static GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab, localPosition, localRotation, localScale, parent);
        }

        public static GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            if (!CheckInit())
            {
                return null;
            }

            return poolModule.Pop(_prefab, localPosition, localRotation, localScale, parent, worldPositionStays);
        }

        public static void Push(GameObject _obj, float _delay = 0f)
        {
            if (!CheckInit())
            {
                return;
            }

            poolModule.Push(_obj, _delay);
        }

        public static void AddPool(GameObject _prefab)
        {
            if (!CheckInit())
            {
                return;
            }

            poolModule.AddPool(_prefab);
        }

        public static void AddPool(GameObject _prefab, PoolContainerParam _poolContainerParam)
        {
            if (!CheckInit())
            {
                return;
            }

            poolModule.AddPool(_prefab, _poolContainerParam);
        }

        public static void Clear(GameObject _prefab)
        {
            if (!CheckInit())
            {
                return;
            }

            poolModule.ClearContainerElments(_prefab);
        }

        public static void ClearAll()
        {
            if (!CheckInit())
            {
                return;
            }

            poolModule.ClearAllContainerElments();
        }

        private static bool CheckInit()
        {
            if (poolModule != null)
            {
                return true;
            }

            Logger.LogPoolWarning("PoolUtil is not initialized, call PoolUtil.Init first");
            return false;
        }
    }
}
