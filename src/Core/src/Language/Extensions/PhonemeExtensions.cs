using System.Linq;

namespace VoicevoxEngineSharp.Core.Language.Extensions
{
    internal static class PhonemeExtensions
    {
        static readonly string[] vowels = new[] { "a", "i", "u", "e", "o", "A", "I", "U", "E", "O", "N" };
        public static bool IsVowel(this Models.Phoneme phoneme)
        {
            return vowels.Contains(phoneme.GetPhoneme());
        }
    }
}
