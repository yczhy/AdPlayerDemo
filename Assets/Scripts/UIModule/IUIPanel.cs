using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public static partial class UIPanelName
    {
        
    }

    public abstract class IOpenUIParam
    {
        
    }

    [RequireComponent(typeof(PanelConfig))]
    public abstract class IUIPanel : MonoBehaviour
    {
        public abstract PanelConfig panelConfig { get; }
        protected UITransitionAnimator uITransitionAnimator;

        protected virtual void Awake()
        {
            if (panelConfig == null)
            {
                Logger.LogUIWarning($"UIPanel {typeof(IUIPanel).Name} missing PanelConfig");
            }
            var cfgs = GetComponents<PanelConfig>();
            if (cfgs.Length > 1)
            {
                Logger.LogUIWarning($"UIPanel {typeof(IUIPanel).Name} has more than 1 PanelConfig");
            }
        }

        public virtual async UniTask<IUIPanel> OnOpen(IOpenUIParam openUIParams)
        {
            return this;
        }

        public abstract void OnClose();
    }

    public abstract class IUIPanel<Tparam> : IUIPanel where Tparam : IOpenUIParam
    {
        public sealed override async UniTask<IUIPanel> OnOpen(IOpenUIParam openUIParams)
        {
            if (openUIParams is not Tparam param)
            {
                Logger.LogUI($"UIPanel {typeof(IUIPanel<Tparam>).Name} open with wrong param type {openUIParams.GetType().Name}");
                return this;
            }

            uITransitionAnimator = GetComponent<UITransitionAnimator>();
            if (uITransitionAnimator != null)
            {
                await uITransitionAnimator.OpenUI(OpenUI);
                return this;
            }

            OpenUI();
            return this;

            void OpenUI()
            {
                OnOpen(param);
            }
        }

        protected abstract IUIPanel OnOpen(Tparam openUIParams);
    }
}
