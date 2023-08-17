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

        public static AccentPhrasesOptions Default()
        {
            return AccentPhrasesOptionsDefault.Value;
        }
    }

    internal static class AccentPhrasesOptionsDefault
    {
        public static readonly AccentPhrasesOptions Value = CoreUnsafe.voicevox_make_default_accent_phrases_options().FromNative();
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

        internal static AccentPhrasesOptions FromNative(this VoicevoxAccentPhrasesOptions accentPhrasesOptions)
        {
            return new AccentPhrasesOptions
            {
                Kana = accentPhrasesOptions.kana
            };
        }
    }
}
