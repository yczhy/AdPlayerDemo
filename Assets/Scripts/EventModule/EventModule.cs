using System;
using System.Collections.Generic;
using System.Linq;

namespace Duskvern
{
    public enum EventScope
    {
        All = 0,
        Local = 1,
        Global = 2,
    }


    public static class EventModule
    {
        private static readonly bool warning = false;


        private static readonly Dictionary<int, IEventCallback> LocalEventHandlers = new();

        private static readonly Dictionary<int, IEventCallback> GlobalEventHandlers = new();



        #region Clear


        public static void Clear(EventScope type)
        {
            switch (type)
            {
                case EventScope.Global:
                    ClearMap(GlobalEventHandlers);
                    break;


                case EventScope.Local:
                    ClearMap(LocalEventHandlers);
                    break;


                case EventScope.All:
                    ClearMap(GlobalEventHandlers);
                    ClearMap(LocalEventHandlers);
                    break;
            }
        }



        private static void ClearMap(Dictionary<int,IEventCallback> map)
        {
            foreach(var callback in map.Values)
            {
                callback?.Clear();
                callback?.Release();
            }
            map.Clear();
        }


        #endregion




        #region Add Global



        public static void AddG(EventDefinition definition, Action callback)
        {
            Add(GlobalEventHandlers, definition.ID, callback);
        }



        public static void AddG<T>(
            EventDefinition<T> definition,
            Action<T> callback)
        {
            Add(GlobalEventHandlers, definition.ID, callback);
        }



        public static void AddG<T1,T2>(
            EventDefinition<T1,T2> definition,
            Action<T1,T2> callback)
        {
            Add(GlobalEventHandlers, definition.ID, callback);
        }



        public static void AddG<T1,T2,T3>(
            EventDefinition<T1,T2,T3> definition,
            Action<T1,T2,T3> callback)
        {
            Add(GlobalEventHandlers, definition.ID, callback);
        }



        public static void AddG<T1,T2,T3,T4>(
            EventDefinition<T1,T2,T3,T4> definition,
            Action<T1,T2,T3,T4> callback)
        {
            Add(GlobalEventHandlers, definition.ID, callback);
        }



        public static void AddG<T1,T2,T3,T4,T5>(
            EventDefinition<T1,T2,T3,T4,T5> definition,
            Action<T1,T2,T3,T4,T5> callback)
        {
            Add(GlobalEventHandlers, definition.ID, callback);
        }


        #endregion





        #region Add Local



        public static void AddL(EventDefinition definition, Action callback)
        {
            Add(LocalEventHandlers, definition.ID, callback);
        }



        public static void AddL<T>(
            EventDefinition<T> definition,
            Action<T> callback)
        {
            Add(LocalEventHandlers, definition.ID, callback);
        }



        public static void AddL<T1,T2>(
            EventDefinition<T1,T2> definition,
            Action<T1,T2> callback)
        {
            Add(LocalEventHandlers, definition.ID, callback);
        }



        public static void AddL<T1,T2,T3>(
            EventDefinition<T1,T2,T3> definition,
            Action<T1,T2,T3> callback)
        {
            Add(LocalEventHandlers,definition.ID,callback);
        }



        public static void AddL<T1,T2,T3,T4>(
            EventDefinition<T1,T2,T3,T4> definition,
            Action<T1,T2,T3,T4> callback)
        {
            Add(LocalEventHandlers,definition.ID,callback);
        }



        public static void AddL<T1,T2,T3,T4,T5>(
            EventDefinition<T1,T2,T3,T4,T5> definition,
            Action<T1,T2,T3,T4,T5> callback)
        {
            Add(LocalEventHandlers,definition.ID,callback);
        }


        #endregion






        #region Trigger Global



        public static void TriggerG(EventDefinition definition)
        {
            Trigger(GlobalEventHandlers,definition.ID);
        }



        public static void TriggerG<T>(
            EventDefinition<T> definition,
            T value)
        {
            Trigger(GlobalEventHandlers,definition.ID,value);
        }



        public static void TriggerG<T1,T2>(
            EventDefinition<T1,T2> definition,
            T1 t1,
            T2 t2)
        {
            Trigger(GlobalEventHandlers,definition.ID,t1,t2);
        }



        public static void TriggerG<T1,T2,T3>(
            EventDefinition<T1,T2,T3> definition,
            T1 t1,
            T2 t2,
            T3 t3)
        {
            Trigger(GlobalEventHandlers,definition.ID,t1,t2,t3);
        }



        public static void TriggerG<T1,T2,T3,T4>(
            EventDefinition<T1,T2,T3,T4> definition,
            T1 t1,
            T2 t2,
            T3 t3,
            T4 t4)
        {
            Trigger(GlobalEventHandlers,definition.ID,t1,t2,t3,t4);
        }



        public static void TriggerG<T1,T2,T3,T4,T5>(
            EventDefinition<T1,T2,T3,T4,T5> definition,
            T1 t1,
            T2 t2,
            T3 t3,
            T4 t4,
            T5 t5)
        {
            Trigger(GlobalEventHandlers,definition.ID,t1,t2,t3,t4,t5);
        }


        #endregion




        #region Trigger Local


        public static void TriggerL(EventDefinition definition)
        {
            Trigger(LocalEventHandlers,definition.ID);
        }



        public static void TriggerL<T>(
            EventDefinition<T> definition,
            T value)
        {
            Trigger(LocalEventHandlers,definition.ID,value);
        }



        public static void TriggerL<T1,T2>(
            EventDefinition<T1,T2> definition,
            T1 t1,
            T2 t2)
        {
            Trigger(LocalEventHandlers,definition.ID,t1,t2);
        }



        public static void TriggerL<T1,T2,T3>(
            EventDefinition<T1,T2,T3> definition,
            T1 t1,
            T2 t2,
            T3 t3)
        {
            Trigger(LocalEventHandlers,definition.ID,t1,t2,t3);
        }



        public static void TriggerL<T1,T2,T3,T4>(
            EventDefinition<T1,T2,T3,T4> definition,
            T1 t1,
            T2 t2,
            T3 t3,
            T4 t4)
        {
            Trigger(LocalEventHandlers,definition.ID,t1,t2,t3,t4);
        }



        public static void TriggerL<T1,T2,T3,T4,T5>(
            EventDefinition<T1,T2,T3,T4,T5> definition,
            T1 t1,
            T2 t2,
            T3 t3,
            T4 t4,
            T5 t5)
        {
            Trigger(LocalEventHandlers,definition.ID,t1,t2,t3,t4,t5);
        }


        #endregion
                #region Remove Global


        public static void RemoveG(EventDefinition definition, Action callback)
        {
            Remove(GlobalEventHandlers, definition.ID, callback);
        }


        public static void RemoveG<T>(EventDefinition<T> definition, Action<T> callback)
        {
            Remove(GlobalEventHandlers, definition.ID, callback);
        }


        public static void RemoveG<T1,T2>(EventDefinition<T1,T2> definition, Action<T1,T2> callback)
        {
            Remove(GlobalEventHandlers, definition.ID, callback);
        }


        public static void RemoveG<T1,T2,T3>(EventDefinition<T1,T2,T3> definition, Action<T1,T2,T3> callback)
        {
            Remove(GlobalEventHandlers, definition.ID, callback);
        }


        public static void RemoveG<T1,T2,T3,T4>(EventDefinition<T1,T2,T3,T4> definition, Action<T1,T2,T3,T4> callback)
        {
            Remove(GlobalEventHandlers, definition.ID, callback);
        }


        public static void RemoveG<T1,T2,T3,T4,T5>(EventDefinition<T1,T2,T3,T4,T5> definition, Action<T1,T2,T3,T4,T5> callback)
        {
            Remove(GlobalEventHandlers, definition.ID, callback);
        }


        #endregion



        #region Remove Local


        public static void RemoveL(EventDefinition definition, Action callback)
        {
            Remove(LocalEventHandlers, definition.ID, callback);
        }


        public static void RemoveL<T>(EventDefinition<T> definition, Action<T> callback)
        {
            Remove(LocalEventHandlers, definition.ID, callback);
        }


        public static void RemoveL<T1,T2>(EventDefinition<T1,T2> definition, Action<T1,T2> callback)
        {
            Remove(LocalEventHandlers, definition.ID, callback);
        }


        public static void RemoveL<T1,T2,T3>(EventDefinition<T1,T2,T3> definition, Action<T1,T2,T3> callback)
        {
            Remove(LocalEventHandlers, definition.ID, callback);
        }


        public static void RemoveL<T1,T2,T3,T4>(EventDefinition<T1,T2,T3,T4> definition, Action<T1,T2,T3,T4> callback)
        {
            Remove(LocalEventHandlers, definition.ID, callback);
        }


        public static void RemoveL<T1,T2,T3,T4,T5>(EventDefinition<T1,T2,T3,T4,T5> definition, Action<T1,T2,T3,T4,T5> callback)
        {
            Remove(LocalEventHandlers, definition.ID, callback);
        }


        #endregion



        #region Core Add


        private static void Add(
            Dictionary<int,IEventCallback> map,
            int id,
            Action callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_0 cb)
                {
                    #if DEBUG
                    var events = cb.GetAction.GetInvocationList().Contains(callback);
                    if (events) Logger.LogError("EventModule", "重复订阅事件");
                    #endif
                    cb.Add(callback);
                }
                else
                {
                    Error("Add",id);
                }
            }
            else
            {
                var cb = ClassPool<EventCallback_0>.Pop();
                cb.Add(callback);
                map.Add(id,cb);
            }
        }



        private static void Add<T>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_1<T> cb)
                {
                    #if DEBUG
                    var events = cb.GetAction.GetInvocationList().Contains(callback);
                    if (events) Logger.LogError("EventModule", "重复订阅事件");
                    #endif
                    cb.Add(callback);
                }
                else
                {
                    Error("Add",id);
                }
            }
            else
            {
                var cb = ClassPool<EventCallback_1<T>>.Pop();
                cb.Add(callback);
                map.Add(id,cb);
            }
        }



        private static void Add<T1,T2>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T1,T2> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_2<T1,T2> cb)
                {
                    #if DEBUG
                    var events = cb.GetAction.GetInvocationList().Contains(callback);
                    if (events) Logger.LogError("EventModule", "重复订阅事件");
                    #endif
                    cb.Add(callback);
                }
                else
                {
                    Error("Add",id);
                }
            }
            else
            {
                var cb = ClassPool<EventCallback_2<T1,T2>>.Pop();
                cb.Add(callback);
                map.Add(id,cb);
            }
        }



        private static void Add<T1,T2,T3>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T1,T2,T3> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_3<T1,T2,T3> cb)
                {
                    #if DEBUG
                    var events = cb.GetAction.GetInvocationList().Contains(callback);
                    if (events) Logger.LogError("EventModule", "重复订阅事件");
                    #endif
                    cb.Add(callback);
                }
                else
                {
                    Error("Add",id);
                }
            }
            else
            {
                var cb = ClassPool<EventCallback_3<T1,T2,T3>>.Pop();
                cb.Add(callback);
                map.Add(id,cb);
            }
        }



        private static void Add<T1,T2,T3,T4>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T1,T2,T3,T4> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_4<T1,T2,T3,T4> cb)
                {
                    #if DEBUG
                    var events = cb.GetAction.GetInvocationList().Contains(callback);
                    if (events) Logger.LogError("EventModule", "重复订阅事件");
                    #endif
                    cb.Add(callback);
                }
                else
                {
                    Error("Add",id);
                }
            }
            else
            {
                var cb = ClassPool<EventCallback_4<T1,T2,T3,T4>>.Pop();
                cb.Add(callback);
                map.Add(id,cb);
            }
        }



        private static void Add<T1,T2,T3,T4,T5>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T1,T2,T3,T4,T5> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_5<T1,T2,T3,T4,T5> cb)
                {
                    #if DEBUG
                    var events = cb.GetAction.GetInvocationList().Contains(callback);
                    if (events) Logger.LogError("EventModule", "重复订阅事件");
                    #endif
                    cb.Add(callback);
                }
                else
                {
                    Error("Add",id);
                }
            }
            else
            {
                var cb = ClassPool<EventCallback_5<T1,T2,T3,T4,T5>>.Pop();
                cb.Add(callback);
                map.Add(id,cb);
            }
        }


        #endregion
                #region Core Trigger


        private static void Trigger(
            Dictionary<int,IEventCallback> map,
            int id)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_0 cb)
                {
                    cb.Trigger();
                }
                else
                {
                    Error("Trigger",id);
                }
            }
        }



        private static void Trigger<T>(
            Dictionary<int,IEventCallback> map,
            int id,
            T value)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_1<T> cb)
                {
                    cb.Trigger(value);
                }
                else
                {
                    Error("Trigger",id);
                }
            }
        }



        private static void Trigger<T1,T2>(
            Dictionary<int,IEventCallback> map,
            int id,
            T1 t1,
            T2 t2)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_2<T1,T2> cb)
                {
                    cb.Trigger(t1,t2);
                }
                else
                {
                    Error("Trigger",id);
                }
            }
        }



        private static void Trigger<T1,T2,T3>(
            Dictionary<int,IEventCallback> map,
            int id,
            T1 t1,
            T2 t2,
            T3 t3)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_3<T1,T2,T3> cb)
                {
                    cb.Trigger(t1,t2,t3);
                }
                else
                {
                    Error("Trigger",id);
                }
            }
        }



        private static void Trigger<T1,T2,T3,T4>(
            Dictionary<int,IEventCallback> map,
            int id,
            T1 t1,
            T2 t2,
            T3 t3,
            T4 t4)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_4<T1,T2,T3,T4> cb)
                {
                    cb.Trigger(t1,t2,t3,t4);
                }
                else
                {
                    Error("Trigger",id);
                }
            }
        }



        private static void Trigger<T1,T2,T3,T4,T5>(
            Dictionary<int,IEventCallback> map,
            int id,
            T1 t1,
            T2 t2,
            T3 t3,
            T4 t4,
            T5 t5)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_5<T1,T2,T3,T4,T5> cb)
                {
                    cb.Trigger(t1,t2,t3,t4,t5);
                }
                else
                {
                    Error("Trigger",id);
                }
            }
        }


        #endregion





        #region Core Remove


        private static void Remove(
            Dictionary<int,IEventCallback> map,
            int id,
            Action callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_0 cb)
                {
                    cb.Remove(callback);

                    if(cb.IsEmpty())
                    {
                        map.Remove(id);
                        cb.Release();
                    }
                }
                else
                {
                    Error("Remove",id);
                }
            }
        }



        private static void Remove<T>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_1<T> cb)
                {
                    cb.Remove(callback);

                    if(cb.IsEmpty())
                    {
                        map.Remove(id);
                        cb.Release();
                    }
                }
                else
                {
                    Error("Remove",id);
                }
            }
        }



        private static void Remove<T1,T2>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T1,T2> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_2<T1,T2> cb)
                {
                    cb.Remove(callback);

                    if(cb.IsEmpty())
                    {
                        map.Remove(id);
                        cb.Release();
                    }
                }
                else
                {
                    Error("Remove",id);
                }
            }
        }



        private static void Remove<T1,T2,T3>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T1,T2,T3> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_3<T1,T2,T3> cb)
                {
                    cb.Remove(callback);

                    if(cb.IsEmpty())
                    {
                        map.Remove(id);
                        cb.Release();
                    }
                }
                else
                {
                    Error("Remove",id);
                }
            }
        }



        private static void Remove<T1,T2,T3,T4>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T1,T2,T3,T4> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_4<T1,T2,T3,T4> cb)
                {
                    cb.Remove(callback);

                    if(cb.IsEmpty())
                    {
                        map.Remove(id);
                        cb.Release();
                    }
                }
                else
                {
                    Error("Remove",id);
                }
            }
        }



        private static void Remove<T1,T2,T3,T4,T5>(
            Dictionary<int,IEventCallback> map,
            int id,
            Action<T1,T2,T3,T4,T5> callback)
        {
            if(map.TryGetValue(id,out var ec))
            {
                if(ec is EventCallback_5<T1,T2,T3,T4,T5> cb)
                {
                    cb.Remove(callback);

                    if(cb.IsEmpty())
                    {
                        map.Remove(id);
                        cb.Release();
                    }
                }
                else
                {
                    Error("Remove",id);
                }
            }
        }


        #endregion





        private static void Error(string type,int id)
        {
            if(warning)
            {
                Logger.LogError(
                    "EventModule错误",
                    $"{type}事件类型错误 ID:{id}");
            }
        }


    }
}