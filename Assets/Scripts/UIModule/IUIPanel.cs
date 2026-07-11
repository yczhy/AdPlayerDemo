using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public static partial class UIPanelName
    {

    }

    public abstract class IOpenUIParam : IPoolable
    {
        public abstract void OnDeSpawn();

        public abstract void OnSpawn();
    }

    [RequireComponent(typeof(PanelConfig))]
    public abstract class IUIPanelBase : MonoBehaviour
    {
        public abstract PanelConfig panelConfig { get; }
        protected UITransitionAnimator uITransitionAnimator;

        private void Awake()
        {
            InitializePanel();
            OnAwake();
        }

        private void InitializePanel()  // 私有方法，子类无法干扰
        {
            if (panelConfig == null)
            {
                Logger.LogUIWarning($"UIPanel {GetType().Name} missing PanelConfig");
            }
            var cfgs = GetComponents<PanelConfig>();
            if (cfgs.Length > 1)
            {
                Logger.LogUIWarning($"UIPanel {GetType().Name} has more than 1 PanelConfig");
            }
        }

        protected abstract void OnAwake();

        public virtual async UniTask<IUIPanelBase> Open(IOpenUIParam openUIParams)
        {
            return this;
        }

        public virtual async void Close()
        {

        }
    }

    public abstract class IUIPanel<Tparam> : IUIPanelBase where Tparam : IOpenUIParam, new()
    {
        public sealed override PanelConfig panelConfig => GetComponent<PanelConfig>();
        protected IOpenUIParam openUIParam; 

        public sealed override async UniTask<IUIPanelBase> Open(IOpenUIParam openUIParam)
        {
            if (openUIParam == null)
            {
                Logger.LogUI($"UIPanel {typeof(IUIPanel<Tparam>).Name} open with null param");
                return this;
            }
            if (openUIParam is not Tparam param)
            {
                Logger.LogUI($"UIPanel {typeof(IUIPanel<Tparam>).Name} open with wrong param type {openUIParam.GetType().Name}");
                return this;
            }

            this.openUIParam = openUIParam;

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

        public sealed override async void Close()
        {
            uITransitionAnimator = GetComponent<UITransitionAnimator>();
            if (uITransitionAnimator != null)
            {
                await uITransitionAnimator.CloseUI(CloseUI);
                return;
            }

            OnClose();

            void CloseUI()
            {
                ReleaseParam();
                OnClose();
            } 
        }

        protected void ReleaseParam()
        {
            if (openUIParam is Tparam param)
            {
                ClassPool<Tparam>.Push(param);
                openUIParam = null;
            }
        }

        protected abstract void OnClose();
    }
}
