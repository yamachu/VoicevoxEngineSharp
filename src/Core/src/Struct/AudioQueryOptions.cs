using System;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Struct
{
    /// <summary>
    /// voicevox_synthesizer_audio_query のオプション。
    /// </summary>
    public struct AudioQueryOptions
    {
        /// <summary>
        /// AquesTalk風記法としてテキストを解釈する
        /// </summary>
        public bool Kana { get; set; }

        public static AudioQueryOptions Default()
        {
            return AudioQueryOptionsDefault.Value;
        }
    }

    internal static class AudioQueryOptionsDefault
    {
        public static readonly AudioQueryOptions Value = CoreUnsafe.voicevox_make_default_audio_query_options().FromNative();
    }

    internal static class AudioQueryOptionsExt
    {
        internal static VoicevoxAudioQueryOptions ToNative(this AudioQueryOptions audioQueryOptions)
        {
            return new VoicevoxAudioQueryOptions
            {
                kana = audioQueryOptions.Kana
            };
        }

        internal static AudioQueryOptions FromNative(this VoicevoxAudioQueryOptions audioQueryOptions)
        {
            return new AudioQueryOptions
            {
                Kana = audioQueryOptions.kana
            };
        }
    }
}
