using Cysharp.Threading.Tasks;
using Main.Scripts.Framework.Constant;
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

            // 先刷新 Sprite 等视觉表现
            RefreshView();

            // Sprite 加载完成后计算最终位置
            CalculatePosition();

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

        /// <summary>
        /// 根据 Sprite 尺寸和场景常量计算角色最终位置
        /// 左侧角色（player）：贴左边缘，x = -HALF_WORLD_WIDTH + spriteWorldWidth/2
        /// 右侧角色（opponent）：贴右边缘，x = +HALF_WORLD_WIDTH - spriteWorldWidth/2，并水平翻转
        /// </summary>
        private void CalculatePosition()
        {
            if (Renderer == null || Renderer.sprite == null)
            {
                Debug.LogWarning("[CharacterViewBase] CalculatePosition 失败：Renderer 或 Sprite 为空");
                return;
            }

            // Sprite 世界宽度
            var spriteWorldWidth = Renderer.sprite.rect.width / GameConst.PIXELS_PER_UNIT;

            if (Data.isFlipped)
            {
                // 右侧角色（对手），靠右边缘放置，翻转向左面对玩家
                var rightX = GameConst.HALF_WORLD_WIDTH - spriteWorldWidth * 0.5f;
                transform.position = new Vector3(rightX, 0f, 0f);
                Renderer.flipX = true;
            }
            else
            {
                // 左侧角色（玩家），靠左边缘放置，默认朝右
                var leftX = -GameConst.HALF_WORLD_WIDTH + spriteWorldWidth * 0.5f;
                transform.position = new Vector3(leftX, 0f, 0f);
                Renderer.flipX = false;
            }
        }

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