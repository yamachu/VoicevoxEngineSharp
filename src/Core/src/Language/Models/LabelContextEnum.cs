namespace VoicevoxEngineSharp.Core.Language.Models
{
    public enum LabelContextEnum
    {
        /// <summary>a2</summary>
        MoraPlaceInAccent,
        /// <summary>f2</summary>
        CurrentAccent,
        /// <summary>f1</summary>
        CurrentMoraCount,
        /// <summary>p3</summary>
        CurrentPhoneme,
        /// <summary>i3</summary>
        BreathGroupIndex,
        /// <summary>f5</summary>
        MoraPositionOfPhraseInBreathGroup,
        /// <summary>g2</summary>
        NextAccent,
        /// <summary>g1</summary>
        NextMoraCount,
        /// <summary>e2</summary>
        PrevAccent,
        /// <summary>e1</summary>
        PrevMoraCount,
        /// <summary>a1</summary>
        DistanceFromAccent,
        /// <summary>a3</summary>
        MoraPositionOfEndOfPhrase,
        // TODO: 名付け………
        /// <summary>j1</summary>
        /// <summary>h1</summary>
        /// <summary>i1</summary>
        /// <summary>i5</summary>
        /// <summary>i6</summary>
        /// <summary>k2</summary>
    }

    internal static class LabelContextEnumExtension
    {
        internal static string ToContextField(this LabelContextEnum contextEnum)
        {
            string[] strs = { "a2", "f2", "f1", "p3", "i3", "f5", "g2", "g1", "e2", "e1", "a", "a3" };
            return strs[(int)contextEnum];
        }
    }
}
