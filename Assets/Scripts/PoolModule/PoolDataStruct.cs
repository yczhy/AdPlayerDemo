using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    public enum PushStrategyType
    {
        ActivateAndDeactivate,
        DeactivateViaHierarchy
    }

    public enum NotificationType
    {
        BroadcastIPoolable, // 广播IPoolable接口
        AloneIPoolable, // 单独调用IPoolable接口
    }

    [Serializable]
    public class PoolContainerParam : IPoolable
    {
        public GameObject prefab;
        public Transform dectivatedParent; // 只有 PushStrategyType 为 DeactivateViaHierarchy 时有效
        public PushStrategyType pushStrategyType;
        public NotificationType notificationType;
        public int capacity = 100;
        public bool recycle = false;
        public bool persistent = false;
        public bool stamp = true;
        public bool warnings = true;
        public List<IPoolable> tempPoolables;
        public PoolModule poolModule;

        public PoolContainerParam(PoolModule _poolModule, GameObject _prefab, List<IPoolable> _tempPoolables, PushStrategyType _pushStrategyType, Transform _dectivatedParent, NotificationType _notificationType, int _capacity = 100, bool _recycle = false, bool _persistent = false, bool _stamp = true, bool _warnings = true)
        {
            poolModule = _poolModule;
            prefab = _prefab;
            dectivatedParent = _dectivatedParent;
            pushStrategyType = _pushStrategyType;
            notificationType = _notificationType;
            capacity = _capacity;
            recycle = _recycle;
            persistent = _persistent;
            stamp = _stamp;
            warnings = _warnings;
            tempPoolables = _tempPoolables;
        }

        public void OnDeSpawn()
        {
            prefab = null;
            dectivatedParent = null;
            pushStrategyType = PushStrategyType.ActivateAndDeactivate;
            notificationType = NotificationType.BroadcastIPoolable;
            capacity = 100;
            recycle = false;
            persistent = false;
            stamp = true;
            warnings = true;
            tempPoolables = null;
        }

        public void OnSpawn()
        {
            
        }
    }
}