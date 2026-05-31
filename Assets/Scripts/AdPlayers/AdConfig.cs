using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    [CreateAssetMenu(fileName = "AdConfig", menuName = "Duskvern/AdConfig")]
    public class AdConfig : ScriptableObject
    {
        public bool enableTestAdMode = false;
        public bool interstitialReplaceReward = false;
        public bool rewardReplaceInterstitial = false;
        public List<AdPlatformParameters> adPlatformParameters = new ();
    }
}