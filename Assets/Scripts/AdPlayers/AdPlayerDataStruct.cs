using System;
using System.Collections.Generic;

namespace Duskvern
{
    public enum AdType
    {
        None,
        Reward,
        Interstitial
    }

    public enum AdPlatform
    {
        None,
        Applovin,
    }

    [Serializable]
    public class AdProviderParameters
    {
        public AdPlatform adPlatform;
        public AdType adType;
        public string adUnitId;

        public Action<string, AdProviderBase> OnAdLoaded;           // 广告加载成功
        public Action<string, AdProviderBase> OnAdLoadFailedEvent; // 广告加载失败（带错误信息）
        public Action<string, AdProviderBase> OnAdDisplayed;        // 广告展示
        public Action<string, AdProviderBase> OnAdClicked;          // 广告点击
        public Action<string, string, AdProviderBase> OnAdDisplayFailed; // 广告展示失败
        public Action<string, AdProviderBase> OnAdRevenuePaid;      // 广告收入回调
        public Action<bool, string, AdProviderBase> OnAdHidden;      // 广告关闭
    }

    [Serializable]
    public class AdPlatformParameters
    {
        public AdPlatform adPlatform;
        public bool isEnableTestMode;
        public List<AdProviderParameters> adParameters;
    }

    public class PlayAdParameters : IPoolable
    {
        public string playAdPos;
        public AdType adType;
        public bool InterstitialReplaceReward;
        public bool RewardReplaceInterstitial;
        public bool openPlayAdCondition;
        public bool isPriceRelations; // 是否进行比价
        public AdPlatform adPlatform; // 当不比价的时候，可以指定平台
        public Action FailCallback;
        public Action SuccessCallback;

        public PlayAdParameters() { }

        private bool isInPool = false;
        public bool InPool { get => isInPool; set => isInPool = value; }

        public void DeSpawn()
        {
            
        }

        public void Spawn()
        {
            FailCallback = null;
            SuccessCallback = null;
        }

        public void Init(string playAdPos, AdType adType, bool InterstitialReplaceReward, bool RewardReplaceInterstitial, bool openPlayAdCondition, bool isPriceRelations, AdPlatform adPlatform, Action FailCallback, Action SuccessCallback)
        {
            this.playAdPos = playAdPos;
            this.adType = adType;
            this.InterstitialReplaceReward = InterstitialReplaceReward;
            this.RewardReplaceInterstitial = RewardReplaceInterstitial;
            this.openPlayAdCondition = openPlayAdCondition;
            this.isPriceRelations = isPriceRelations;
            this.adPlatform = adPlatform;
            this.FailCallback = FailCallback;
            this.SuccessCallback = SuccessCallback;
        }
    }
}

