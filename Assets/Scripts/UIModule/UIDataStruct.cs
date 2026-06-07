using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duskvern
{
    public enum UIPanelLayer
    {
        BaseLayer,
        PopLayer,
        TeachLayer,
    }

    public class GamePanelParams : IOpenUIParams
    {
        
    }

    [Serializable]
    public class UIPanelConfig
    {
        [ReadOnly] public string panelName; // 名字
        public UIPanelLayer panelLayer; // 层级
        public IUIPanel panelPrefab; // 预制体
        public bool mulitopenable; // 是否可以多开
        public bool cacheable; // 是否缓存
    }
}

