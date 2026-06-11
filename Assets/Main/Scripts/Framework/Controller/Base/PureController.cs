using Cysharp.Threading.Tasks;
using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Core;
using QFramework;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    /// <summary>
    /// 支持资源预加载的异步逻辑控制器
    /// Controller 初始化时可预加载 Addressable 资源，资源生命周期与该 Controller 绑定
    /// </summary>
    public abstract class SyncPureController : PureController, IResourceLoader
    {
        public Dictionary<string, AsyncOperationHandle> AssetCaches { get; } = new();

        public IResourceLoader GetResourceLoader()
        {
            return this;
        }
    }
}