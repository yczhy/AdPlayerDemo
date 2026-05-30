using UnityEngine;

namespace Duskvern
{
    public class MaxRewardAdProvider
    {
        private string rewardAdUnitId = "你的Reward广告ID";

        public void Init()
        {
            // 初始化 Reward 广告
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardAdLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardAdDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardAdHidden;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardAdClicked;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardAdDisplayFailed;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardReceived;

            LoadRewardAd();
        }

        /// <summary>
        /// 加载 Reward 广告
        /// </summary>
        public void LoadRewardAd()
        {
            MaxSdk.LoadRewardedAd(rewardAdUnitId);
        }

        /// <summary>
        /// 播放 Reward 广告
        /// </summary>
        public void ShowRewardAd()
        {
            if (MaxSdk.IsRewardedAdReady(rewardAdUnitId))
            {
                MaxSdk.ShowRewardedAd(rewardAdUnitId);
            }
            else
            {
                Debug.Log("Reward广告未准备好");
            }
        }

        // 广告加载成功
        private void OnRewardAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Reward广告加载成功");
        }

        // 广告加载失败
        private void OnRewardAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            Debug.Log($"Reward广告加载失败: {errorInfo.Message}");

            // 失败后重新加载
        }

        // 广告展示成功
        private void OnRewardAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Reward广告开始展示");
        }

        // 广告关闭
        private void OnRewardAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Reward广告关闭");

            // 提前加载下一条
            LoadRewardAd();
        }

        // 广告点击
        private void OnRewardAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Reward广告被点击");
        }

        // 广告展示失败
        private void OnRewardAdDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"Reward广告展示失败: {errorInfo.Message}");

            LoadRewardAd();
        }

        // 用户获得奖励
        private void OnRewardReceived(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log($"获得奖励: {reward.Amount} {reward.Label}");

            // 在这里发奖励
            // AddCoin();
        }
    }
}