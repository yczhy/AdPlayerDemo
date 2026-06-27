using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public class UIModule : MonoBehaviour
    {
        // 已经打开的 面板
        private readonly List<IUIPanel> openPanels = new List<IUIPanel>();

        // 用来标识UI层级 和 对应 的 GameObject
        private readonly Dictionary<UIPanelLayer, GameObject> uiLayers = new Dictionary<UIPanelLayer, GameObject>();

        [SerializeField] private Transform uiRoot; // UI根节点

        public Dictionary<UIPanelType, IUIPanel> uiPanelPrefabs = new();

        public async UniTask<T> OpenPanel<T>(UIPanelType uIPanelType, IOpenUIParam _openUIParams) where T : IUIPanel
        {
            T beOpenPanel = null;
            PanelConfig cfg = null;
            if (uiPanelPrefabs.ContainsKey(uIPanelType))
            {
                beOpenPanel = uiPanelPrefabs[uIPanelType] as T;
            }
            else
            {
                var obj = await AssetPoolUtils.SpawnAsync(uIPanelType.ToString());
                if (obj.TryGetComponent(out beOpenPanel))
                {
                    cfg = beOpenPanel.panelConfig;
                    if (cfg == null)
                    {
                        Debug.LogError($"UIPanel {uIPanelType} 没有配置");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError($"UIPanel {uIPanelType} 没有实现 IUIPanel 接口");
                    return null;
                }

                if (cfg.cache)
                {
                    uiPanelPrefabs.Add(uIPanelType, beOpenPanel);
                }
            }
            bool isMulitOpen = cfg.mulitOpen;
            if (!isMulitOpen && openPanels.Contains(beOpenPanel))
            {
                Logger.LogUI($"UIPanel {uIPanelType} 已经打开");
                beOpenPanel = openPanels.FirstOrDefault(x => x.panelConfig.uIPanelType == uIPanelType) as T;
                beOpenPanel.transform.localScale = Vector3.one;
                beOpenPanel.transform.position = Vector3.zero;
            }

            UIPanelLayer layer = cfg.uIPanelLayer;
            if (uiLayers.TryGetValue(layer, out var layerObj))
            {
                beOpenPanel.transform.SetParent(layerObj.transform);
                await beOpenPanel.OnOpen(_openUIParams);
            }
            else
            {
                Debug.LogError($"UIPanelLayer {layer} 不存在");
            }

            return beOpenPanel;
        }
    }
}

