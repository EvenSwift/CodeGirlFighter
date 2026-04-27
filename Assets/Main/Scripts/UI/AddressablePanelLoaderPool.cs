using System;
using System.IO;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeFighter.UI
{
    public class AddressablePanelLoaderPool : AbstractPanelLoaderPool
    {
        public class AddressablePanelLoader : IPanelLoader
        {
            private const string UIPath = "Assets/Main/Addressable/Prefabs/UI/";
            private AsyncOperationHandle<GameObject> _handle;

            public GameObject LoadPanelPrefab(PanelSearchKeys panelSearchKeys)
            {
                var name = string.IsNullOrEmpty(panelSearchKeys.GameObjName)
                    ? panelSearchKeys.PanelType.Name
                    : panelSearchKeys.GameObjName;
                _handle = Addressables.LoadAssetAsync<GameObject>(Path.Combine(UIPath, $"{name}.prefab"));
                _handle.WaitForCompletion();
                return _handle.Result;
            }

            public void LoadPanelPrefabAsync(PanelSearchKeys panelSearchKeys, Action<GameObject> onPanelPrefabLoad)
            {
                var name = string.IsNullOrEmpty(panelSearchKeys.GameObjName)
                    ? panelSearchKeys.PanelType.Name
                    : panelSearchKeys.GameObjName;
                _handle = Addressables.LoadAssetAsync<GameObject>(Path.Combine(UIPath, $"{name}.prefab"));
                _handle.Completed += _ => { onPanelPrefabLoad(_handle.Result); };
            }

            public void Unload()
            {
                Addressables.Release(_handle);
            }
        }

        protected override IPanelLoader CreatePanelLoader()
        {
            return new AddressablePanelLoader();
        }
    }
}
