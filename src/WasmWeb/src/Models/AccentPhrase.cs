#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace VoicevoxEngineSharp.WasmWeb.Models
{
    internal record AccentPhrase
    {
        [JsonPropertyName("moras")]
        public IEnumerable<Mora> Moras { get; init; } = [];

        [JsonPropertyName("accent")]
        public int Accent { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("pause_mora")]
        public Mora? PauseMora { get; init; }

        [JsonPropertyName("is_interrogative")]
        public bool IsInterrogative { get; init; }

        public static AccentPhrase FromDomain(VoicevoxEngineSharp.Core.Acoustic.Models.AccentPhrase domainAccentPhrase)
            => new AccentPhrase
            {
                Accent = domainAccentPhrase.Accent,
                PauseMora = domainAccentPhrase.PauseMora == null ? null : Mora.FromDomain(domainAccentPhrase.PauseMora),
                Moras = domainAccentPhrase.Moras.Select(mora => Mora.FromDomain(mora)),
                IsInterrogative = domainAccentPhrase.IsInterrogative,
            };

        public static VoicevoxEngineSharp.Core.Acoustic.Models.AccentPhrase ToDomain(AccentPhrase accentPhrase)
            => new VoicevoxEngineSharp.Core.Acoustic.Models.AccentPhrase
            {
                Accent = accentPhrase.Accent,
                Moras = accentPhrase.Moras.Select(v => Mora.ToDomain(v)),
                PauseMora = accentPhrase.PauseMora == null ? null : Mora.ToDomain(accentPhrase.PauseMora),
                IsInterrogative = accentPhrase.IsInterrogative,
            };
    }
}
