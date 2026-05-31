using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public class MaxInterstitialADProvider : AdProviderBase
    {
        public override bool IsReady
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
        public override void Init(AdProviderParameters adProviderParameters)
        {
            base.Init(adProviderParameters);
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayed;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHidden;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClicked;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialDisplayFailed;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaid;
        }

        public override void Dispose()
        {
            base.Dispose();
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayed;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHidden;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent -= OnInterstitialClicked;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialDisplayFailed;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnInterstitialRevenuePaid;
        }

        /// <summary>
        /// 加载插屏广告
        /// </summary>
        public override void LoadAd()
        {
            if (isLoading)
            {
                Logger.LogAd("插屏广告正在加载已加载");
                return;
            }
            if (IsReady)
            {
                Logger.LogAd("插屏广告已加载");
                return;
            }

            isLoading = true;
            Logger.LogAd("开始加载插屏广告");
            MaxSdk.LoadInterstitial(adUnitId);
        }

        /// <summary>
        /// 展示插屏广告
        /// </summary>
        public override void ShowAd()
        {
            if (!IsReady)
            {
                Logger.LogAd("插屏广告未准备好");
                AdEnd();
                adProviderParameters.OnAdHidden?.Invoke(false, string.Empty, this);
                return;
            }

            hasRevenuePaid = false;
            hasRewarded = false;
            MaxSdk.ShowInterstitial(adUnitId);
        }

        // 插屏加载成功
        private void OnInterstitialLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("插屏广告加载成功, 但不是当前插屏广告");
                return;
            }
            isLoading = false;
            ecpm = SDKTools.NormalECPM(adInfo.Revenue);
            currentRetryCount = 0;
            adProviderParameters.OnAdLoaded?.Invoke(adInfo?.ToString(), this);
            Logger.LogAd("插屏广告加载成功 该条广告ECPM为: " + ecpm);
        }

        // 插屏加载失败
        private void OnInterstitialLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("插屏广告加载失败, 但不是当前插屏广告");
                return;
            }
            isLoading = false;
            Logger.LogAd($"插屏广告加载失败: {errorInfo.Message}");
            adProviderParameters.OnAdLoadFailedEvent?.Invoke(errorInfo?.ToString(), this);

            if (currentRetryCount < retryLoadCount)
            {
                currentRetryCount++;
                RetryLoadAd(currentRetryCount).Forget();
            }
        }

        // 插屏展示成功
        private void OnInterstitialDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("插屏广告展示成功, 但不是当前插屏广告");
                return;
            }
            Logger.LogAd("插屏广告开始展示");
            adProviderParameters.OnAdDisplayed?.Invoke(adInfo?.ToString(), this);
        }

        // 插屏关闭
        private void OnInterstitialHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("插屏广告关闭, 但不是当前插屏广告");
                return;
            }
            bool success = hasRevenuePaid;
            adProviderParameters.OnAdHidden?.Invoke(success, adInfo?.ToString(), this);
            Logger.LogAd("插屏广告关闭, 是否可以奖励: " + success);
            AdEnd();
        }

        // 插屏点击
        private void OnInterstitialClicked(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("插屏广告点击, 但不是当前插屏广告");
                return;
            }
            Logger.LogAd("插屏广告被点击");
            adProviderParameters.OnAdClicked?.Invoke(adInfo?.ToString(), this);
        }

        // 插屏展示失败
        private void OnInterstitialDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("插屏广告展示失败, 但不是当前插屏广告");
                return;
            }
            Logger.LogAd($"插屏广告展示失败: {errorInfo.Message}");
            adProviderParameters.OnAdDisplayFailed?.Invoke(adInfo?.ToString(), errorInfo?.ToString(), this);
            AdEnd();
        }

        private void OnInterstitialRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("插屏广告收益支付, 但不是当前插屏广告");
                return;
            }
            hasRevenuePaid = true;
            Logger.LogAd("插屏广告收益支付成功");
            adProviderParameters.OnAdRevenuePaid?.Invoke(adInfo?.ToString(), this);
        }
    }
}