using System.Collections.Generic;
using System.Linq;

namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    internal class OjtPhoneme : BasePhoneme
    {
        internal static new IList<string> phonemeList = new List<string>{
            "pau",
            "A",
            "E",
            "I",
            "N",
            "O",
            "U",
            "a",
            "b",
            "by",
            "ch",
            "cl",
            "d",
            "dy",
            "e",
            "f",
            "g",
            "gw",
            "gy",
            "h",
            "hy",
            "i",
            "j",
            "k",
            "kw",
            "ky",
            "m",
            "my",
            "n",
            "ny",
            "o",
            "p",
            "py",
            "r",
            "ry",
            "s",
            "sh",
            "t",
            "ts",
            "ty",
            "u",
            "v",
            "w",
            "y",
            "z",
        };

        internal static int numPhoneme = phonemeList.Count;

        internal static string spacePhoneme = "pau";

        public OjtPhoneme(string phoneme, float start, float end) : base(phoneme, start, end)
        {
        }

        public static IEnumerable<OjtPhoneme> Convert(IEnumerable<OjtPhoneme> phonemes)
        {
            var nextPhonemes = phonemes.ToList();
            if (phonemes.First().phoneme == "sil")
            {
                nextPhonemes[0].phoneme = spacePhoneme;
            }
            if (phonemes.Last().phoneme == "sil")
            {
                nextPhonemes[^1].phoneme = spacePhoneme;
            }

            return nextPhonemes;
        }
    }

    internal static class OjtPhonemeExtension
    {
        public static IEnumerable<OjtPhoneme> ToOjtPhonemeDataList(this IEnumerable<string> phonemeStrList)
        {
            var phonemeDataList = phonemeStrList.Select((p, i) => new OjtPhoneme(phoneme: p, start: i, end: i + 1));
            return OjtPhoneme.Convert(phonemeDataList);
        }
    }
}
