using System.Linq;
using VoicevoxEngineSharp.Core.Language.Domains.Romkan;

namespace VoicevoxEngineSharp.Core.Language.Extensions
{
    internal static class MoraExtensions
    {
        public static string ToText(this Models.Mora mora)
        {
            var joinedMora = string.Join("", mora.Phonemes.Select(p => p.GetPhoneme()));
            return joinedMora._ToTextFromJoinedMora();
        }

        internal static string _ToTextFromJoinedMora(this string joinedMora)
        {
            return joinedMora switch
            {
                "cl" => "ッ",
                "ti" => "ティ",
                "tu" => "トゥ",
                "di" => "ディ",
                "du" => "ドゥ",
                _ => LibRomkan.Instance().ToKatakana(joinedMora),
            };
        }
    }
}
