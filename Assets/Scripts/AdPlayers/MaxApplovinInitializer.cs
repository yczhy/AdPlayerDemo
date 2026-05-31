using UnityEngine;

namespace Duskvern
{
    public class MaxApplovinInitializer : AdInitializerBase
    {
        private bool isInited = false;
        public override bool IsInited => isInited;

        protected override AdPlatform AdPlatform => AdPlatform.Applovin;

        public override void Init(AdConfig _adConfig)
        {
            var parameters = InitParameter(_adConfig);
            if (parameters.Equals(null) || !parameters.isInit)
            {
                Logger.LogAd($"{AdPlatform} is not initialized. Please check the parameters.");
                return;
            }

            if (parameters.openTestMode)
            {
                
            }

            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdk.SdkConfiguration sdkConfiguration) =>
            {
                // AppLovin SDK is initialized, start loading ads
                isInited = true;
            };

            MaxSdk.InitializeSdk();
        }
    }
}

