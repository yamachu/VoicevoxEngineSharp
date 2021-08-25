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

        public virtual int PhonemeId => phonemeList.FindIndex((s) => s == phoneme);
    }
}
