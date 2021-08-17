using VoicevoxEngineSharp.Core.Language.Extensions;

namespace VoicevoxEngineSharp.Core.Language.Models
{
    public class Utterance
    {
        private IEnumerable<BreathGroup> _breathGroups { get; init; }
        private IEnumerable<Phoneme> _pauses { get; init; }

        public static Utterance FromPhonemes(IEnumerable<Phoneme> phonemes)
        {
            var pauses = new List<Phoneme>();
            var breathGroups = new List<BreathGroup>();
            var groupPhonemes = new List<Phoneme>();

            foreach (var phoneme in phonemes)
            {
                if (!phoneme.IsPose)
                {
                    groupPhonemes.Add(phoneme);
                    continue;
                }

                pauses.Add(phoneme);
                if (groupPhonemes.Count > 0)
                {
                    breathGroups.Add(BreathGroup.FromPhonemes(groupPhonemes));
                    groupPhonemes.Clear();
                }
            }

            return new Utterance(breathGroups, pauses);
        }

        private Utterance(IEnumerable<BreathGroup> breathGroups, IEnumerable<Phoneme> pauses)
        {
            _breathGroups = breathGroups;
            _pauses = pauses;
        }

        public void SetContext(string key, string value)
        {
            foreach (var breathGroup in _breathGroups)
            {
                breathGroup.SetContext(key, value);
            }
        }

        private IEnumerable<Phoneme> ToPhonemes()
        {
            var accentPhrases = _breathGroups.SelectMany(bg => bg.AccentPhrases).ToArray();
            foreach (var (prev, cent, post)
                in Enumerable.Zip(
                    Enumerable.Concat(new AccentPhrase?[] { null }, accentPhrases[..^1]),
                    accentPhrases,
                    Enumerable.Concat(accentPhrases[1..], new AccentPhrase?[] { null })))
            {
                var moraNum = cent.Moras.Count();
                var accent = cent.Accent;

                if (prev != null)
                {
                    prev.SetContext("g1", moraNum.ToString());
                    prev.SetContext("g2", accent.ToString());
                }

                if (post != null)
                {
                    post.SetContext("e1", moraNum.ToString());
                    post.SetContext("e2", accent.ToString());
                }

                cent.SetContext("f1", moraNum.ToString());
                cent.SetContext("f2", accent.ToString());

                foreach (var (mora, iMora) in cent.Moras.Select((m, i) => (m, i)))
                {
                    mora.SetContext("a1", (iMora - accent + 1).ToString());
                    mora.SetContext("a2", (iMora + 1).ToString());
                    mora.SetContext("a3", (moraNum - iMora).ToString());
                }
            }

            foreach (var (prev, cent, post)
                in Enumerable.Zip(
                    Enumerable.Concat(new BreathGroup?[] { null }, _breathGroups.Take(_breathGroups.Count() - 1)),
                    _breathGroups,
                    Enumerable.Concat(_breathGroups.Skip(1), new BreathGroup?[] { null })))
            {
                var accentPhraseNum = cent.AccentPhrases.Count();

                if (prev != null)
                {
                    prev.SetContext("j1", accentPhraseNum.ToString());
                }

                if (post != null)
                {
                    post.SetContext("h1", accentPhraseNum.ToString());
                }

                cent.SetContext("i1", accentPhraseNum.ToString());
                // Todo: 参照一緒だったよね…確認しよう
                cent.SetContext("i5",
                    (accentPhrases.FindIndex((elm) => elm == cent.AccentPhrases.ElementAt(0)) + 1).ToString());
                cent.SetContext("i6",
                    (accentPhrases.Length -
                    accentPhrases.FindIndex((elm) => elm == cent.AccentPhrases.ElementAt(0))).ToString()
                );
            }

            SetContext("k2", _breathGroups.Select(bg => bg.AccentPhrases.Count()).Sum().ToString());

            var phonemes = new List<Phoneme>();

            foreach (var (pause, index) in _pauses.Select((p, i) => (p, i)))
            {
                if (pause != null)
                {
                    phonemes.Add(pause);
                }

                if (index < _pauses.Count() - 1)
                {
                    phonemes.AddRange(_breathGroups.ElementAt(index).Phonemes);
                }
            }

            return phonemes;
        }

        public IEnumerable<Phoneme> Phonemes => ToPhonemes();
        public IEnumerable<string> Labels => Phonemes.Select(p => p.Label);
    }
}
