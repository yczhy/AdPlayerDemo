using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Duskvern
{
    public class AddressableLoader : IAssetLoader
    {
        public AssetLoadType assetLoadType => AssetLoadType.Addressable;
        private AssetModule assetModule;
        
        // 记录已加载的资源句柄，用于卸载
        private readonly Dictionary<string, AsyncOperationHandle> _loadedHandles = new();
        private readonly Dictionary<UnityEngine.Object, string> _assetToKey = new(); 

        public IAssetLoader Init(AssetModule assetModule)
        {
            this.assetModule = assetModule;
            Addressables.InitializeAsync();
            return this;
        }

        public T LoadAsset<T>(string _assetName) where T : UnityEngine.Object
        {
            // 这里是同步加载
            var handle = Addressables.LoadAssetAsync<T>(_assetName);
            handle.WaitForCompletion();
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                RegisterHandle(_assetName, handle);
                return handle.Result;
            }
            
            Logger.LogError("Asset", $"Failed to load asset {_assetName}: {handle.OperationException}");
            return null;
        }

        public async UniTask<T> LoadAsssetAsync<T>(string _assetName) where T : UnityEngine.Object
        {
            try
            {
                // 检查是否已加载
                if (_loadedHandles.TryGetValue(_assetName, out var existingHandle))
                {
                    if (existingHandle.IsValid() && existingHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        return existingHandle.Result as T;
                    }
                }
                
                // 异步加载
                var handle = Addressables.LoadAssetAsync<T>(_assetName);
                RegisterHandle(_assetName, handle);
                
                // 等待加载完成
                await handle.Task;
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return handle.Result;
                }
                
                Logger.LogError("Asset", $"Failed to load asset {_assetName}: {handle.OperationException}");
                return null;
            }
            catch (System.Exception ex)
            {
                Logger.LogError("Asset", $"Exception loading asset {_assetName}: {ex.Message}");
                return null;
            }
        }

        public void UnloadAsset<T>(T _asset) where T : UnityEngine.Object
        {
            if (_asset == null)
            {
                Logger.LogWarning("Asset", "Attempting to unload null asset");
                return;
            }
            
            // 通过资源对象查找对应的key
            if (_assetToKey.TryGetValue(_asset, out var key))
            {
                UnloadAsset(key);
                _assetToKey.Remove(_asset);
            }
            else
            {
                // 直接尝试释放资源（需要知道key）
                Logger.LogWarning("Asset", $"Cannot unload {_asset.name}: no tracking info");
            }
        }

        public void UnloadAsset(string _assetName)
        {
            if (_loadedHandles.TryGetValue(_assetName, out var handle))
            {
                if (handle.IsValid())
                {
                    // 记录资源对象以便从_assetToKey中移除
                    if (handle.Result is UnityEngine.Object assetObj && _assetToKey.ContainsKey(assetObj))
                    {
                        _assetToKey.Remove(assetObj);
                    }
                    
                    Addressables.Release(handle);
                    _loadedHandles.Remove(_assetName);
                }
            }
            else
            {
                Logger.LogWarning("Asset", $"Asset {_assetName} not found in loaded handles");
            }
        }

        public void UnloadAllAssets()
        {
            foreach (var kvp in _loadedHandles)
            {
                if (kvp.Value.IsValid())
                {
                    Addressables.Release(kvp.Value);
                }
            }
            _loadedHandles.Clear();
            _assetToKey.Clear();
            
            // 可选：清理未使用的资源
            Resources.UnloadUnusedAssets();
        }
        
        // 辅助方法：注册加载的句柄
        private void RegisterHandle<T>(string assetName, AsyncOperationHandle<T> handle) where T : UnityEngine.Object
        {
            if (_loadedHandles.ContainsKey(assetName))
            {
                // 如果已存在，先释放旧的
                if (_loadedHandles[assetName].IsValid())
                {
                    Addressables.Release(_loadedHandles[assetName]);
                }
                _loadedHandles.Remove(assetName);
            }
            
            _loadedHandles.Add(assetName, handle);
            
            // 监听完成事件，记录资源对象
            handle.Completed += op =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded && op.Result != null)
                {
                    _assetToKey[op.Result] = assetName;
                }
            };
        }
        
        // 可选：批量加载
        public async UniTask<T[]> LoadAssetsAsync<T>(string[] assetNames) where T : UnityEngine.Object
        {
            var tasks = assetNames.Select(name => LoadAsssetAsync<T>(name));
            var results = await UniTask.WhenAll(tasks);
            return results.Where(r => r != null).ToArray();
        }
        
        // 可选：通过标签加载
        public async UniTask<T[]> LoadAssetsByLabelAsync<T>(string label) where T : UnityEngine.Object
        {
            try
            {
                var handle = Addressables.LoadAssetsAsync<T>(label, null);
                await handle.Task;
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // 为每个资源注册句柄（需要使用不同的key策略）
                    foreach (var asset in handle.Result)
                    {
                        // 这里需要处理多个资源的句柄管理
                    }
                    return handle.Result.ToArray();
                }
                
                return Array.Empty<T>();
            }
            catch (System.Exception ex)
            {
                Logger.LogError("Asset", $"Failed to load assets by label {label}: {ex.Message}");
                return Array.Empty<T>();
            }
        }
    }
}