using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public static class AdUtils
    {
        private static AdConfig adConfig;
        private static readonly AdModule adModule = new AdModule();

        public static void Init()
        {
            adConfig = Resources.Load<AdConfig>("Duskvern_Configs/AdConfig");
            if (adConfig == null)
            {
                Logger.LogAd("AdConfig not found");
                return;
            }
            adModule.Init(adConfig).Forget();
        }

        public static void ShowRewardAd(string playAdPos, Action<bool> callback)
        {
            PlayAd(playAdPos, AdType.Reward, callback, false);
        }

        public static void ShowRewardAd(string playAdPos, Action<bool> callback, bool openPlayAdCondition = false)
        {
            PlayAd(playAdPos, AdType.Reward, callback, openPlayAdCondition);
        }

        public static void ShowRewardAd(string playAdPos, Action SuccessCallback, Action FailCallback)
        {
            PlayAd(playAdPos, AdType.Reward, SuccessCallback, FailCallback, false);
        }

        public static void ShowRewardAd(string playAdPos, Action SuccessCallback, Action FailCallback, bool openPlayAdCondition = false)
        {
            PlayAd(playAdPos, AdType.Reward, SuccessCallback, FailCallback, openPlayAdCondition);
        }

        public static void ShowInterstitialAd(string playAdPos, Action SuccessCallback, Action FailCallback)
        {
            PlayAd(playAdPos, AdType.Interstitial, SuccessCallback, FailCallback, false);
        }

        public static void ShowInterstitialAd(string playAdPos, Action<bool> callback)
        {
            PlayAd(playAdPos, AdType.Interstitial, callback);
        }

        public static void ShowInterstitialAd(string playAdPos, Action<bool> callback, bool openPlayAdCondition = false)
        {
            PlayAd(playAdPos, AdType.Interstitial, callback, openPlayAdCondition);
        }

        public static void ShowInterstitialAd(string playAdPos, Action SuccessCallback, Action FailCallback, bool openPlayAdCondition = false)
        {
            PlayAd(playAdPos, AdType.Interstitial, SuccessCallback, FailCallback, openPlayAdCondition);
        }

        public static void ShowAd(string playAdPos, AdType adType, Action<bool> callback)
        {
            PlayAd(playAdPos, adType, callback);
        }

        public static void PlayAd(string playAdPos, AdType adType, Action<bool> callback, bool openPlayAdCondition = false)
        {
            PlayAd(playAdPos, adType, () => callback?.Invoke(true), () => callback?.Invoke(false), openPlayAdCondition);
        }

        public static void PlayAd(string playAdPos, AdType adType, Action SuccessCallback, Action FailCallback, bool openPlayAdCondition = false)
        {
            PlayAd(playAdPos, adType, SuccessCallback, FailCallback, true, adConfig.interstitialReplaceReward, adConfig.rewardReplaceInterstitial, openPlayAdCondition, AdPlatform.None);
        }

        public static void PlayAd(string playAdPos, AdType adType, Action SuccessCallback, Action FailCallback, bool isPriceRelations = true, 
            bool InterstitialReplaceReward = false, bool RewardReplaceInterstitial = false, 
            bool openPlayAdCondition = false, AdPlatform adPlatform = AdPlatform.None)
        {
            var parameters = ClassPool<PlayAdParameters>.Pop();
            parameters.Init(playAdPos, adType, InterstitialReplaceReward, RewardReplaceInterstitial, openPlayAdCondition, isPriceRelations, adPlatform, FailCallback, SuccessCallback);
            PlayAd(parameters);
        }

        public static void PlayAd(PlayAdParameters playAdParameters)
        {
            adModule.PlayAd(playAdParameters);
        }
    }

}
