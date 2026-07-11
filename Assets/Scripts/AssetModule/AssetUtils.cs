using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Duskvern
{
    public static class AssetUtils
    {
        private static AssetModule _assetModule;
        
        /// <summary>
        /// 初始化资源模块（必须在游戏启动时调用一次）
        /// </summary>
        public static void Init(AssetLoadType loadType = AssetLoadType.Resources)
        {
            _assetModule = new AssetModule().Init(loadType);
        }
        
        /// <summary>
        /// 获取资源模块实例
        /// </summary>
        public static AssetModule Module => _assetModule;
        
        /// <summary>
        /// 检查是否已初始化
        /// </summary>
        public static bool IsInitialized => _assetModule != null;
        
        /// <summary>
        /// 同步加载资源
        /// </summary>
        public static T Load<T>(string assetPath) where T : Object
        {
            EnsureInitialized();
            return _assetModule.LoadAsset<T>(assetPath);
        }
        
        /// <summary>
        /// 异步加载资源
        /// </summary>
        public static async UniTask<T> LoadAsync<T>(string assetPath) where T : Object
        {
            EnsureInitialized();
            return await _assetModule.LoadAssetAsync<T>(assetPath);
        }
        
        /// <summary>
        /// 批量异步加载资源
        /// </summary>
        public static async UniTask<T[]> LoadAllAsync<T>(params string[] assetPaths) where T : Object
        {
            EnsureInitialized();
            return await _assetModule.LoadAssetsAsync<T>(assetPaths);
        }
        
        /// <summary>
        /// 卸载指定资源
        /// </summary>
        public static void Unload<T>(T asset) where T : Object
        {
            if (asset == null) return;
            EnsureInitialized();
            _assetModule.UnloadAsset(asset);
        }
        
        /// <summary>
        /// 卸载所有资源
        /// </summary>
        public static void UnloadAll()
        {
            if (!IsInitialized) return;
            _assetModule.UnLoadAllAssets();
        }
        
        /// <summary>
        /// 加载并实例化 GameObject（同步）
        /// </summary>
        public static GameObject Instantiate(string assetPath, Transform parent = null)
        {
            var prefab = Load<GameObject>(assetPath);
            if (prefab == null) return null;
            
            var instance = Object.Instantiate(prefab, parent);
            instance.name = prefab.name;
            return instance;
        }
        
        /// <summary>
        /// 异步加载并实例化 GameObject
        /// </summary>
        public static async UniTask<GameObject> InstantiateAsync(string assetPath, Transform parent = null)
        {
            var prefab = await LoadAsync<GameObject>(assetPath);
            if (prefab == null) return null;
            
            var instance = Object.Instantiate(prefab, parent);
            instance.name = prefab.name;
            return instance;
        }
        
        /// <summary>
        /// 加载并实例化后自动绑定组件
        /// </summary>
        public static async UniTask<T> InstantiateWithComponentAsync<T>(string assetPath, Transform parent = null) where T : Component
        {
            var go = await InstantiateAsync(assetPath, parent);
            if (go == null) return null;
            
            var component = go.GetComponent<T>();
            if (component == null)
            {
                Logger.LogWarning("AssetUtils", $"GameObject {assetPath} does not have component {typeof(T).Name}");
            }
            return component;
        }
        
        /// <summary>
        /// 释放 GameObject 实例（销毁并卸载原始资源）
        /// </summary>
        public static void ReleaseInstance(GameObject instance, bool unloadAsset = false)
        {
            if (instance == null) return;
            
            if (unloadAsset)
            {
                // 获取原始资源路径（需要额外存储，简单实现为名称匹配）
                Unload(instance);
            }
            
            Object.Destroy(instance);
        }
        
        /// <summary>
        /// 确保模块已初始化
        /// </summary>
        private static void EnsureInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("AssetUtils not initialized. Call AssetUtils.Initialize() first.");
            }
        }
        
        #region 便捷方法
        
        /// <summary>
        /// 加载 Sprite
        /// </summary>
        public static Sprite LoadSprite(string assetPath)
        {
            return Load<Sprite>(assetPath);
        }
        
        /// <summary>
        /// 加载 Texture2D
        /// </summary>
        public static Texture2D LoadTexture(string assetPath)
        {
            return Load<Texture2D>(assetPath);
        }
        
        /// <summary>
        /// 加载 AudioClip
        /// </summary>
        public static AudioClip LoadAudio(string assetPath)
        {
            return Load<AudioClip>(assetPath);
        }
        
        /// <summary>
        /// 加载 Material
        /// </summary>
        public static Material LoadMaterial(string assetPath)
        {
            return Load<Material>(assetPath);
        }
        
        #endregion
    }
}