using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duskvern
{
    public class PoolContainer
    {
        private GameObject prefab; // 对象池的预制件
        public GameObject Prefab
        {
            get => prefab;
            set
            {
                if (value == null)
                {
                    if (Warnings) Logger.LogPoolWarning($"Prefab is null");
                    return;
                }
                if (value != null && value.GetComponent<IPoolable>() == null)
                {
                    if (Warnings) Logger.LogPoolWarning($"Prefab {value.name} does not implement IPoolable interface");
                    return;
                }
                prefab = value;
            }
        }
        public string Name => prefab.name; // 对象池的名称

        private Transform deactivatedTransform; // 存放已回收对象的父级对象 --- 将物体直接放到这个对象下进行隐藏，避免Setactive(false)带来的性能问题
        public Transform DeactivatedTransform => deactivatedTransform;

        private PushStrategyType pushStrategyType; // 回收策略
        public PushStrategyType PushStrategyType => pushStrategyType;

        private NotificationType notificationType; // 回收通知类型
        public NotificationType NotificationType => notificationType;

        private int capacity; // 对象池容量
        public int Capacity => capacity;

        public bool recycle; // 是否允许回收
        public bool Recycle => recycle;

        private bool persistent; // 是否持久化
        public bool Persistent => persistent;

        private bool stamp; // 是否标记索引
        public bool Stamp => stamp;

        private bool warnings; // 是否显示警告
        public bool Warnings => warnings;

        private List<GameObject> spawnedClonesList = new List<GameObject>(); // 已生成的对象列表 -- 方便知道对象的生成顺序

        private List<GameObject> deSpawnedClonesList = new List<GameObject>(); // 记录已经收回的对象

        private List<Delay> delays = new List<Delay>(); // 延迟回收列表

        public int DespawnedCount => deSpawnedClonesList.Count; // 已回收对象数量
        public int SpawnedCount => spawnedClonesList.Count; // 已生成对象数量
        public int TotalCount => DespawnedCount + SpawnedCount; // 总数量

        private List<IPoolable> TempPoolables;

        private PoolModule poolModule;

        public PoolContainer Init(PoolContainerParam poolContainerParam)
        {
            poolModule = poolContainerParam.poolModule;
            Prefab = poolContainerParam.prefab;
            pushStrategyType = poolContainerParam.pushStrategyType;
            notificationType = poolContainerParam.notificationType;
            capacity = poolContainerParam.capacity;
            recycle = poolContainerParam.recycle;
            persistent = poolContainerParam.persistent;
            stamp = poolContainerParam.stamp;
            warnings = poolContainerParam.warnings;
            TempPoolables = poolContainerParam.tempPoolables;
            deactivatedTransform = poolContainerParam.dectivatedParent;
            return this;
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = delays.Count - 1; i >= 0; i--)
            {
                var delay = delays[i];
                delay.delayTime -= deltaTime;
                if (delay.delayTime <= 0)
                {
                    TryDespawn(delay.clone);
                    delays.RemoveAt(i);
                    ClassPool<Delay>.Push(delay);
                }
            }
        }

        public GameObject Spawn(Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            var clone = default(GameObject);
            TrySpawn(ref clone, localPosition, localRotation, localScale, parent, worldPositionStays);
            return clone;
        }

        public bool TrySpawn(ref GameObject clone, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            if (Prefab == null)
            {
                if (Warnings) Logger.LogPoolWarning($"Prefab is null, pool name: {Name}");
                return false;
            }

            // 先看可不可以从 回收池 中取出对象
            for (int i = deSpawnedClonesList.Count - 1; i >= 0; i--)
            {
                clone = deSpawnedClonesList[i];
                deSpawnedClonesList.RemoveAt(i);

                if (clone == null)
                {
                    if (Warnings) Logger.LogPoolWarning($"DeSpawnedClonesList Clone is null, pool name: {Name}");
                    continue;
                }

                SpawnClone(clone, localPosition, localRotation, localScale, parent, worldPositionStays);
                return true;
            }

            // 如果回收池中没有对象，则生成新的对象

            // 先看是否 在对象池容量内
            if (TotalCount < Capacity || Capacity <= 0)
            {
                // 先创建一个
                clone = CreateClone(localPosition, localRotation, localScale, parent, worldPositionStays);
                spawnedClonesList.Add(clone);
                if (pushStrategyType == PushStrategyType.ActivateAndDeactivate)
                {
                    clone.SetActive(true);
                }

                NotifyOnSpawn(clone);
                return true;
            }

            // 如果对象池达到最大上限，看是否允许循环回收
            if (recycle && TryDespawnOldest(ref clone))
            {
                SpawnClone(clone, localPosition, localRotation, localScale, parent, worldPositionStays);
                return true;
            }

            return false;
        }

        private void SpawnClone(GameObject clone, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            var cloneTransform = clone.transform;
            cloneTransform.SetParent(null, false);
            cloneTransform.localPosition = localPosition;
            cloneTransform.localRotation = localRotation;
            cloneTransform.localScale = localScale;
            cloneTransform.SetParent(parent, worldPositionStays);

            if (parent == null)
            {
                SceneManager.MoveGameObjectToScene(clone, SceneManager.GetActiveScene());
            }

            if (PushStrategyType == PushStrategyType.ActivateAndDeactivate)
            {
                clone.SetActive(true);
            }

            spawnedClonesList.Add(clone);
            NotifyOnSpawn(clone);
        }

        private bool TryDespawnOldest(ref GameObject clone)
        {
            if (spawnedClonesList.Count <= 0)
            {
                if (warnings) Logger.LogPoolWarning($"SpawnedClonesList is empty, pool name: {Name}");
                return false;
            }
            var _clone = spawnedClonesList[0];
            spawnedClonesList.RemoveAt(0);
            if (_clone == null)
            {
                if (warnings) Logger.LogPoolWarning($"SpawnedClonesList Clone is null, pool name: {Name}");
                return false;
            }
            NotifyOnDespawn(_clone);
            clone = _clone;
            return true;
        }

        private GameObject CreateClone(Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
            var clone = DoInstantiate(Prefab, localPosition, localRotation, localScale, parent, worldPositionStays);
            clone.name = stamp ? $"{Prefab.name}_({spawnedClonesList.Count})" : Prefab.name;
            poolModule.AddObjToPrefabMap(clone, Prefab);
            return clone;
        }

        private void NotifyOnSpawn(GameObject clone)
        {
            GetPoolableComponents(clone);
            foreach (var poolable in TempPoolables)
            {
                poolable.OnSpawn();
            }
        }

        private void NotifyOnDespawn(GameObject clone)
        {
            GetPoolableComponents(clone);
            foreach (var poolable in TempPoolables)
            {
                poolable.OnDeSpawn();
            }
        }

        private void GetPoolableComponents(GameObject clone)
        {
            if (clone == null)
            {
                if (warnings) Logger.LogPoolWarning($"Clone is null, pool name: {Name}");
                return;
            }

            TempPoolables.Clear();
            switch (notificationType)
            {
                case NotificationType.AloneIPoolable:
                    clone.GetComponents(TempPoolables);
                    break;
                case NotificationType.BroadcastIPoolable:
                    clone.GetComponentsInChildren(TempPoolables);
                    break;
            }
        }

        private GameObject DoInstantiate(GameObject prefab, Vector3 localPosition, Quaternion localRotation,
            Vector3 localScale, Transform parent, bool worldPositionStays)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false && UnityEditor.PrefabUtility.IsPartOfRegularPrefab(prefab) == true)
            {
                if (worldPositionStays == true)
                {
                    return (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent);
                }
                else
                {
                    var clone = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent);

                    clone.transform.localPosition = localPosition;
                    clone.transform.localRotation = localRotation;
                    clone.transform.localScale = localScale;

                    return clone;
                }
            }
#endif

            if (worldPositionStays == true)
            {
                return GameObject.Instantiate(prefab, parent);
            }
            else
            {
                var clone = GameObject.Instantiate(prefab, localPosition, localRotation, parent);
                clone.transform.localPosition = localPosition;
                clone.transform.localRotation = localRotation;
                clone.transform.localScale = localScale;
                return clone;
            }
        }

        public void Despawn(GameObject clone, float delayTime = 0f)
        {
            if (clone == null)
            {
                if (warnings) Logger.LogPoolWarning($"Clone is null, pool name: {Name}");
                return;
            }

            if (delayTime > 0f)
            {
                DespawnWithDelay(clone, delayTime);
            }
            else
            {
                TryDespawn(clone);
                for (var i = delays.Count - 1; i >= 0; i--)
                {
                    var delay = delays[i];
                    if (delay.clone == clone)
                    {
                        delays.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        // 这里负责更新延时销毁的时间，如果对同一物体多次进行延迟销毁，取最快销毁的那一次
        private void DespawnWithDelay(GameObject clone, float delayTime)
        {
            for (var i = delays.Count - 1; i >= 0; i--)
            {
                var delay = delays[i];
                if (delay.clone == clone)
                {
                    if (delayTime < delay.delayTime)
                    {
                        delay.delayTime = delayTime;
                    }
                    return;
                }
            }

            var newDelay = ClassPool<Delay>.Pop();
            newDelay.clone = clone;
            newDelay.delayTime = delayTime;
            delays.Add(newDelay);
        }

        private bool TryDespawn(GameObject clone)
        {
            if (clone == null)
            {
                if (warnings) Logger.LogPoolWarning($"Clone is null, pool name: {Name}");
                return false;
            }

            if (deSpawnedClonesList.Contains(clone))
            {
                if (warnings) Logger.LogPoolWarning($"Clone already despawned, pool name: {Name}");
                return false;
            }

            if (spawnedClonesList.Remove(clone))
            {
                DespawnNow(clone);
                return true;
            }
            else
            {
                if (warnings) Logger.LogPoolWarning($"Clone not found in pool, pool name: {Name}");
            } 
            return false;
        }

        private void DespawnNow(GameObject clone)
        {
            deSpawnedClonesList.Add(clone);
            NotifyOnDespawn(clone);

            if (pushStrategyType == PushStrategyType.ActivateAndDeactivate)
            {
                clone.SetActive(false);
            }
            else
            {
                clone.transform.SetParent(DeactivatedTransform);
            }
        }

        // 回收所有的元素，包括正在延时的
        public void DespawnAll()
        {
            for (var i = spawnedClonesList.Count - 1; i >= 0; i--)
            {
                var clone = spawnedClonesList[i];
                if (clone != null)
                {
                    DespawnNow(clone);
                }
            }
            spawnedClonesList.Clear();

            for (var i = delays.Count - 1; i >= 0; i--)
            {
                var delay = delays[i];
                if (delay.clone != null)
                {
                    DespawnNow(delay.clone);
                    ClassPool<Delay>.Push(delay);
                }
            }
            delays.Clear();
        }
    }
}

