namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    internal class JvsPhoneme : BasePhoneme
    {
        internal static new IList<string> phonemeList = new List<string>{
            "pau",
            "I",
            "N",
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
            "gy",
            "h",
            "hy",
            "i",
            "j",
            "k",
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
            "u",
            "v",
            "w",
            "y",
            "z",
        };

        internal static int numPhoneme = phonemeList.Count;

        internal static string spacePhoneme = "pau";

        public JvsPhoneme(string phoneme, float start, float end) : base(phoneme, start, end)
        {
        }

        public static IEnumerable<JvsPhoneme> Convert(IEnumerable<JvsPhoneme> phonemes)
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
}
