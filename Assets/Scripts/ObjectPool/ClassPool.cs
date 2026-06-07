using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    public class ClassPool<T> where T : class, IPoolable, new()
    {
        private static int maxValue = 50;

        public static int MaxValue { get => maxValue; set => maxValue = value; }
        private static List<T> cache = new List<T>();

        public static T Pop()
        {
            var count = cache.Count;
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
            instance.OnSpawn();
            return instance;
        }

        public static void Push(T instance)
        {
            if (instance == null)
            {
                Logger.LogWarning(LogType.PoolLog, "放回对象池时：Instance is null");
                return;
            }

            if (cache.Contains(instance))
            {
                Logger.LogWarning(LogType.PoolLog, "对象池中已存在该对象 " + typeof(T).Name);
                return;
            }

            if (cache.Count >= maxValue)
            {
                Logger.LogWarning(LogType.PoolLog, "对象池已满，无法放回对象 进行销毁" + typeof(T).Name);
                instance.OnDeSpawn();
                return;
            }

            instance.OnDeSpawn();
            cache.Add(instance);
        }
    }
}
