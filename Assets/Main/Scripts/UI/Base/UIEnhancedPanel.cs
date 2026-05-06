using System;
using System.Collections.Generic;
using Main.Scripts.Framework.Controller.Base.Interface;
using Main.Scripts.Framework.Core;
using QFramework;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Main.Scripts.UI.Base
{
    /// <summary>
    /// 增强版 UIPanel 基类，集成音效、动画、资源缓存
    /// </summary>
    public abstract class UIEnhancedPanel : UIPanel, IController, IResourceLoader
    {
        [Header("Audio")]
        [SerializeField] private string popSound;
        [SerializeField] private string closeSound;

        private bool _isAnimating;

        public Dictionary<string, AsyncOperationHandle> AssetCaches { get; } = new();

        protected override void OnOpen(IUIData uiData = null)
        {
            base.OnOpen(uiData);

            if (!string.IsNullOrEmpty(popSound))
            {
                AudioKit.PlaySound(popSound);
            }

            PlayOpenAnimation();
        }

        /// <summary>
        /// 带动画的关闭方法。在子类的关闭按钮事件中调用此方法，而不是直接调用 CloseSelf()
        /// </summary>
        protected void CloseWithAnim(Action onComplete)
        {
            if (_isAnimating) return;
            _isAnimating = true;

            if (!string.IsNullOrEmpty(closeSound))
            {
                AudioKit.PlaySound(closeSound);
            }

            PlayCloseAnimation(() =>
            {
                _isAnimating = false;
                onComplete?.Invoke();
            });
        }

        /// <summary>
        /// 入场动画，子类重写
        /// </summary>
        protected virtual void PlayOpenAnimation() { }

        /// <summary>
        /// 退场动画，子类重写。动画完成后必须调用 onComplete
        /// </summary>
        protected virtual void PlayCloseAnimation(Action onComplete)
        {
            onComplete?.Invoke();
        }

        protected override void OnClose()
        {
            ClearCaches();
        }

        private void ClearCaches()
        {
            foreach (var handle in AssetCaches.Values)
            {
                ResourceManager.Release(handle);
            }

            AssetCaches.Clear();
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }

        public IResourceLoader GetResourceLoader()
        {
            return this;
        }
    }
}
