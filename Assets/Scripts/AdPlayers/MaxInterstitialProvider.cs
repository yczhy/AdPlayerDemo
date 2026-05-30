using UnityEngine;

namespace Duskvern
{
    public class MaxInterstitialADProvider : IAdProvider
    {
        protected string adUnitId = "你的插屏广告ID";
        protected AdType adType;

        protected bool isReward = false;
        protected bool isLoaded = false;
        

        public AdType AdType => adType;

        public string AdUnitId => adUnitId;

        public bool IsReady
        {
            get
            {
                bool isReady = MaxSdk.IsInterstitialReady(adUnitId);
                return isReady;
            }
        }

        /// <summary>
        /// 初始化插屏广告
        /// </summary>
        public void Init(AdType adType, string adUnitId)
        {
            this.adType = adType;
            this.adUnitId = adUnitId;
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayed;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHidden;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClicked;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialDisplayFailed;
        }

        /// <summary>
        /// 加载插屏广告
        /// </summary>
        public void LoadAd()
        {
            if (isLoaded)
            {
                Logger.LogAd("插屏广告已加载");
                return;
            }
            Logger.LogAd("开始加载插屏广告");
            MaxSdk.LoadInterstitial(adUnitId);
        }

        /// <summary>
        /// 展示插屏广告
        /// </summary>
        public void ShowInterstitial()
        {
            if (IsReady)
            {
                MaxSdk.ShowInterstitial(adUnitId);
            }
            else
            {
                Logger.LogAd("插屏广告未准备好");
            }
        }

        // 插屏加载成功
        private void OnInterstitialLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            isLoaded = true;
            Logger.LogAd("插屏广告加载成功");
        }

        // 插屏加载失败
        private void OnInterstitialLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            isLoaded = false;
            Logger.LogAd($"插屏广告加载失败: {errorInfo.Message}");

            // 可自行加重试逻辑
        }

        // 插屏展示成功
        private void OnInterstitialDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogAd("插屏广告开始展示");
        }

        // 插屏关闭
        private void OnInterstitialHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogAd("插屏广告关闭");
        }

        // 插屏点击
        private void OnInterstitialClicked(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogAd("插屏广告被点击");
        }

        // 插屏展示失败
        private void OnInterstitialDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            Logger.LogAd($"插屏广告展示失败: {errorInfo.Message}");
        }
    }
}