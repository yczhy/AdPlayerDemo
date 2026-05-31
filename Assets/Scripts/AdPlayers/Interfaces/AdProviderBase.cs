using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    // 类是所有广告都要有的参数，而接口是外部调用广告的接口
    public class AdProviderBase : IAdProvider
    {
        protected int retryLoadCount = 3; // 广告失败重新加载的次数
        protected int retryLoadInterval = 5; // 广告失败重新加载的间隔时间（秒）
        protected int currentRetryCount = 0; // 当前广告加载失败重试的次数

        protected bool hasRevenuePaid = false;
        protected bool hasRewarded = false;
        protected bool isLoading = false;
        protected bool disposed = false;

        protected double ecpm;
        protected AdPlatform adPlatform;
        protected AdProviderParameters adProviderParameters;

        protected string adUnitId = "你的插屏广告ID";
        protected AdType adType;

        protected void AdEnd()
        {
            hasRevenuePaid = false;
            hasRewarded = false;
            isLoading = false;

            if (disposed)
            {
                return;
            }
            LoadAd();
        }

        protected async UniTask RetryLoadAd(int retryCount)
        {
            await UniTask.Delay(retryCount * retryLoadInterval * 1000);

            if (disposed)
            {
                return;
            }
            LoadAd();
        }

        public AdType AdType => adType;
        public string AdUnitId => adUnitId;
        public double ECPM => ecpm;
        public AdPlatform ADPlatform => adPlatform;

        public virtual bool IsReady { get; }
        public virtual void Init(AdProviderParameters adProviderParameters)
        {
            this.disposed = false;
            this.adProviderParameters = adProviderParameters;
            this.adPlatform = adProviderParameters.adPlatform;
            this.adType = adProviderParameters.adType;
            this.adUnitId = adProviderParameters.adUnitId;
        }
        public virtual void Dispose()
        {
            disposed = true;
            this.adProviderParameters = null;
            this.adPlatform = AdPlatform.None;
            this.adType = AdType.None;
            this.adUnitId = string.Empty;
        }
        public virtual void LoadAd() { }
        public virtual void ShowAd(){  }
    }
}

