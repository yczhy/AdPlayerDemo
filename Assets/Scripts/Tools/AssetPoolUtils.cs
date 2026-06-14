using UnityEngine;

namespace Duskvern
{
    // 只负责对象的取出和回收：
    // 1. 取出对象：先看对象池，没有则实例化
    // 2. 回收对象：先看对象池，没有则销毁
    public static class AssetPoolUtils
    {
        public static GameObject Pop(GameObject prefab)
        {
            return Pop(prefab, Vector3.zero, Quaternion.identity, Vector3.one, null, false);
        }

        public static GameObject Pop(GameObject prefab, Transform parent, bool worldPositionStays = false)
        {
            return Pop(prefab, Vector3.zero, Quaternion.identity, Vector3.one, parent, worldPositionStays);
        }

        public static GameObject Pop(GameObject prefab, Vector3 localPosition)
        {
            return Pop(prefab, localPosition, Quaternion.identity, Vector3.one, null, false);
        }

        public static GameObject Pop(GameObject prefab, Vector3 localPosition, Transform parent,
            bool worldPositionStays = false)
        {
            return Pop(prefab, localPosition, Quaternion.identity, Vector3.one, parent, worldPositionStays);
        }

        public static GameObject Pop(GameObject prefab, Vector3 localPosition, Quaternion localRotation)
        {
            return Pop(prefab, localPosition, localRotation, Vector3.one, null, false);
        }

        public static GameObject Pop(GameObject prefab, Vector3 localPosition, Quaternion localRotation,
            Transform parent, bool worldPositionStays = false)
        {
            return Pop(prefab, localPosition, localRotation, Vector3.one, parent, worldPositionStays);
        }

        public static GameObject Pop(GameObject prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale)
        {
            return Pop(prefab, localPosition, localRotation, localScale, null, false);
        }

        public static GameObject Pop(GameObject prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent)
        {
            return Pop(prefab, localPosition, localRotation, localScale, parent, false);
        }

        public static GameObject Pop(GameObject prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            if (prefab == null)
            {
                Logger.LogPoolWarning("Prefab is null");
                return null;
            }

            if (PoolUtil.IsInitialized)
            {
                var clone = PoolUtil.Pop(prefab, localPosition, localRotation, localScale, parent, worldPositionStays);
                if (clone != null)
                {
                    return clone;
                }
            }

            return Instantiate(prefab, localPosition, localRotation, localScale, parent, worldPositionStays);
        }

        public static void Push(GameObject obj, float delay = 0f)
        {
            if (obj == null)
            {
                Logger.LogPoolWarning("Object is null");
                return;
            }

            if (PoolUtil.IsInitialized && PoolUtil.TryPush(obj, delay))
            {
                return;
            }

            DestroyObject(obj, delay);
        }

        private static GameObject Instantiate(GameObject prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            var clone = Object.Instantiate(prefab, parent);
            var cloneTransform = clone.transform;
            cloneTransform.localPosition = localPosition;
            cloneTransform.localRotation = localRotation;
            cloneTransform.localScale = localScale;

            if (parent == null && worldPositionStays == false)
            {
                clone.transform.SetParent(null, false);
            }

            clone.name = prefab.name;
            return clone;
        }

        private static void DestroyObject(GameObject obj, float delay = 0f)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(obj, delay);
            }
            else
            {
                Object.DestroyImmediate(obj);
            }
        }
    }
}
