using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Core;
using QFramework;
using UnityEngine;

namespace Main.Scripts.UI.Base
{
    /// <summary>
    /// 标记接口：Widget 所需的数据结构
    /// </summary>
    public interface IWidgetData
    {
    }

    /// <summary>
    /// 定义 Widget 的标准生命周期
    /// </summary>
    public interface IWidget<in T> where T : IWidgetData
    {
        void Open(IResourceLoader resourceProvider, T uiData = default);
        void Close();
    }

    /// <summary>
    /// 数据驱动的可复用 UI 单元基类
    /// 生命周期：Init → [Open → Reset → Open → ...] → Close → Release
    /// </summary>
    public abstract class UIWidget<T> : MonoBehaviour, IWidget<T>, IController, ICanLoadResource where T : IWidgetData
    {
        public T Data { get; private set; }

        protected IResourceLoader ResourceProvider { get; private set; }

        private bool _initialized;

        public virtual void Open(IResourceLoader resourceProvider, T data = default)
        {
            if (!_initialized)
            {
                ResourceProvider = resourceProvider;
                OnInit();
                _initialized = true;
            }
            else
            {
                OnReset();
            }

            Data = data;
            OnOpen(data);
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            OnRelease();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 首次初始化（仅调用一次），用于获取组件引用等一次性操作
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// 每次打开时调用，根据 Data 刷新 UI
        /// </summary>
        protected abstract void OnOpen(T data);

        /// <summary>
        /// 重开时调用（非首次），用于清理上一次的数据绑定
        /// </summary>
        protected virtual void OnReset() { }

        /// <summary>
        /// 关闭或销毁时调用，用于释放资源、注销事件
        /// </summary>
        protected virtual void OnRelease() { }

        private void OnDestroy()
        {
            OnRelease();
        }

        public virtual IResourceLoader GetResourceLoader()
        {
            return ResourceProvider;
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
    }
}
