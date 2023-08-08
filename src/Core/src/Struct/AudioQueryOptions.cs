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
    }
}
