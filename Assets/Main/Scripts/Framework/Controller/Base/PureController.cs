using Cysharp.Threading.Tasks;
using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Core;
using QFramework;

namespace Main.Scripts.Framework.Controller.Base
{
    /// <summary>
    /// 无需MonoBehaviour的逻辑控制器
    /// </summary>
    public abstract class PureController : IInitController
    {
        public async UniTask Initialize(IInitContext context = null)
        {
            await OnInitialize(context);
            OnPostProcess();
        }

        protected virtual UniTask OnInitialize(IInitContext context)
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// 初始化后处理，可用于子类注册交互事件等
        /// </summary>
        protected virtual void OnPostProcess()
        {
        }

        public void Release()
        {
            OnRelease();
        }

        protected abstract void OnRelease();

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}
