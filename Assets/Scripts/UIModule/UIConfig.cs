using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Duskvern
{
    public class PanelConfig : MonoBehaviour
    {
        public UIPanelType uIPanelType;
        public UIPanelLayer uIPanelLayer;
        public bool mulitOpen;
        public bool cache;
        public bool nativeClose;
    }

    public enum UIPanelType
    {
        GamePanel,
    }
}
