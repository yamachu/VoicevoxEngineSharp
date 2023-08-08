using System;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Struct
{
    /// <summary>
    /// voicevox_synthesizer_tts のオプション。
    /// </summary>
    public struct TtsOptions
    {
        /// <summary>
        /// AquesTalk風記法としてテキストを解釈する
        /// </summary>
        public bool Kana { get; set; }
        /// <summary>
        /// 疑問文の調整を有効にする
        /// </summary>
        public bool EnableInterrogativeUpspeak { get; set; }
    }

    internal static class TtsOptionsExt
    {
        internal static VoicevoxTtsOptions ToNative(this TtsOptions ttsOptions)
        {
            return new VoicevoxTtsOptions
            {
                kana = ttsOptions.Kana,
                enable_interrogative_upspeak = ttsOptions.EnableInterrogativeUpspeak
            };
        }
    }
}
