using System;
using CodeFighter.Framework.Controller;
using CodeFighter.Framework.Controller.Base.Interface;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeFighter.Framework.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public ControllerSet ControllerSet { get; private set; }
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
            Initialize();
        }

        private void Update()
        {
            if (ControllerSet == null) return;
            ControllerSet.Update();
        }

        private async void Initialize()
        {
            try
            {
                Architecture = GameArchitecture.Interface as GameArchitecture;

                SetControllers();

                await Addressables.InitializeAsync().Task;

                var ctx = new LoadingContext(progress => { /* 加载进度回调 */ });
                await ControllerSet.Initialize(ctx);

                IsInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GameManager] 初始化失败\n{ex}");
            }
        }

        protected virtual void SetControllers()
        {
            ControllerSet = new ControllerSet();
            // 子类在此注册具体控制器
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

            ControllerSet?.Release();
            Architecture.Deinit();
            IsInitialized = false;
        }
    }
}
