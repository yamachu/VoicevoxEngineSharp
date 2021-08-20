using System.Collections.Generic;
using System.Linq;

namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    public record AccentPhrase
    {
        public IEnumerable<Mora> Moras { get; init; }

        public int Accent { get; init; }

#nullable enable
        public Mora? PauseMora { get; init; }
#nullable disable
    }

    internal static class AccentPhraseExtension
    {
        public static IEnumerable<Mora> ToFlattenMoras(this IEnumerable<AccentPhrase> accentPhrases)
        {
            return accentPhrases
                .SelectMany(accentPhrase => accentPhrase.PauseMora == null
                ? accentPhrase.Moras
                : Enumerable.Concat(accentPhrase.Moras, new[] { accentPhrase.PauseMora }));
        }
    }

}
