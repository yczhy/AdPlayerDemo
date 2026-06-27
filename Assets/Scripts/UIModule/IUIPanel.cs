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
    public abstract class IUIPanelBase : MonoBehaviour
    {
        public abstract PanelConfig panelConfig { get; }
        protected UITransitionAnimator uITransitionAnimator;

        protected virtual void Awake()
        {
            if (panelConfig == null)
            {
                Logger.LogUIWarning($"UIPanel {typeof(IUIPanelBase).Name} missing PanelConfig");
            }
            var cfgs = GetComponents<PanelConfig>();
            if (cfgs.Length > 1)
            {
                Logger.LogUIWarning($"UIPanel {typeof(IUIPanelBase).Name} has more than 1 PanelConfig");
            }
        }

        public virtual async UniTask<IUIPanelBase> OnOpen(IOpenUIParam openUIParams)
        {
            return this;
        }

        public abstract void OnClose();
    }

    public abstract class IUIPanel<Tparam> : IUIPanelBase where Tparam : IOpenUIParam
    {
        public sealed override async UniTask<IUIPanelBase> OnOpen(IOpenUIParam openUIParams)
        {
            if (openUIParams == null)
            {
                Logger.LogUI($"UIPanel {typeof(IUIPanel<Tparam>).Name} open with null param");
                return this;
            }
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

        protected abstract IUIPanelBase OnOpen(Tparam openUIParams);
    }
}
