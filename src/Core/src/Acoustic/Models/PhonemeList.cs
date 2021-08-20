using System.Collections.Generic;
using System.Linq;

namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    internal class PhonemeList
    {
        internal static IEnumerable<string> UnvoicedMoraPhonemes = new List<string> { "A", "I", "U", "E", "O", "cl", "pau" };
        internal static IEnumerable<string> MoraPhonemes = new List<string> { "a", "i", "u", "e", "o", "N" }.Concat(UnvoicedMoraPhonemes);
    }
}
