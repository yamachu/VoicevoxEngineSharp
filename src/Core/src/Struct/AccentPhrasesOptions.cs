using System;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Struct
{
    /// <summary>
    /// voicevox_synthesizer_create_accent_phrases のオプション。
    /// </summary>
    public struct AccentPhrasesOptions
    {
        /// <summary>
        /// AquesTalk風記法としてテキストを解釈する
        /// </summary>
        public bool Kana { get; set; }
    }

    internal static class AccentPhrasesOptionsExt
    {
        internal static VoicevoxAccentPhrasesOptions ToNative(this AccentPhrasesOptions accentPhrasesOptions)
        {
            return new VoicevoxAccentPhrasesOptions
            {
                kana = accentPhrasesOptions.Kana
            };
        }
    }
}
