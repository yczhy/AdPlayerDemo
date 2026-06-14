using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    // TODO: 增加一个资源计数机制
    public class AssetModule
    {
        private IAssetLoader assetLoader;

        public AssetModule Init(AssetLoadType assetLoadType)
        {
            switch (assetLoadType)
            {
                case AssetLoadType.Resources:
                    assetLoader = new ResourceLoader().Init(this);
                    break;
                case AssetLoadType.Addressable:
                    assetLoader = new AddressableLoader().Init(this);
                    break;
            }
            return this;
        }

        public T LoadAsset<T>(string assetPath) where T : UnityEngine.Object
        {
            return assetLoader.LoadAsset<T>(assetPath);
        }

        public UniTask<T> LoadAssetAsync<T>(string assetPath) where T : UnityEngine.Object
        {
            return assetLoader.LoadAsssetAsync<T>(assetPath);
        }

        public UniTask<T[]> LoadAssetsAsync<T>(string[] assetPaths) where T : UnityEngine.Object
        {
            return assetLoader.LoadAssetsAsync<T>(assetPaths);
        }

        public void UnloadAsset<T>(T asset) where T : UnityEngine.Object
        {
            assetLoader.UnloadAsset(asset);
        }

        public void UnLoadAllAssets()
        {
            assetLoader.UnloadAllAssets();
        }
    }
}

