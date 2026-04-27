using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using CodeFighter.Framework.Controller.Base.Interface;
using CodeFighter.Framework.Core;
using QFramework;

namespace CodeFighter.Framework.Controller
{
    public class ControllerSet : IInitController, IUpdate
    {
        public List<IInitController> Controllers { get; private set; } = new();
        private readonly Dictionary<Type, IInitController> _controllerMap = new();
        private readonly HashSet<IUpdate> _updateControllers = new();

        public async UniTask Initialize(IInitContext context = null)
        {
            var loadingContext = context as ILoadingContext;

            int total = Controllers.Count;
            for (int i = 0; i < total; i++)
            {
                await Controllers[i].Initialize(context);

                float progress = (float)(i + 1) / total;
                loadingContext?.UpdateProgress(progress);
            }
        }

        public void Add(IInitController controller)
        {
            Controllers.Add(controller);

            var type = controller.GetType();
            if (!_controllerMap.ContainsKey(type))
            {
                _controllerMap[type] = controller;
            }

            if (controller is IUpdate updatable)
            {
                _updateControllers.Add(updatable);
            }
        }

        public T Get<T>() where T : IInitController
        {
            if (_controllerMap.TryGetValue(typeof(T), out var controller))
            {
                return (T)controller;
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
            _controllerMap.Clear();
            _updateControllers.Clear();
        }

        public IArchitecture GetArchitecture()
        {
            return GameArchitecture.Interface;
        }

        public void Update()
        {
            foreach (var controller in _updateControllers)
            {
                controller.Update();
            }
        }
    }
}
