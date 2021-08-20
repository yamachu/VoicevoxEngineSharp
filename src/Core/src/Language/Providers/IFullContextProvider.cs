using System.Collections.Generic;

namespace VoicevoxEngineSharp.Core.Language.Providers
{
    public interface IFullContextProvider
    {
        IEnumerable<string> ToFullContextLabels(string text);
    }
}
