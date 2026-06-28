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
        private readonly List<IUIPanelBase> openPanels = new List<IUIPanelBase>();

        // 用来标识UI层级 和 对应 的 GameObject
        private readonly Dictionary<UIPanelLayer, GameObject> uiLayers = new Dictionary<UIPanelLayer, GameObject>();

        [SerializeField] private Transform uiRoot; // UI根节点

        private Dictionary<UIPanelType, GameObject> uiPanelPrefabs = new();

        private const string UIPrefabPath = "Prefabs/UIPanel/";

        private void Awake()
        {
            if (uiRoot == null)
            {
                var obj = new GameObject("UIRoot");
                uiRoot = obj.transform;
            }

            for (int i = 0; i < Enum.GetValues(typeof(UIPanelLayer)).Length; i++)
            {
                var layer = (UIPanelLayer)i;
                var layerObj = new GameObject(layer.ToString());
                layerObj.transform.SetParent(uiRoot);
                uiLayers.Add(layer, layerObj);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                int openPanelCount = openPanels.Count;
                for (int i = openPanelCount - 1; i >= 0; i--)
                {
                    var panel = openPanels[i];
                    if (panel.panelConfig.nativeClose)
                    {
                        ClosePanel(panel).Forget();
                    }
                }
            }
        }

        public async UniTask<IUIPanelBase> OpenPanel(UIPanelType uIPanelType, IOpenUIParam _openUIParams)
        {
            IUIPanelBase beOpenPanel = null;
            var openedPanel = openPanels.FirstOrDefault(x => x.panelConfig.uIPanelType == uIPanelType);
            bool isMultiOpen = openedPanel != null ? openedPanel.panelConfig.multiOpen : false;
            PanelConfig cfg = openedPanel != null ? openedPanel.panelConfig : null;
            if (openedPanel == null || isMultiOpen)
            {
                beOpenPanel = await CreatePanelInstance(uIPanelType);
                if (beOpenPanel == null)
                {
                    Debug.LogError($"UIPanel {uIPanelType} 预制体不存在");
                    return null;
                }
                cfg = beOpenPanel.panelConfig;
            }
            else
            {
                cfg = openedPanel.panelConfig;
                if (!isMultiOpen)
                {
                    Logger.LogUI($"UIPanel {uIPanelType} 已经打开");
                    openedPanel.transform.localScale = Vector3.one;
                    openedPanel.transform.position = Vector3.zero;
                    beOpenPanel = openedPanel;
                    openPanels.Remove(openedPanel);
                }
            }

            if (uIPanelType != cfg.uIPanelType)
            {
                Debug.LogError($"UIPanel {uIPanelType} 和 {cfg.uIPanelType} 不一致");
                return null;
            }

            UIPanelLayer layer = cfg.uIPanelLayer;
            if (uiLayers.TryGetValue(layer, out var layerObj))
            {
                beOpenPanel.transform.SetParent(layerObj.transform);
                await beOpenPanel.Open(_openUIParams);
            }
            else
            {
                Debug.LogError($"UIPanelLayer {layer} 不存在");
            }

            openPanels.Add(beOpenPanel);
            return beOpenPanel;
        }


        public async UniTask ClosePanel(IUIPanelBase panel)
        {
            if (panel == null) return;
            openPanels.Remove(panel);
            PoolUtil.DeSpawn(panel.gameObject);
            panel.Close();
        }

        private async UniTask<IUIPanelBase> CreatePanelInstance(UIPanelType uIPanelType)
        {
            IUIPanelBase beOpenPanelObj = null;
            GameObject panelPrefab = null;
            PanelConfig cfg = null;

            if (!uiPanelPrefabs.TryGetValue(uIPanelType, out panelPrefab))
            {
                string path = UIPrefabPath + uIPanelType.ToString();
                panelPrefab = await AssetUtils.LoadAsync<GameObject>(path);
            }

            if (panelPrefab == null)
            {
                Debug.LogError($"UIPanel {uIPanelType} 预制体不存在");
                return null;
            }

            var obj = PoolUtil.Spawn(panelPrefab);
            if (obj == null)
            {
                Debug.LogError($"UIPanel {uIPanelType} 预制体实例化失败");
                return null;
            }

            if (obj.TryGetComponent(out beOpenPanelObj))
            {
                cfg = beOpenPanelObj.panelConfig;
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

            if (cfg.cache && !uiPanelPrefabs.ContainsKey(uIPanelType))
            {
                uiPanelPrefabs.Add(uIPanelType, panelPrefab);
            }
            return beOpenPanelObj;
        }
    }
}

