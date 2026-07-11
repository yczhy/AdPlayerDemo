using System;
using UnityEngine;

namespace Duskvern
{
    public interface IEventCallback
    {
        bool IsEmpty();
        void Clear();
        void Release();
    }

    public abstract class EventCallbackBase : IEventCallback, IPoolable
    {
        public abstract void Clear();

        public abstract bool IsEmpty();

        public abstract void OnDeSpawn();

        public abstract void OnSpawn();

        public abstract void Release();
    }

    public class EventCallback_0 : EventCallbackBase
    {
        private Action action; public Action GetAction => action;

        public override void Clear() => action = null;

        public override bool IsEmpty() => action == null;

        public override void OnDeSpawn()
        {
            Clear();
        }

        public override void OnSpawn()
        {
            Clear();
        }

        public void Trigger() => action?.Invoke();
        public void Add(Action action) => this.action += action;
        public void Remove(Action action) => this.action -= action;

        public override void Release() => ClassPool<EventCallback_0>.Push(this);
    }

    public class EventCallback_1<T> : EventCallbackBase
    {
        private Action<T> action; public Action<T> GetAction => action;

        public override void Clear() => action = null;

        public override bool IsEmpty() => action == null;

        public override void OnDeSpawn()
        {
            Clear();
        }

        public override void OnSpawn()
        {
            Clear();
        }

        public void Trigger(T t) => action?.Invoke(t);
        public void Add(Action<T> action) => this.action += action;
        public void Remove(Action<T> action) => this.action -= action;
        public override void Release() => ClassPool<EventCallback_1<T>>.Push(this);
    }

    public class EventCallback_2<T, V> : EventCallbackBase
    {
        private Action<T, V> action; public Action<T, V> GetAction => action;

        public override void Clear() => action = null;

        public override bool IsEmpty() => action == null;

        public override void OnDeSpawn()
        {
            Clear();
        }

        public override void OnSpawn()
        {
            Clear();
        }

        public void Trigger(T t, V v) => action?.Invoke(t, v);
        public void Add(Action<T, V> action) => this.action += action;
        public void Remove(Action<T, V> action) => this.action -= action;
        public override void Release() => ClassPool<EventCallback_2<T, V>>.Push(this);
    }

    public class EventCallback_3<T, V, G> : EventCallbackBase
    {
        private Action<T, V, G> action; public Action<T, V, G> GetAction => action;

        public override void Clear() => action = null;

        public override bool IsEmpty() => action == null;

        public override void OnDeSpawn()
        {
            Clear();
        }

        public override void OnSpawn()
        {
            Clear();
        }

        public void Trigger(T t, V v, G g) => action?.Invoke(t, v, g);
        public void Add(Action<T, V, G> action) => this.action += action;
        public void Remove(Action<T, V, G> action) => this.action -= action;
        public override void Release() => ClassPool<EventCallback_3<T, V, G>>.Push(this);
    }

    public class EventCallback_4<T, V, G, K> : EventCallbackBase
    {
        private Action<T, V, G, K> action; public Action<T, V, G, K> GetAction => action;

        public override void Clear() => action = null;

        public override bool IsEmpty() => action == null;

        public override void OnDeSpawn()
        {
            Clear();
        }

        public override void OnSpawn()
        {
            Clear();
        }

        public void Trigger(T t, V v, G g, K k) => action?.Invoke(t, v, g, k);
        public void Add(Action<T, V, G, K> action) => this.action += action;
        public void Remove(Action<T, V, G, K> action) => this.action -= action;
        public override void Release() => ClassPool<EventCallback_4<T, V, G, K>>.Push(this);
    }

    public class EventCallback_5<T, V, G, K, N> : EventCallbackBase
    {
        private Action<T, V, G, K, N> action; public Action<T, V, G, K, N> GetAction => action;

        public override void Clear() => action = null;

        public override bool IsEmpty() => action == null;

        public override void OnDeSpawn()
        {
            Clear();
        }

        public override void OnSpawn()
        {
            Clear();
        }

        public void Trigger(T t, V v, G g, K k, N n) => action?.Invoke(t, v, g, k, n);
        public void Add(Action<T, V, G, K, N> action) => this.action += action;
        public void Remove(Action<T, V, G, K, N> action) => this.action -= action;
        public override void Release() => ClassPool<EventCallback_5<T, V, G, K, N>>.Push(this);
    }
}

