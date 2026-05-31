using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Duskvern
{
    public abstract class AdInitializerBase
    {
        public abstract bool IsInited { get; }
        protected abstract AdPlatform AdPlatform { get; }

        protected AdInitParameters InitParameter(AdConfig _adConfig)
        {
            AdConfig adConfig = _adConfig;
            if (adConfig == null || adConfig.adPlatformParameters == null || adConfig.adPlatformParameters.Count == 0)
            {
                Logger.LogAd("AdConfig is null");
                return new AdInitParameters()
                {
                    isInit = false,
                    openTestMode = false,
                };
            }

            AdPlatformParameters adPlatformParameters = adConfig.adPlatformParameters.FirstOrDefault(p => p.adPlatform == AdPlatform);

            bool openTestMode = adConfig.enableTestAdMode && adPlatformParameters.isEnableTestMode;
            Logger.LogAd($"{AdPlatform} 平台 是否开启测试模式： openTestMode: {openTestMode}");
            return new AdInitParameters()
                {
                    isInit = true,
                    openTestMode = openTestMode,
                };
        }

        public abstract void Init(AdConfig _adConfig);

        public struct AdInitParameters
        {
            public bool isInit;
            public bool openTestMode;
        }
    }
}