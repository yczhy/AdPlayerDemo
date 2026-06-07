using System.Collections.Generic;
using UnityEngine;

namespace Duskvern
{
    public class UIModule : MonoBehaviour
    {
        // 已经打开的 面板
        private readonly List<IUIPanel> openPanels = new List<IUIPanel>();

        // 用来标识UI层级 和 对应 的 GameObject
        private readonly Dictionary<UIPanelLayer, GameObject> uiLayers = new Dictionary<UIPanelLayer, GameObject>();

        public void Init()
        {
            
        }

        
    }
}

