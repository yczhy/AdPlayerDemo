using UnityEngine;

namespace Duskvern
{
    public static class AssetUtils
    {
        public static T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }
    }
}

