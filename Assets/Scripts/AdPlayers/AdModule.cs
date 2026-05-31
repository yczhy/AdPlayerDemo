using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public class AdModule
    {
        private bool isAdPlaying = false;
        private bool isAdPlayEnd = false;
        private readonly List<AdInitializerBase> allAdInitializers = new()
        {
            new MaxApplovinInitializer()
        };
        private readonly List<IAdProvider> allAdProviders = new();

        private PlayAdParameters playAdParameters;

        private bool CanPlayAd = false;

        private bool PlatformInitiated
        {
            get
            {
                bool isInitiated = true;
                foreach (var adInitializer in allAdInitializers)
                {
                    if (!adInitializer.IsInited)
                    {
                        isInitiated = false;
                        break;
                    }
                }
                return isInitiated;
            }
        }

        public async UniTask Init(AdConfig _adConfig)
        {
            CanPlayAd = false;
            foreach (var adInitializer in allAdInitializers)
            {
                adInitializer.Init(_adConfig);
            }
            await UniTask.WaitWhile(() => PlatformInitiated);

            foreach (var adProvider in allAdProviders)
            {
                adProvider.Dispose();
            }
            allAdProviders.Clear();

            List<AdPlatformParameters> adPlatformParameters = _adConfig.adPlatformParameters;

            if (adPlatformParameters == null || adPlatformParameters.Count == 0)
            {
                Logger.LogAd("AdPlatformParameters is null or empty");
                return;
            }

            foreach (var adPlatformParameter in adPlatformParameters)
            {
                if (adPlatformParameter.adParameters == null)
                {
                    Logger.LogAd("AdPlatformParameters.adParameters is null");
                    continue;
                }

                switch (adPlatformParameter.adPlatform)
                {
                    case AdPlatform.Applovin:
                        foreach (var _adProviderParameters in adPlatformParameter.adParameters)
                        {
                            if (_adProviderParameters.adType == AdType.Interstitial)
                            {
                                var providerParams = CreateProviderParameters(
                                    adPlatformParameter.adPlatform,
                                    _adProviderParameters.adType,
                                    _adProviderParameters.adUnitId
                                );
                                AddCanPlayProvider(new MaxInterstitialADProvider(), providerParams);
                            }
                            else if (_adProviderParameters.adType == AdType.Reward)
                            {
                                var providerParams = CreateProviderParameters(
                                    adPlatformParameter.adPlatform,
                                    _adProviderParameters.adType,
                                    _adProviderParameters.adUnitId
                                );
                                AddCanPlayProvider(new MaxRewardAdProvider(), providerParams);
                            }
                        }
                        break;
                }
            }

            foreach (var adProvider in allAdProviders)
            {
                adProvider.LoadAd();
            }

            isAdPlaying = false;
            isAdPlayEnd = false;
            CanPlayAd = true;
        }

        private AdProviderParameters CreateProviderParameters(
            AdPlatform adPlatform,
            AdType adType,
            string adUnitId)
        {
            return new AdProviderParameters()
            {
                adPlatform = adPlatform,
                adType = adType,
                adUnitId = adUnitId,

                OnAdLoaded = this.OnAdLoaded,
                OnAdLoadFailedEvent = this.OnAdLoadFailedEvent,
                OnAdDisplayed = this.OnAdDisplayed,
                OnAdClicked = this.OnAdClicked,
                OnAdDisplayFailed = OnAdDisplayFailed,
                OnAdRevenuePaid = OnAdRevenuePaid,
                OnAdHidden = OnAdHidden
            };
        }

        private void AddCanPlayProvider(IAdProvider adProvider, AdProviderParameters adProviderParameters)
        {
            adProvider.Init(adProviderParameters);
            allAdProviders.Add(adProvider);
        }

        public void PlayAd(PlayAdParameters playAdParameters)
        {
            if (!CanPlayAd)
            {
                Logger.LogAd("广告模块未初始化");
                return;
            }
            if (playAdParameters == null)
            {
                Logger.LogAd("播放广告参数为空");
                return;
            }
            if (isAdPlaying)
            {
                playAdParameters.FailCallback?.Invoke();
                Logger.LogAd("当前有广告正在播放");
                return;
            }
            isAdPlayEnd = false;
            this.playAdParameters = playAdParameters;
            OnStartPlayAd();
            IAdProvider _adProvider = SelectAdProvider();
            _adProvider = CheckAdPlayCondition(_adProvider);
            OnPlayAd(_adProvider);
        }

        private IAdProvider SelectAdProvider()
        {
            bool isAll = (playAdParameters.InterstitialReplaceReward && playAdParameters.adType == AdType.Reward)
                         || (playAdParameters.RewardReplaceInterstitial && playAdParameters.adType == AdType.Interstitial);

            IAdProvider _maxEcpmAdprovider = null;
            if (playAdParameters.isPriceRelations)
            {
                foreach (var adProvider in allAdProviders)
                {
                    if (!adProvider.IsReady) continue;
                    if (isAll || adProvider.AdType == playAdParameters.adType)
                    {
                        if ((_maxEcpmAdprovider != null && _maxEcpmAdprovider.ECPM < adProvider.ECPM)
                            || _maxEcpmAdprovider == null)
                        {
                            _maxEcpmAdprovider = adProvider;
                        }
                    }
                }
            }
            else
            {
                foreach (var adProvider in allAdProviders)
                {
                    if (adProvider.ADPlatform != playAdParameters.adPlatform || !adProvider.IsReady) continue;
                    if (adProvider.AdType == playAdParameters.adType)
                    {
                        _maxEcpmAdprovider = adProvider;
                        break;
                    }
                    else if (isAll)
                    {
                        _maxEcpmAdprovider = adProvider;
                    }
                }
            }
            return _maxEcpmAdprovider;
        }

        private void OnStartPlayAd()
        {

        }

        private void OnPlayAd(IAdProvider _adProvider)
        {
            if (_adProvider == null)
            {
                Logger.LogAd("没有可用的广告平台");
                OnEndPlayAd(false);
                return;
            }
            isAdPlaying = true;
            Logger.LogAd("开始播放广告 : " + _adProvider.ADPlatform + _adProvider.AdType);
            _adProvider.ShowAd();
        }

        private void OnEndPlayAd(bool isSucceed)
        {
            if (isAdPlayEnd)
            {
                Logger.LogAd("广告播放结束回调已执行");
                return;
            }

            isAdPlayEnd = true;
            isAdPlaying = false;

            var successCallback = playAdParameters?.SuccessCallback;
            var failCallback = playAdParameters?.FailCallback;

            playAdParameters = null;

            if (isSucceed)
            {
                Logger.LogAd("广告播放成功 : 执行成功回调");
                successCallback?.Invoke();
            }
            else
            {
                Logger.LogAd("广告播放失败 : 执行失败回调");
                failCallback?.Invoke();
            }
        }

        private IAdProvider CheckAdPlayCondition(IAdProvider _adProvider)
        {
            if (_adProvider == null)
            {
                Logger.LogAd("没有可用的广告平台");
                return null;
            }

            if (!playAdParameters.openPlayAdCondition)
            {
                Logger.LogAd("广告播放条件未开启");
                return _adProvider;
            }
            
            // 这里是后续添加广告播放条件的代码
            return _adProvider;
        }

        #region 广告平台回调
        // 广告加载成功回调
        private void OnAdLoaded(string adInfo, AdProviderBase adProviderBase)
        {

        }

        // 广告加载失败回调
        private void OnAdLoadFailedEvent(string errorInfo, AdProviderBase adProviderBase)
        {
        }

        // 广告展示回调
        private void OnAdDisplayed(string adInfo, AdProviderBase adProviderBase)
        {

        }

        // 广告点击回调
        private void OnAdClicked(string adInfo, AdProviderBase adProviderBase)
        {

        }

        // 广告展示失败回调
        private void OnAdDisplayFailed(string adInfo, string errorInfo, AdProviderBase adProviderBase)
        {
            OnEndPlayAd(false);
        }

        // 广告收入回调
        private void OnAdRevenuePaid(string adInfo, AdProviderBase adProviderBase)
        {

        }

        // 广告关闭回调
        private void OnAdHidden(bool completed, string adInfo, AdProviderBase adProviderBase)
        {
            OnEndPlayAd(completed);
        }
    }
        #endregion
}