using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CodeFighter.Framework.Core
{
    public interface IResourceStrategy
    {
        AsyncOperationHandle<T> LoadAsset<T>(string path);
        AsyncOperationHandle<T> LoadAssetAsync<T>(string path);
        AsyncOperationHandle<T> LoadAssetAsync<T>(string path, Action<T> onComplete);
        AsyncOperationHandle<GameObject> InstantiateAsync(string path, InstantiationParameters? parameters);
        AsyncOperationHandle<GameObject> InstantiateAsync(
            string path,
            InstantiationParameters? parameters,
            Action<GameObject> onComplete);
        void Release(AsyncOperationHandle handle);
        void ReleaseInstance(GameObject obj);
    }

    public interface ISceneLoader
    {
        AsyncOperationHandle<SceneInstance> LoadSceneAsync(
            string path,
            LoadSceneMode loadMode,
            Action onComplete);
        void UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handle);
    }

    public static class ResourceManager
    {
        private static readonly IResourceStrategy Strategy = new AddressableStrategy();

        public static AsyncOperationHandle<T> LoadAsset<T>(string path)
        {
            return Strategy.LoadAsset<T>(path);
        }

        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string path)
        {
            return Strategy.LoadAssetAsync<T>(path);
        }

        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string path, Action<T> onComplete)
        {
            return Strategy.LoadAssetAsync(path, onComplete);
        }

        public static AsyncOperationHandle<GameObject> InstantiateAsync(
            string path,
            InstantiationParameters? parameters = null)
        {
            return Strategy.InstantiateAsync(path, parameters);
        }

        public static AsyncOperationHandle<GameObject> InstantiateAsync(
            string path,
            InstantiationParameters? parameters,
            Action<GameObject> onComplete)
        {
            return Strategy.InstantiateAsync(path, parameters, onComplete);
        }

        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(
            string path,
            LoadSceneMode loadMode = LoadSceneMode.Single,
            Action onComplete = null)
        {
            return ((ISceneLoader)Strategy).LoadSceneAsync(path, loadMode, onComplete);
        }

        public static void UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handle)
        {
            ((ISceneLoader)Strategy).UnloadSceneAsync(handle);
        }

        public static void Release(AsyncOperationHandle handle)
        {
            Strategy.Release(handle);
        }

        public static void ReleaseInstance(GameObject obj)
        {
            Strategy.ReleaseInstance(obj);
        }
    }

    public class AddressableStrategy : IResourceStrategy, ISceneLoader
    {
        public AsyncOperationHandle<T> LoadAsset<T>(string path)
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            handle.WaitForCompletion();
            return handle;
        }

        public AsyncOperationHandle<T> LoadAssetAsync<T>(string path)
        {
            return Addressables.LoadAssetAsync<T>(path);
        }

        public AsyncOperationHandle<T> LoadAssetAsync<T>(string path, Action<T> onComplete)
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            handle.Completed += completedHandle =>
            {
                if (completedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    onComplete?.Invoke(completedHandle.Result);
                }
            };
            return handle;
        }

        public AsyncOperationHandle<GameObject> InstantiateAsync(
            string path,
            InstantiationParameters? parameters)
        {
            return parameters.HasValue
                ? Addressables.InstantiateAsync(path, parameters.Value)
                : Addressables.InstantiateAsync(path);
        }

        public AsyncOperationHandle<GameObject> InstantiateAsync(
            string path,
            InstantiationParameters? parameters,
            Action<GameObject> onComplete)
        {
            var handle = InstantiateAsync(path, parameters);
            handle.Completed += completedHandle =>
            {
                if (completedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    onComplete?.Invoke(completedHandle.Result);
                }
            };
            return handle;
        }

        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(
            string path,
            LoadSceneMode loadMode,
            Action onComplete)
        {
            var handle = Addressables.LoadSceneAsync(path, loadMode);
            handle.Completed += completedHandle =>
            {
                if (completedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    onComplete?.Invoke();
                }
            };
            return handle;
        }

        public void UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handle)
        {
            Addressables.UnloadSceneAsync(handle);
        }

        public void Release(AsyncOperationHandle handle)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }

        public void ReleaseInstance(GameObject obj)
        {
            if (obj != null)
            {
                Addressables.ReleaseInstance(obj);
            }
        }
    }
}
