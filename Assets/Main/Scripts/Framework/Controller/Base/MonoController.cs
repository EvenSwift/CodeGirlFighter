using Cysharp.Threading.Tasks;
using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Core;
using QFramework;
using UnityEngine;

namespace Main.Scripts.Framework.Controller.Base
{
    /// <summary>
    /// 需要MonoBehaviour的逻辑控制器
    /// </summary>
    public abstract class MonoController : MonoBehaviour, IInitController
    {
        private bool _isReleased;

        public virtual async UniTask Initialize(IInitContext context = null)
        {
            await OnInitialize(context);
        }

        protected virtual UniTask OnInitialize(IInitContext context)
        {
            return UniTask.CompletedTask;
        }

        public void Release()
        {
            if (_isReleased) return;
            _isReleased = true;

            OnRelease();
        }

        protected abstract void OnRelease();

        private void OnDestroy()
        {
            if (!_isReleased)
            {
                _isReleased = true;
                OnRelease();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}
