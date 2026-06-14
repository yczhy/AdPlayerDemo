using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public enum AssetLoadType
    {
        Resources,
        Addressable
    }

    public interface IAssetLoader
    {
        AssetLoadType assetLoadType { get; }

        IAssetLoader Init(AssetModule assetModule);

        T LoadAsset<T> (string assetName) where T : UnityEngine.Object;

        UniTask<T> LoadAsssetAsync<T> (string assetName) where T : UnityEngine.Object;

        UniTask<T[]> LoadAssetsAsync<T>(string[] assetNames) where T : UnityEngine.Object;
        
        void UnloadAsset<T> (T asset) where T : UnityEngine.Object;

        void UnloadAllAssets ();
    }
}

