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
            pools.Clear();
            cloneToPoolMap.Clear();
            tempPoolable.Clear();
            poolRoot = new GameObject("PoolRoot").transform;
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

        public void ClearContainerElments(GameObject _prefab)
        {
            if (pools.TryGetValue(_prefab, out var pool))
            {
                pool.DespawnAll();
            }
        }

        public GameObject Pop(GameObject _prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            if (pools.TryGetValue(_prefab, out var pool))
            {
                return pool.Spawn(localPosition, localRotation, localScale, parent, worldPositionStays);
            }
            else
            {
                AddPool(_prefab);
                return Pop(_prefab, localPosition, localRotation, localScale, parent, worldPositionStays);
            }
        }

        public void Push(GameObject _Obj, float _delay = 0f)
        {
            if (_Obj == null)
            {
                Logger.LogPoolWarning($"Object is null");
                return;
            }
            if (cloneToPoolMap.TryGetValue(_Obj, out var prefab))
            {
                if (pools.TryGetValue(prefab, out var pool))
                {
                    pool.Despawn(_Obj, _delay);
                }
                else
                {
                    Logger.LogPoolWarning($"Pool for {prefab.name} not found");
                    return;
                }
            }
            else
            {
                Logger.LogPoolWarning($"Object {_Obj.name} not found in cloneToPoolMap");
            }
        }

        public void AddPool(GameObject _prefab)
        {
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
            foreach (var param in poolContainerParams)
            {
                AddPool(param.prefab, param);
            }
        }

        public void AddPool(GameObject _prefab, PoolContainerParam _poolContainerParam)
        {
            if (!pools.ContainsKey(_prefab))
            {
                var contain = new PoolContainer().Init(_poolContainerParam);
                pools.Add(_prefab, contain);
            }
            return;
        }

        public void RemovePool(GameObject _prefab)
        {
            if (pools.ContainsKey(_prefab))
            {
                pools.Remove(_prefab);
            }
            return;
        }

        public void AddObjToPrefabMap(GameObject _obj, GameObject _prefab)
        {
            if (!cloneToPoolMap.ContainsKey(_obj))
            {
                cloneToPoolMap.Add(_obj, _prefab);
            }
            else
            {
                cloneToPoolMap[_obj] = _prefab;
                Logger.LogPoolWarning($"Object {_obj.name} already exists in cloneToPoolMap");
            }
        }
    }
}
