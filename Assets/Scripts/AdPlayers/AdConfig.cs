using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    [CreateAssetMenu(fileName = "AdConfig", menuName = "Duskvern/AdConfig")]
    public class AdConfig : ScriptableObject
    {
        public List<AdPlatformParameters> adPlatformParameters = new ();
    }
}