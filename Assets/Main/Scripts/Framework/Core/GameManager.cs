using System;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeFighter.Framework.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameArchitecture Architecture { get; private set; }
        public bool IsInitialized { get; private set; }

        private async void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            await Initialize();
        }

        private async System.Threading.Tasks.Task Initialize()
        {
            try
            {
                Architecture = GameArchitecture.Interface as GameArchitecture;
                await Addressables.InitializeAsync().Task;
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GameManager] 初始化失败\n{ex}");
            }
        }

        private void OnApplicationQuit()
        {
            Release();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Release();
                Instance = null;
            }
        }

        private void Release()
        {
            if (!IsInitialized || Architecture == null)
            {
                return;
            }

            Architecture.Deinit();
            IsInitialized = false;
        }
    }
}
