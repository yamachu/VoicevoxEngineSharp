using System.Linq;
using VoicevoxEngineSharp.Core.Language.Models;
using VoicevoxEngineSharp.Core.Language.Providers;

namespace VoicevoxEngineSharp.Core.Language.Usecases
{
    public class TextToUtterance
    {
        private IFullContextProvider _provider { get; init; }
        public TextToUtterance(IFullContextProvider provider)
        {
            _provider = provider;
        }

        internal Utterance ToUtterance(string text)
        {
            var labels = _provider.ToFullContextLabels(text);
            var phonemes = labels.Select(l => Phoneme.FromLabel(l));
            return Utterance.FromPhonemes(phonemes);
        }
    }
}
