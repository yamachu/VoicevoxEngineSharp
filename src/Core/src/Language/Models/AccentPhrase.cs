namespace VoicevoxEngineSharp.Core.Language.Models
{
    public class AccentPhrase
    {
        private IEnumerable<Mora> _moras { get; init; }
        private int _accent { get; init; }

        public static AccentPhrase FromPhonemes(IEnumerable<Phoneme> phonemes)
        {
            var moras = new List<Mora>();
            var moraPhonemes = new List<Phoneme>();

            foreach (var (phoneme, nextPhoneme) in Enumerable.Zip(phonemes, Enumerable.Concat(phonemes.Skip(1), new Phoneme?[] { null })))
            {
                moraPhonemes.Add(phoneme);
                if (nextPhoneme == null || phoneme.Context("a2") != nextPhoneme.Context("a2"))
                {
                    var (consonant, vowel) = moraPhonemes.Count switch
                    {
                        1 => (null, moraPhonemes[0]),
                        2 => (moraPhonemes[0], moraPhonemes[1]),
                        _ => throw new ArgumentException("TODO"),
                    };

                    moras.Add(new Mora(vowel: vowel, consonant: consonant));
                    moraPhonemes.Clear();
                }
            }

            return new AccentPhrase(moras, Convert.ToInt32(moras[0].Vowel.Context("f2")));
        }

        private AccentPhrase(IEnumerable<Mora> moras, int accent)
        {
            _moras = moras;
            _accent = accent;
        }

        public void SetContext(string key, string value)
        {
            foreach (var mora in _moras)
            {
                mora.SetContext(key, value);
            }
        }

        public IEnumerable<Phoneme> Phonemes => _moras.SelectMany(v => v.Phonemes);

        public IEnumerable<string> Labels => Phonemes.Select(p => p.Label);

        public IEnumerable<Mora> Moras => _moras;

        public int Accent => _accent;

        public AccentPhrase Merge(AccentPhrase accentPhrase)
        {
            return new AccentPhrase(Enumerable.Concat(_moras, accentPhrase._moras), _accent);
        }
    }
}
