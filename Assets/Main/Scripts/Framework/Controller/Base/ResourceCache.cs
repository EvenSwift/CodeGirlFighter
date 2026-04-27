using System.Collections.Generic;
using CodeFighter.Framework.Controller.Base.Interface;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeFighter.Framework.Controller.Base
{
    /// <summary>
    /// 可复用的资源缓存，通过组合方式提供给任何控制器使用
    /// </summary>
    public class ResourceCache : IResourceLoader
    {
        public Dictionary<string, AsyncOperationHandle> AssetCaches { get; } = new();

        public IResourceLoader GetResourceLoader()
        {
            return this;
        }
    }
}
