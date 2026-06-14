using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public class ResourceLoader : IAssetLoader
    {
        public AssetLoadType assetLoadType => AssetLoadType.Resources;
        
        private readonly Dictionary<Object, string> _assetMap = new Dictionary<Object, string>();

        public IAssetLoader Init(AssetModule assetModule) => this;

        public T LoadAsset<T>(string assetName) where T : Object
        {
            var asset = Resources.Load<T>(assetName);
            if (asset == null)
            {
                Logger.LogError("Asset", $"Asset {assetName} not found");
                return null;
            }
            
            _assetMap[asset] = assetName;
            return asset;
        }

        public async UniTask<T> LoadAsssetAsync<T>(string assetName) where T : Object
        {
            var request = Resources.LoadAsync<T>(assetName);
            var asset = await request;
            
            if (asset == null)
            {
                Logger.LogError("Asset", $"Asset {assetName} not found");
                return null;
            }
            
            if (asset is T result)
            {
                _assetMap[result] = assetName;
                return result;
            }
            
            Logger.LogError("Asset", $"Asset {assetName} type mismatch");
            return null;
        }

        public async UniTask<T[]> LoadAssetsAsync<T>(string[] assetNames) where T : Object
        {
            if (assetNames == null || assetNames.Length == 0) return System.Array.Empty<T>();
            
            var tasks = assetNames.Select(LoadAsssetAsync<T>).ToArray();
            await UniTask.WhenAll(tasks);
            
            var results = tasks.Select(t => t.GetAwaiter().GetResult()).Where(r => r != null).ToArray();
            Logger.Log("Asset", $"Loaded {results.Length}/{assetNames.Length} assets");
            return results;
        }

        public void UnloadAsset<T>(T asset) where T : Object
        {
            if (asset == null) return;
            
            _assetMap.Remove(asset);
            Resources.UnloadAsset(asset);
        }

        public void UnloadAllAssets()
        {
            foreach (var asset in _assetMap.Keys.ToList())
            {
                if (asset != null) Resources.UnloadAsset(asset);
            }
            
            _assetMap.Clear();
        }
    }
}