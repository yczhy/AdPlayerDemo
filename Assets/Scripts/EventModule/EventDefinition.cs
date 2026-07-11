using UnityEngine;

namespace Duskvern
{
    public class EventDefinition
    {
        private static int idGenerator = -1;
        public readonly int ID;

        public EventDefinition()
        {
            ID = ++idGenerator;
        }
    }

    public sealed class EventDefinition<T> : EventDefinition
    {
        
    }

    public sealed class EventDefinition<T, U> : EventDefinition
    {
        
    }

    public sealed class EventDefinition<T, U, V> : EventDefinition
    {
        
    }

    public sealed class EventDefinition<T, U, V, W> : EventDefinition
    {
        
    }

    public sealed class EventDefinition<T, U, V, W, X> : EventDefinition
    {
        
    }
}

