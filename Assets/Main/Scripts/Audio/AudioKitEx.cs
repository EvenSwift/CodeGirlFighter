using System;
using QFramework;
using UnityEngine.Audio;

namespace Main.Scripts.Audio
{
    /// <summary>
    /// AudioKit 扩展：集成 AudioMixer 分组路由
    /// </summary>
    public static class AudioKitEx
    {
        private static AudioMixerGroup _musicGroup;
        private static AudioMixerGroup _soundGroup;
        private static AudioMixerGroup _voiceGroup;
        private static bool _initialized;

        /// <summary>
        /// 初始化 AudioMixer 分组。Music/Voice 在 Init 时绑定一次（持久化播放器），Sound 在每次播放时绑定（池化播放器）
        /// </summary>
        public static void Init(AudioMixerGroup music, AudioMixerGroup sound, AudioMixerGroup voice)
        {
            _musicGroup = music;
            _soundGroup = sound;
            _voiceGroup = voice;

            // MusicPlayer 和 VoicePlayer 是持久化单例，Init 时绑定一次即可
            if (_musicGroup != null)
            {
                AudioKit.MusicPlayer.SetGroup(_musicGroup);
            }

            if (_voiceGroup != null)
            {
                AudioKit.VoicePlayer.SetGroup(_voiceGroup);
            }

            _initialized = true;
        }

        #region Music

        public static void PlayMusic(string clipName, bool loop = true, Action onBegin = null, Action onEnd = null,
            float volume = 1f)
        {
            AudioKit.PlayMusic(clipName, loop, onBegin, onEnd, volume);

            if (_initialized && _musicGroup != null)
            {
                AudioKit.MusicPlayer.SetGroup(_musicGroup);
            }
        }

        public static void StopMusic() => AudioKit.StopMusic();
        public static void PauseMusic() => AudioKit.PauseMusic();
        public static void ResumeMusic() => AudioKit.ResumeMusic();

        #endregion

        #region Sound

        /// <summary>
        /// 播放音效。Sound 播放器是池化的，每次播放绑定 Group
        /// </summary>
        public static AudioPlayer PlaySound(string clipName, bool loop = false,
            Action<AudioPlayer> callBack = null, float volumeScale = 1f)
        {
            var player = AudioKit.PlaySound(clipName, loop, callBack, volumeScale);

            if (player != null && _soundGroup != null)
            {
                player.SetGroup(_soundGroup);
            }

            return player;
        }

        public static void StopAllSound() => AudioKit.StopAllSound();

        #endregion

        #region Voice

        public static void PlayVoice(string clipName, bool loop = false, Action onBegin = null,
            Action onEnd = null)
        {
            AudioKit.PlayVoice(clipName, loop, onBegin, onEnd);

            if (_initialized && _voiceGroup != null)
            {
                AudioKit.VoicePlayer.SetGroup(_voiceGroup);
            }
        }

        public static void StopVoice() => AudioKit.StopVoice();

        #endregion
    }
}
