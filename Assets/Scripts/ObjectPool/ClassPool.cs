using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    public class ClassPool<T> where T : class, IPoolable, new()
    {
        private static int maxValue = 50;

        public static int MaxValue { get => maxValue; set => maxValue = value; }
        private static List<T> cache = new List<T>();

        public static T Get()
        {
            var count = cache.Count;
            if (count > maxValue)
            {
                Logger.LogWarning(LogType.PoolLog, "对象池已满，无法获取对象 " + typeof(T).Name);
                return null;
            }
            T instance = null;
            var index = count - 1;
            if (index >= 0)
            {
                instance = cache[index];
                cache.RemoveAt(index);
            }
            else
            {
                instance = new T();
            }
            instance.InPool = false;
            instance.Spawn();
            return instance;
        }

        public static void Release(T instance)
        {
            if (instance == null)
            {
                Logger.LogWarning(LogType.PoolLog, "放回对象池时：Instance is null");
                return;
            }

            if (cache.Count > maxValue)
            {
                Logger.LogWarning(LogType.PoolLog, "对象池已满，无法放回对象 进行销毁" + typeof(T).Name);
                instance.DeSpawn();
                return;
            }

            instance.InPool = true;
            instance.DeSpawn();
            cache.Add(instance);
        }
    }
}

