using System;
using System.Collections.Generic;

namespace Duskvern
{
    public enum AdType
    {
        Reward,
        Interstitial
    }

    public enum AdPlatform
    {
        Applovin,
    }

    public enum AdState
    {
        Init,
        Ready,
        Failed,
        Loading,
        Loaded,


    }

    [Serializable]
    public struct AdProviderParameters
    {
        public AdType adType;
        public string adUnitId;
    }

    [Serializable]
    public struct AdPlatformParameters
    {
        public AdPlatform adPlatform;
        public List<AdProviderParameters> adParameters;
    }
}

