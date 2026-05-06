using System;

namespace Main.Scripts.Framework.Controller.Base.Interface
{
    public interface ILoadingContext : IInitContext
    {
        // 提供一个供 Controller 调用的进度更新方法
        void UpdateProgress(float progress);
    }

    // 简单实现一个类
    public class LoadingContext : ILoadingContext
    {
        private readonly Action<float> _onProgress;
        public LoadingContext(Action<float> onProgress) => _onProgress = onProgress;
    
        public void UpdateProgress(float progress) => _onProgress?.Invoke(progress);
    }
}
