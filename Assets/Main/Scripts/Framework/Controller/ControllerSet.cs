using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using CodeFighter.Framework.Controller.Base.Interface;
using CodeFighter.Framework.Core;
using QFramework;

namespace CodeFighter.Framework.Controller
{
    [Serializable]
    public class ControllerSet : IInitController, IUpdate
    {
        public List<IInitController> Controllers { get; private set; } = new();
        private List<IUpdate> _updateControllers = new();
        
        public async UniTask Initialize(IInitContext context = null)
        {
            // 尝试转换，看看是不是带进度功能的 Context
            var loadingContext = context as ILoadingContext;

            int total = Controllers.Count;
            for (int i = 0; i < total; i++)
            {
                // 1. 调用子 Controller 原始的 Initialize
                await Controllers[i].Initialize(context);

                if (Controllers[i] is IUpdate updateController)
                {
                    if (!_updateControllers.Contains(updateController))
                        _updateControllers.Add(updateController);
                }

                // 2. 如果支持进度，则反馈给外部
                float progress = (float)(i + 1) / total;
                loadingContext?.UpdateProgress(progress);
            }
        }

        public void Add(IInitController controller)
        {
            Controllers.Add(controller);
        }

        public T Get<T>() where T : IInitController
        {
            foreach (var controller in Controllers)
            {
                if (controller is T t)
                {
                    return t;
                }
            }

            return default;
        }

        public void Release()
        {
            foreach (var controller in Controllers)
            {
                controller.Release();
            }

            Controllers.Clear();
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }
        
        public void Update()
        {
            // Debug.Log("Update Controllers");
            foreach (var controller in _updateControllers)
            {
                controller.Update();
            }
        }
    }
}
