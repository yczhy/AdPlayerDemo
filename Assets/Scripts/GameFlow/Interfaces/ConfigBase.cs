using UnityEngine;

namespace Duskvern
{
    public abstract class IConfigBase : ScriptableObject
    {

    }

    public abstract class ILevelConfigBase : IConfigBase
    {
        
    }

    public abstract class IModuleConfigBase<T> : IConfigBase where T : IConfigBase
    {
         private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(typeof(T).Name);
                    if (_instance == null)
                    {
                        Debug.LogError($"Config {typeof(T).Name} not found in Resources!");
                    }
                }
                return _instance;
            }
        }
    }
}

