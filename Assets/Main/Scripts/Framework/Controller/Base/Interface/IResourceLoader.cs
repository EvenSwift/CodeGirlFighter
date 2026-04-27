using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using CodeFighter.Framework.Core;
using QFramework;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

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

    public static class LoadResourceExtension
    {
        public static T LoadAsset<T>(this ICanLoadResource subLoader, string path) where T : Object
        {
            var loader = subLoader.GetResourceLoader();
            if (loader.AssetCaches.TryGetValue(path, out var cacheHandle))
            {
                return cacheHandle.Result.As<T>();
            }
            var handle = ResourceManager.LoadAsset<T>(path);
            loader.AssetCaches.Add(path, handle);
            return handle.Result.As<T>();
        }
        public static AsyncOperationHandle LoadAssetAsync<T>(this ICanLoadResource subLoader, string path,
            Action<T> onComplete) where T : Object
        {
            var loader = subLoader.GetResourceLoader();
            if (loader.AssetCaches.TryGetValue(path, out var cacheHandle))
            {
                if (cacheHandle.IsDone)
                {
                    onComplete?.Invoke(cacheHandle.Result as T);
                }
                else
                {
                    cacheHandle.Completed += _ => onComplete?.Invoke(cacheHandle.Result as T);
                }

                return cacheHandle;
            }

            var handle = ResourceManager.LoadAssetAsync(path, onComplete);
            loader.AssetCaches.Add(path, handle);
            return handle;
        }

        /// <summary>
        /// UniTask 版本（支持 async/await 和取消）
        /// </summary>
        public static async UniTask<T> LoadAssetAsync<T>(this ICanLoadResource subLoader, string path,
            CancellationToken token = default) where T : Object
        {
            var loader = subLoader.GetResourceLoader();
            if (loader.AssetCaches.TryGetValue(path, out var cacheHandle))
            {
                var result = cacheHandle.Result as T;
                if (cacheHandle.IsDone)
                {
                    if (result)
                    {
                        return result;
                    }

                    ResourceManager.Release(cacheHandle);
                    loader.AssetCaches.Remove(path);
                }
                else
                {
                    await cacheHandle.ToUniTask(cancellationToken: token);
                    return cacheHandle.Result as T;
                }
            }

            var handle = ResourceManager.LoadAssetAsync<T>(path);
            try
            {
                loader.AssetCaches.Add(path, handle);
                await handle.ToUniTask(cancellationToken: token);
                return handle.Result;
            }
            catch (OperationCanceledException)
            {
                LogKit.I($"LoadAssetAsync was canceled. path: {path}");
                loader.AssetCaches.Remove(path);
                ResourceManager.Release(handle);
                throw;
            }
            catch (Exception ex)
            {
                LogKit.E($"LoadAssetAsync error: {ex.Message}, path: {path}");
                loader.AssetCaches.Remove(path);
                ResourceManager.Release(handle);
                throw;
            }
        }

        #region 实例化

        public static async UniTask<T> InstantiateAsync<T>(this ICanLoadResource loader, string path,
            InstantiationParameters? parameters = null, IInitContext context = null, CancellationToken token = default)
            where T : Object
        {
            var handle = ResourceManager.InstantiateAsync(path, parameters);
            try
            {
                await handle.ToUniTask(cancellationToken: token);
                var controller = handle.Result.GetComponent<T>();
                if (controller is MonoController monoController)
                {
                    await monoController.Initialize(context);
                }

                return controller;
            }
            catch (OperationCanceledException)
            {
                LogKit.I($"InstantiateAsync was canceled. path: {path}");
                if (handle.IsValid())
                {
                    ResourceManager.ReleaseInstance(handle.Result);
                }

                throw;
            }
            catch (Exception ex)
            {
                LogKit.E($"InstantiateAsync error: {ex.Message}, path: {path}");
                if (handle.IsValid())
                {
                    ResourceManager.ReleaseInstance(handle.Result);
                }

                throw;
            }
        }

        #endregion


        public static void ClearCaches(this ICanLoadResource subLoader)
        {
            var loader = subLoader.GetResourceLoader();
            if (loader.AssetCaches.Count == 0)
            {
                return;
            }

            foreach (var handle in loader.AssetCaches.Values)
            {
                ResourceManager.Release(handle);
            }

            loader.AssetCaches.Clear();
        }
    }
}
