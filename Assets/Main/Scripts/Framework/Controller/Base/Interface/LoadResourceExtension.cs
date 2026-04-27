using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using CodeFighter.Framework.Core;
using QFramework;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Object = UnityEngine.Object;

namespace CodeFighter.Framework.Controller.Base.Interface
{
    public static class LoadResourceExtension
    {
        /// <summary>
        /// 同步加载资源（内部调用 WaitForCompletion，不可在异步流程中使用）
        /// </summary>
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

        /// <summary>
        /// 异步加载资源（回调版本）
        /// </summary>
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
        /// 异步加载资源（UniTask 版本，支持 async/await 和取消）
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

        /// <summary>
        /// 异步实例化预制体（UniTask 版本），若为 MonoController 则自动调用 Initialize
        /// </summary>
        public static async UniTask<T> InstantiateAsync<T>(this ICanLoadResource loader, string path,
            InstantiationParameters? parameters = null, IInitContext context = null, CancellationToken token = default)
            where T : Object
        {
            var handle = ResourceManager.InstantiateAsync(path, parameters);
            try
            {
                await handle.ToUniTask(cancellationToken: token);

                if (handle.Result == null)
                {
                    throw new InvalidOperationException($"InstantiateAsync failed: result is null. path: {path}");
                }

                var component = handle.Result.GetComponent<T>();
                if (component == null)
                {
                    ResourceManager.ReleaseInstance(handle.Result);
                    throw new InvalidOperationException(
                        $"InstantiateAsync failed: component {typeof(T).Name} not found on prefab. path: {path}");
                }

                if (component is MonoController monoController)
                {
                    await monoController.Initialize(context);
                }

                return component;
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
            catch (InvalidOperationException)
            {
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

        /// <summary>
        /// 清理所有缓存资源
        /// </summary>
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
