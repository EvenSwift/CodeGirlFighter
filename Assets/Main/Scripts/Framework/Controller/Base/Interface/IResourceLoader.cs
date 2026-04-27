using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeFighter.Framework.Controller.Base.Interface
{
    /// <summary>
    /// 实现该接口以加载资源，并管理资源缓存
    /// 适用于界面
    /// </summary>
    public interface IResourceLoader : ICanLoadResource
    {
        Dictionary<string, AsyncOperationHandle> AssetCaches { get; }
    }

    /// <summary>
    /// 实现该接口以加载资源，需要依赖IResourceLoader实例
    /// 适用于界面内的子对象，ItemView等
    /// </summary>
    public interface ICanLoadResource
    {
        IResourceLoader GetResourceLoader();
    }
}
