using System;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Struct
{
    /// <summary>
    /// voicevox_synthesizer_synthesis のオプション。
    /// </summary>
    public struct SynthesisOptions
    {
        /// <summary>
        /// 疑問文の調整を有効にする
        /// </summary>
        public bool EnableInterrogativeUpspeak { get; set; }
    }

    internal static class SynthesisOptionsExt
    {
        internal static VoicevoxSynthesisOptions ToNative(this SynthesisOptions synthesisOptions)
        {
            return new VoicevoxSynthesisOptions
            {
                enable_interrogative_upspeak = synthesisOptions.EnableInterrogativeUpspeak
            };
        }
    }
}
