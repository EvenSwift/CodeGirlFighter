using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Model.Data;
using UnityEngine;

namespace Main.Scripts.Entity
{
    /// <summary>
    /// CharacterViewBase 的初始化上下文，传递资源加载器和角色数据
    /// </summary>
    public class CharacterEntityViewInitContext : IInitContext
    {
        public IResourceLoader ResourceLoader { get; }
        public CharacterData Data { get; }

        public CharacterEntityViewInitContext(IResourceLoader resourceLoader, CharacterData data)
        {
            ResourceLoader = resourceLoader;
            Data = data;
        }
    }

    /// <summary>
    /// 角色实体 View，挂载到角色 Prefab 上的具体表现类
    /// </summary>
    public class CharacterEntityView : CharacterViewBase
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public override SpriteRenderer Renderer => spriteRenderer;
        public override Transform RendererTransform => spriteRenderer != null ? spriteRenderer.transform : transform;

        protected override void RefreshView()
        {
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }
    }
}