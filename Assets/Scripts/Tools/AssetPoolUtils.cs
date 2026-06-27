using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    // 传入资源路径，先通过 AssetUtils 加载 prefab，再通过对象池生成实例。
    public static class AssetPoolUtils
    {
        public static GameObject Spawn(string assetPath)
        {
            return Spawn(assetPath, Vector3.zero, Quaternion.identity, Vector3.one, null, false);
        }

        public static GameObject Spawn(string assetPath, Transform parent, bool worldPositionStays = false)
        {
            return Spawn(assetPath, Vector3.zero, Quaternion.identity, Vector3.one, parent, worldPositionStays);
        }

        public static GameObject Spawn(string assetPath, Vector3 localPosition)
        {
            return Spawn(assetPath, localPosition, Quaternion.identity, Vector3.one, null, false);
        }

        public static GameObject Spawn(string assetPath, Vector3 localPosition, Transform parent,
            bool worldPositionStays = false)
        {
            return Spawn(assetPath, localPosition, Quaternion.identity, Vector3.one, parent, worldPositionStays);
        }

        public static GameObject Spawn(string assetPath, Vector3 localPosition, Quaternion localRotation)
        {
            return Spawn(assetPath, localPosition, localRotation, Vector3.one, null, false);
        }

        public static GameObject Spawn(string assetPath, Vector3 localPosition, Quaternion localRotation,
            Transform parent, bool worldPositionStays = false)
        {
            return Spawn(assetPath, localPosition, localRotation, Vector3.one, parent, worldPositionStays);
        }

        public static GameObject Spawn(string assetPath, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale)
        {
            return Spawn(assetPath, localPosition, localRotation, localScale, null, false);
        }

        public static GameObject Spawn(string assetPath, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent)
        {
            return Spawn(assetPath, localPosition, localRotation, localScale, parent, false);
        }

        public static GameObject Spawn(string assetPath, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            var prefab = LoadPrefab(assetPath);
            return SpawnByPrefab(prefab, localPosition, localRotation, localScale, parent, worldPositionStays);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath)
        {
            return SpawnAsync(assetPath, Vector3.zero, Quaternion.identity, Vector3.one, null, false);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath, Transform parent,
            bool worldPositionStays = false)
        {
            return SpawnAsync(assetPath, Vector3.zero, Quaternion.identity, Vector3.one, parent, worldPositionStays);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath, Vector3 localPosition)
        {
            return SpawnAsync(assetPath, localPosition, Quaternion.identity, Vector3.one, null, false);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath, Vector3 localPosition, Transform parent,
            bool worldPositionStays = false)
        {
            return SpawnAsync(assetPath, localPosition, Quaternion.identity, Vector3.one, parent, worldPositionStays);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath, Vector3 localPosition,
            Quaternion localRotation)
        {
            return SpawnAsync(assetPath, localPosition, localRotation, Vector3.one, null, false);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath, Vector3 localPosition,
            Quaternion localRotation, Transform parent, bool worldPositionStays = false)
        {
            return SpawnAsync(assetPath, localPosition, localRotation, Vector3.one, parent, worldPositionStays);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath, Vector3 localPosition,
            Quaternion localRotation, Vector3 localScale)
        {
            return SpawnAsync(assetPath, localPosition, localRotation, localScale, null, false);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath, Vector3 localPosition,
            Quaternion localRotation, Vector3 localScale, Transform parent)
        {
            return SpawnAsync(assetPath, localPosition, localRotation, localScale, parent, false);
        }

        public static UniTask<GameObject> SpawnAsync(string assetPath, Vector3 localPosition,
            Quaternion localRotation, Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            return SpawnAsyncCore(assetPath, localPosition, localRotation, localScale, parent, worldPositionStays);
        }

        private static async UniTask<GameObject> SpawnAsyncCore(string assetPath, Vector3 localPosition,
            Quaternion localRotation, Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                Logger.LogPoolWarning("Asset path is null or empty");
                return null;
            }

            var prefab = await AssetUtils.LoadAsync<GameObject>(assetPath);
            return SpawnByPrefab(prefab, localPosition, localRotation, localScale, parent, worldPositionStays);
        }

        private static GameObject SpawnByPrefab(GameObject prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            if (prefab == null)
            {
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

        public static void Despawn(GameObject obj, float delay = 0f)
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

        private static GameObject LoadPrefab(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                Logger.LogPoolWarning("Asset path is null or empty");
                return null;
            }

            return AssetUtils.Load<GameObject>(assetPath);
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
