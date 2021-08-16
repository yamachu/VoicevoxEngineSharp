using System.Collections.Generic;

internal record AccentPhrase
{
    public IEnumerable<Mora> Moras { get; init; }

    public int Accent { get; init; }

#nullable enable
    public Mora? PauseMora { get; init; }
#nullable disable
}
