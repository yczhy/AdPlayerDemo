using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public abstract class IUIPanel : MonoBehaviour
    {
        public abstract string uiName { get; }

        protected virtual void Awake()
        {
            
        }

        public async UniTask OpenAnimation()
        {
            
        }

        // 方式一
        public abstract void OnOpen(IOpenUIParams openUIParams);

        public void CloseAnimation()
        {
            
        }

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

            OnOpen(param);
        }

        protected abstract void OnOpen(Tparam openUIParams);
    }
}
