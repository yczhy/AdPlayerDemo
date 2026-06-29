namespace Duskvern
{
    using UnityEngine;

    /// <summary>
    /// MonoBehaviour单例基类（使用 FindAnyObjectByType）
    /// </summary>
    public abstract class SingletonMono<T> : MonoBehaviour, ISingleton where T : SingletonMono<T>
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _isQuitting = false; // 防止退出时重复创建

        public static T Instance
        {
            get
            {
                if (_isQuitting)
                {
                    Debug.LogWarning($"[Singleton] {typeof(T).Name} 已在退出中，返回 null");
                    return null;
                }

                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            // 使用 FindAnyObjectByType 替代已过时的 FindFirstObjectByType
                            _instance = FindAnyObjectByType<T>();

                            if (_instance == null)
                            {
                                // 创建新的GameObject并挂载
                                GameObject go = new GameObject(typeof(T).Name);
                                _instance = go.AddComponent<T>();
                                DontDestroyOnLoad(go);

                                Debug.Log($"[Singleton] 创建新的 {typeof(T).Name} 实例");
                            }
                            else
                            {
                                // 确保已有的单例不销毁
                                DontDestroyOnLoad(_instance.gameObject);
                            }
                        }
                    }
                }
                return _instance;
            }
        }

        // 实现接口方法
        public virtual void Init() { }
        public virtual void Dispose()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
                Init();
            }
            else if (_instance != this)
            {
                // 如果已经存在实例，销毁重复的
                Debug.LogWarning($"[Singleton] 检测到重复的 {typeof(T).Name}，已销毁");
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _isQuitting = true;
                Dispose();
            }
        }

        // 应用退出时的清理
        protected virtual void OnApplicationQuit()
        {
            _isQuitting = true;
        }
    }
}