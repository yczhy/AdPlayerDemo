using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    public class PoolModule
    {
        private readonly Dictionary<GameObject, PoolContainer> pools = new();
        private readonly Dictionary<GameObject, GameObject> cloneToPoolMap = new();
        private readonly List<IPoolable> tempPoolable = new();

        private Transform poolRoot;
        private Transform poolDeActiveRoot;

        public PoolModule Init(List<PoolContainerParam> _poolContainerParams = null)
        {
            if (poolRoot != null)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(poolRoot.gameObject);
                }
                else
                {
                    Object.DestroyImmediate(poolRoot.gameObject);
                }
            }

            pools.Clear();
            cloneToPoolMap.Clear();
            tempPoolable.Clear();
            poolRoot = new GameObject("PoolRoot").transform;
            if (Application.isPlaying)
            {
                Object.DontDestroyOnLoad(poolRoot.gameObject);
            }
            poolDeActiveRoot = new GameObject("PoolDeActiveRoot").transform;
            poolDeActiveRoot.SetParent(poolRoot);
            poolDeActiveRoot.gameObject.SetActive(false);

            List<PoolContainerParam> poolContainerParams = new();
            if (_poolContainerParams != null) poolContainerParams.AddRange(_poolContainerParams);
            var config = Resources.Load<PoolContainerConfig>("PoolContainerConfig");
            if (config != null) poolContainerParams.AddRange(config.PoolContainerParams);
            foreach (var _param in poolContainerParams)
            {
                _param.dectivatedParent = poolDeActiveRoot;
                _param.tempPoolables = tempPoolable;
                _param.poolModule = this;
            }
            AddPools(poolContainerParams);
            return this;
        }

        public void OnUpdate(float _deltaTime)
        {
            foreach (var pool in pools)
            {
                pool.Value.OnUpdate(_deltaTime);
            }
        }

        public void ClearAllContainerElments()
        {
            foreach (var pool in pools)
            {
                ClearContainerElments(pool.Key);
            }
        }

        public void ClearAllContainerElements()
        {
            ClearAllContainerElments();
        }

        public void ClearContainerElments(GameObject _prefab)
        {
            if (_prefab == null)
            {
                Logger.LogPoolWarning("Prefab is null");
                return;
            }

            if (pools.TryGetValue(_prefab, out var pool))
            {
                pool.DespawnAll();
            }
        }

        public void ClearContainerElements(GameObject _prefab)
        {
            ClearContainerElments(_prefab);
        }

        public GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            if (_prefab == null)
            {
                Logger.LogPoolWarning("Prefab is null");
                return null;
            }

            if (!pools.TryGetValue(_prefab, out var pool))
            {
                AddPool(_prefab);
                pools.TryGetValue(_prefab, out pool);
            }

            return pool?.Spawn(localPosition, localRotation, localScale, parent, worldPositionStays);
        }

        public GameObject Pop(GameObject _prefab)
        {
            return Pop(_prefab, Vector3.zero, Quaternion.identity, Vector3.one, null, false);
        }

        public GameObject Pop(GameObject _prefab, Transform parent, bool worldPositionStays = false)
        {
            return Pop(_prefab, Vector3.zero, Quaternion.identity, Vector3.one, parent, worldPositionStays);
        }

        public GameObject Pop(GameObject _prefab, Vector3 localPosition)
        {
            return Pop(_prefab, localPosition, Quaternion.identity, Vector3.one, null, false);
        }

        public GameObject Pop(GameObject _prefab, Vector3 localPosition, Transform parent,
            bool worldPositionStays = false)
        {
            return Pop(_prefab, localPosition, Quaternion.identity, Vector3.one, parent, worldPositionStays);
        }

        public GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation)
        {
            return Pop(_prefab, localPosition, localRotation, Vector3.one, null, false);
        }

        public GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Transform parent, bool worldPositionStays = false)
        {
            return Pop(_prefab, localPosition, localRotation, Vector3.one, parent, worldPositionStays);
        }

        public GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale)
        {
            return Pop(_prefab, localPosition, localRotation, localScale, null, false);
        }

        public GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent)
        {
            return Pop(_prefab, localPosition, localRotation, localScale, parent, false);
        }

        public void Push(GameObject _Obj, float _delay = 0f)
        {
            TryPush(_Obj, _delay);
        }

        public bool TryPush(GameObject _Obj, float _delay = 0f)
        {
            if (_Obj == null)
            {
                Logger.LogPoolWarning($"Object is null");
                return false;
            }
            if (cloneToPoolMap.TryGetValue(_Obj, out var prefab))
            {
                if (pools.TryGetValue(prefab, out var pool))
                {
                    pool.Despawn(_Obj, _delay);
                    return true;
                }
                else
                {
                    Logger.LogPoolWarning($"Pool for {prefab.name} not found");
                    return false;
                }
            }
            else
            {
                Logger.LogPoolWarning($"Object {_Obj.name} not found in cloneToPoolMap");
                return false;
            }
        }

        public void AddPool(GameObject _prefab)
        {
            if (_prefab == null)
            {
                Logger.LogPoolWarning("Prefab is null");
                return;
            }

            if (!pools.ContainsKey(_prefab))
            {
                PoolContainerParam m_param = new(
                    this,
                    _prefab,
                    tempPoolable,
                    PushStrategyType.ActivateAndDeactivate,
                    poolDeActiveRoot,
                    NotificationType.BroadcastIPoolable,
                    100,
                    false,
                    false,
                    true,
                    true
                );
                AddPool(_prefab, m_param);
            }
            return;
        }

        public void AddPools(List<PoolContainerParam> poolContainerParams)
        {
            if (poolContainerParams == null)
            {
                return;
            }

            foreach (var param in poolContainerParams)
            {
                if (param == null || param.prefab == null)
                {
                    Logger.LogPoolWarning("PoolContainerParam or prefab is null");
                    continue;
                }

                AddPool(param.prefab, param);
            }
        }

        public void AddPool(GameObject _prefab, PoolContainerParam _poolContainerParam)
        {
            if (_prefab == null)
            {
                Logger.LogPoolWarning("Prefab is null");
                return;
            }

            if (!pools.ContainsKey(_prefab))
            {
                if (_poolContainerParam == null)
                {
                    AddPool(_prefab);
                    return;
                }

                _poolContainerParam.poolModule = this;
                _poolContainerParam.prefab = _prefab;
                _poolContainerParam.dectivatedParent = poolDeActiveRoot;
                _poolContainerParam.tempPoolables = tempPoolable;

                var contain = new PoolContainer().Init(_poolContainerParam);
                pools.Add(_prefab, contain);
            }
            return;
        }

        public void RemovePool(GameObject _prefab)
        {
            if (_prefab == null)
            {
                Logger.LogPoolWarning("Prefab is null");
                return;
            }

            if (pools.TryGetValue(_prefab, out var pool))
            {
                pool.DestroyAll();
                pools.Remove(_prefab);
                RemoveObjToPrefabMap(_prefab);
            }
            return;
        }

        public void AddObjToPrefabMap(GameObject _obj, GameObject _prefab)
        {
            if (_obj == null || _prefab == null)
            {
                Logger.LogPoolWarning("Object or prefab is null");
                return;
            }

            cloneToPoolMap[_obj] = _prefab;
        }

        private void RemoveObjToPrefabMap(GameObject _prefab)
        {
            var removeKeys = new List<GameObject>();
            foreach (var item in cloneToPoolMap)
            {
                if (item.Value == _prefab)
                {
                    removeKeys.Add(item.Key);
                }
            }

            foreach (var key in removeKeys)
            {
                cloneToPoolMap.Remove(key);
            }
        }
    }
}
