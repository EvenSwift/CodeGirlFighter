using Cysharp.Threading.Tasks;
using CodeFighter.Framework.Controller.Base.Interface;
using CodeFighter.Framework.Core;
using QFramework;
using UnityEngine;

namespace CodeFighter.Framework.Controller.Base
{
    /// <summary>
    /// 需要MonoBehaviour的逻辑控制器
    /// </summary>
    public abstract class MonoController : MonoBehaviour, IInitController
    {
        private bool _isReleased = false;
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
        
            // 如果是代码主动调用 Release，且物体还活着，则执行销毁
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        protected abstract void OnRelease();

        private void OnDestroy()
        {
            // 如果是 Unity 自动销毁（比如切换场景），确保逻辑也得到清理
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
