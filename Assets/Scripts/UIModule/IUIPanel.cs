using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public static partial class UIPanelName
    {
        
    }

    public abstract class IOpenUIParams
    {
        
    }

    public abstract class IUIPanel : MonoBehaviour
    {
        public abstract string uiName { get; }
        protected UITransitionAnimator uITransitionAnimator;

        protected virtual void Awake(){ }

        public abstract void OnOpen(IOpenUIParams openUIParams);

        public abstract void OnClose();
    }

    public abstract class IUIPanel<Tparam> : IUIPanel where Tparam : IOpenUIParams
    {
        public sealed override void OnOpen(IOpenUIParams openUIParams)
        {
            if (openUIParams is not Tparam param)
            {
                Logger.LogUI($"UIPanel {uiName} open with wrong param type {openUIParams.GetType().Name}");
                return;
            }

            uITransitionAnimator = GetComponent<UITransitionAnimator>();
            if (uITransitionAnimator != null)
            {
                uITransitionAnimator.OpenUI(OpenUI).Forget();
                return;
            }

            OpenUI();

            void OpenUI()
            {
                OnOpen(param);
            }
        }

        protected abstract void OnOpen(Tparam openUIParams);
    }
}
