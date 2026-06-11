using Cysharp.Threading.Tasks;
using Main.Scripts.Framework.Controller.Base;
using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Core;
using Main.Scripts.Framework.Model.Data;
using UnityEngine;

namespace Main.Scripts.Entity
{
    /// <summary>
    /// 角色 View 基类，所有角色表现类的抽象基类
    /// 继承 MonoController，与 Prefab 绑定
    /// </summary>
    public abstract class CharacterViewBase : MonoController, ICanLoadResource, ICanGetGameManager
    {
        protected IResourceLoader ResourceLoader;

        /// <summary>
        /// 角色 SpriteRenderer，子类必须提供
        /// </summary>
        public abstract SpriteRenderer Renderer { get; }

        /// <summary>
        /// 渲染器的 Transform，用于动画、翻转等操作
        /// </summary>
        public abstract Transform RendererTransform { get; }

        /// <summary>
        /// 当前角色数据引用
        /// </summary>
        public CharacterData Data { get; private set; }

        #region LifeCycle

        protected override UniTask OnInitialize(IInitContext context)
        {
            var initContext = (CharacterEntityViewInitContext)context;
            ResourceLoader = initContext.ResourceLoader;
            Data = initContext.Data;

            transform.position = Data.position;
            RefreshView();

            return UniTask.CompletedTask;
        }

        protected override void OnRelease()
        {
            Data = null;
            ResourceLoader = null;
        }

        #endregion

        /// <summary>
        /// 刷新角色视觉表现（Sprite、翻转、动画等），子类必须实现
        /// </summary>
        protected abstract void RefreshView();

        #region ICanLoadResource

        public IResourceLoader GetResourceLoader()
        {
            return ResourceLoader;
        }

        #endregion

        #region ICanGetGameManager

        public GameManager GetGameManager()
        {
            return GameManager.Instance;
        }

        #endregion
    }
}