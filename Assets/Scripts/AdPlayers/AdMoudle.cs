using System.Collections.Generic;
using UnityEngine;
namespace Duskvern
{
    public class AdMoudle
    {
        private readonly HashSet<IAdProvider> allAdProviders = new();

        private readonly HashSet<IAdProvider> canPlayProviders = new();



        public void Init(List<AdPlatformParameters> adPlatformParameters)
        {
            
        }

        public void AddCanPlayProvider(IAdProvider adProvider)
        {
            
        }

        public void PlayAd(AdType adType)
        {
            
        }
    }

    
}