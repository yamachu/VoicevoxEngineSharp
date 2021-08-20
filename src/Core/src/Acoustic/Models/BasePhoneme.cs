using System;
using System.Collections.Generic;
using System.Linq;
using VoicevoxEngineSharp.Core.Language.Extensions;

namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    internal abstract class BasePhoneme : IBasePhoneme
    {
        internal static IList<string> phonemeList = new List<string>();
        internal string phoneme { get; set; }
        internal float start { get; init; }
        internal float end { get; init; }

        public BasePhoneme(string phoneme, float start, float end)
        {
            this.phoneme = phoneme;
            this.start = MathF.Round(start, 2);
            this.end = MathF.Round(end, 2);
        }

        public override bool Equals(object obj)
        {
            return obj is BasePhoneme && GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return new { phoneme, start, end }.GetHashCode();
        }

        public int PhonemeId => phonemeList.FindIndex((s) => s == phoneme);
    }

    internal static class BasePhonemeExtension
    {
        public static (IEnumerable<BasePhoneme?> consonantPhonemeList, BasePhoneme[] vowelPhonemeList, int[] vowelIndexes) SplitMora(this IEnumerable<BasePhoneme> phonemeList)
        {
            var vowelIndexes = phonemeList.Select((v, i) => (v, i))
                .Where(v => !PhonemeList.MoraPhonemes.Any(p => p == v.v.phoneme))
                .Select(v => v.i)
                .ToArray();

            var vowelPhonemeList = vowelIndexes.Select(i => phonemeList.ElementAt(i)).ToArray();
            var consonantPhonemeList = Enumerable.Concat(
                new BasePhoneme?[] { null },
                Enumerable.Zip(vowelIndexes[..^1], vowelIndexes[1..])
                    .Select((pair) => pair.Second - pair.First == 1 ? null : phonemeList.ElementAtOrDefault(pair.Second - 1))
                );

            return (consonantPhonemeList, vowelPhonemeList, vowelIndexes);
        }
    }
}
