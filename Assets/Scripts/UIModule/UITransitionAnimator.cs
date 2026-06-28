using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Duskvern
{
    public class UITransitionAnimator : MonoBehaviour
    {
        public async UniTask OpenUI(Action _openAction)
        {
            var openAction = _openAction;
            if (openAction == null)
            {
                Logger.LogUI("openAction is null");
                return;
            }


            openAction.Invoke();
        }

        public async UniTask CloseUI(Action _closeAction)
        {
            var closeAction = _closeAction;
            if (closeAction == null)
            {
                Logger.LogUI("closeAction is null");
                return;
            }

            closeAction.Invoke();
        }
    }
}

