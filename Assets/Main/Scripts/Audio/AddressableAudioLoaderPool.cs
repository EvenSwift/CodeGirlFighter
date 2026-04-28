using System;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeFighter.Audio
{
    public class AddressableAudioLoaderPool : AbstractAudioLoaderPool
    {
        protected override IAudioLoader CreateLoader()
        {
            return new AddressableAudioLoader();
        }
    }

    public class AddressableAudioLoader : IAudioLoader
    {
        private AsyncOperationHandle<AudioClip> _handle;
        public AudioClip Clip { get; private set; }

        public AudioClip LoadClip(AudioSearchKeys audioSearchKeys)
        {
            _handle = Addressables.LoadAssetAsync<AudioClip>(audioSearchKeys.AssetName);
            _handle.WaitForCompletion();
            Clip = _handle.Result;
            return Clip;
        }

        public void LoadClipAsync(AudioSearchKeys audioSearchKeys, Action<bool, AudioClip> onLoad)
        {
            _handle = Addressables.LoadAssetAsync<AudioClip>(audioSearchKeys.AssetName);
            _handle.Completed += _ =>
            {
                Clip = _handle.Result;
                onLoad(_handle.IsValid(), Clip);
            };
        }

        public void Unload()
        {
            Addressables.Release(_handle);
        }
    }
}
