using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duskvern
{
    public enum UIPanelLayer
    {
        BaseLayer = 0,
        PopLayer = 1,
        TeachLayer = 2,
    }

    public class GamePanelParams : IOpenUIParam
    {
        
    }

    [Serializable]
    public class UIPanelConfig
    {
        [ReadOnly] public string panelName; // 名字
        public UIPanelLayer panelLayer; // 层级
        public IUIPanelBase panelPrefab; // 预制体
        public bool mulitopenable; // 是否可以多开
        public bool cacheable; // 是否缓存
    }
}

