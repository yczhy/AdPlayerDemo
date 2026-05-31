using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public class MaxRewardAdProvider : AdProviderBase
    {
        public override bool IsReady
        {
            get
            {
                bool isReady = MaxSdk.IsRewardedAdReady(adUnitId);
                return isReady;
            }
        }

        /// <summary>
        /// 初始化激励广告
        /// </summary>
        public override void Init(AdProviderParameters adProviderParameters)
        {
            base.Init(adProviderParameters);

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardAdLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardAdDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardAdHidden;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardAdClicked;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardAdDisplayFailed;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardReceived;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardAdRevenuePaid;
        }

        public override void Dispose()
        {
            base.Dispose();
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardAdLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardAdDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardAdHidden;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardAdClicked;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardAdDisplayFailed;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardReceived;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnRewardAdRevenuePaid;
        }

        /// <summary>
        /// 加载激励广告
        /// </summary>
        public override void LoadAd()
        {
            if (isLoading)
            {
                Logger.LogAd("激励广告正在加载已加载");
                return;
            }

            if (IsReady)
            {
                Logger.LogAd("激励广告已加载");
                return;
            }

            isLoading = true;
            Logger.LogAd("开始加载激励广告");
            MaxSdk.LoadRewardedAd(adUnitId);
        }

        /// <summary>
        /// 展示激励广告
        /// </summary>
        public override void ShowAd()
        {
            if (!IsReady)
            {
                Logger.LogAd("激励广告未准备好");
                AdEnd();
                adProviderParameters.OnAdHidden?.Invoke(false, string.Empty, this);
                return;
            }

            hasRevenuePaid = false;
            hasRewarded = false;
            MaxSdk.ShowRewardedAd(adUnitId);
        }

        // 激励广告加载成功
        private void OnRewardAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("激励广告加载成功, 但不是当前激励广告");
                return;
            }

            isLoading = false;
            ecpm = SDKTools.NormalECPM(adInfo.Revenue);
            currentRetryCount = 0;
            adProviderParameters.OnAdLoaded?.Invoke(adInfo?.ToString(), this);
            Logger.LogAd("激励广告加载成功 该条广告ECPM为: " + ecpm);
        }

        // 激励广告加载失败
        private void OnRewardAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("激励广告加载失败, 但不是当前激励广告");
                return;
            }

            isLoading = false;
            Logger.LogAd($"激励广告加载失败: {errorInfo.Message}");
            adProviderParameters.OnAdLoadFailedEvent?.Invoke(errorInfo?.ToString(), this);

            if (currentRetryCount < retryLoadCount)
            {
                currentRetryCount++;
                RetryLoadAd(currentRetryCount).Forget();
            }
        }

        // 激励广告展示成功
        private void OnRewardAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("激励广告展示成功, 但不是当前激励广告");
                return;
            }

            Logger.LogAd("激励广告开始展示");
            adProviderParameters.OnAdDisplayed?.Invoke(adInfo?.ToString(), this);
        }

        // 激励广告关闭
        private void OnRewardAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("激励广告关闭, 但不是当前激励广告");
                return;
            }

            bool success = hasRewarded && hasRevenuePaid;
            adProviderParameters.OnAdHidden?.Invoke(success, adInfo?.ToString(), this);
            Logger.LogAd("激励广告关闭, 是否可以奖励: " + success);
            AdEnd();
        }

        // 激励广告点击
        private void OnRewardAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("激励广告点击, 但不是当前激励广告");
                return;
            }

            Logger.LogAd("激励广告被点击");
            adProviderParameters.OnAdClicked?.Invoke(adInfo?.ToString(), this);
        }

        // 激励广告展示失败
        private void OnRewardAdDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("激励广告展示失败, 但不是当前激励广告");
                return;
            }

            Logger.LogAd($"激励广告展示失败: {errorInfo.Message}");
            adProviderParameters.OnAdDisplayFailed?.Invoke(adInfo?.ToString(), errorInfo?.ToString(), this);
            AdEnd();
        }

        // 用户获得奖励
        private void OnRewardReceived(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("激励广告获得奖励, 但不是当前激励广告");
                return;
            }
            hasRewarded = true;
        }

        private void OnRewardAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != this.adUnitId)
            {
                Logger.LogAd("激励广告收益支付, 但不是当前激励广告");
                return;
            }

            hasRevenuePaid = true;
            Logger.LogAd("激励广告收益支付成功");
            adProviderParameters.OnAdRevenuePaid?.Invoke(adInfo?.ToString(), this);
        }
    }
}