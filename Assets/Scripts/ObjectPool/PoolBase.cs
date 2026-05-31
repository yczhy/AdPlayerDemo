using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    public abstract class PoolBase<T> where T : class, IPoolable, new()
    {
        

        /// <summary>
        /// 当前这个池已经生成过多少个元素
        /// </summary>
        protected int spawnCount = 0;

        /// <summary>
        /// 下一个生成对象的编号
        /// </summary>
        public int NextIndex => spawnCount;

        public int PoolIndex { get => spawnCount; set => spawnCount = value; }

        protected bool inPool = false;
        public bool InPool => inPool;

        public virtual void DeSpawn()
        {
        }

        public virtual T Spawn()
        {
            int index = spawnCount;
            spawnCount++;

            return OnSpawn(index);
        }

        /// <summary>
        /// 子类实现具体生成逻辑，并拿到这是第几个生成的元素
        /// </summary>
        protected abstract T OnSpawn(int index);
    }
}