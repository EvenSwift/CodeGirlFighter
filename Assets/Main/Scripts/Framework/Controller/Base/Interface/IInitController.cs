using Cysharp.Threading.Tasks;
using Main.Scripts.Framework.Core;
using QFramework;

namespace Main.Scripts.Framework.Controller.Base.Interface
{
    public interface IInitController : IController
    {
        UniTask Initialize(IInitContext context = null);
        void Release();
    }

    public interface ICanGetController : ICanGetGameManager
    {
    }

    public static class InitControllerExtension
    {
        /// <summary>
        /// 获取全局控制器
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetGlobalController<T>(this ICanGetController self) where T : IInitController
        {
            var globalControllerSet = self.GetGameManager().ControllerSet;
            return globalControllerSet.Get<T>();
        }

        /// <summary>
        /// 获取游戏场景内控制器
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        // public static T GetGameController<T>(this ICanGetController self) where T : IInitController
        // {
        //     var globalControllerSet = self.GetGameManager().GameProcess.ControllerSet;
        //     return globalControllerSet.Get<T>();
        // }
    }
}
