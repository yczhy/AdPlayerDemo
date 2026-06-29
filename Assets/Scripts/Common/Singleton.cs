namespace Duskvern
{
    /// <summary>
    /// 纯C#单例基类
    /// </summary>
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>, new()
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                            _instance.Init();
                        }
                    }
                }
                return _instance;
            }
        }

        public virtual void Init() { }
        public virtual void Dispose()
        {
            _instance = null;
        }
    }
}