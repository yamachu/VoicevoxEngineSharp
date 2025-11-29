using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VoicevoxEngineSharp.Core.Language.Models
{
    public class Phoneme
    {
        private Dictionary<string, string> _contexts { get; init; }

        public static Phoneme FromLabel(string label)
        {
            var labelRegex = new Regex(
                @"^(?<p1>.+?)\^(?<p2>.+?)\-(?<p3>.+?)\+(?<p4>.+?)\=(?<p5>.+?)"
                + @"/A\:(?<a1>.+?)\+(?<a2>.+?)\+(?<a3>.+?)"
                + @"/B\:(?<b1>.+?)\-(?<b2>.+?)_(?<b3>.+?)"
                + @"/C\:(?<c1>.+?)_(?<c2>.+?)\+(?<c3>.+?)"
                + @"/D\:(?<d1>.+?)\+(?<d2>.+?)_(?<d3>.+?)"
                + @"/E\:(?<e1>.+?)_(?<e2>.+?)\!(?<e3>.+?)_(?<e4>.+?)\-(?<e5>.+?)"
                + @"/F\:(?<f1>.+?)_(?<f2>.+?)\#(?<f3>.+?)_(?<f4>.+?)\@(?<f5>.+?)_(?<f6>.+?)\|(?<f7>.+?)_(?<f8>.+?)"
                + @"/G\:(?<g1>.+?)_(?<g2>.+?)\%(?<g3>.+?)_(?<g4>.+?)_(?<g5>.+?)"
                + @"/H\:(?<h1>.+?)_(?<h2>.+?)"
                + @"/I\:(?<i1>.+?)\-(?<i2>.+?)\@(?<i3>.+?)\+(?<i4>.+?)\&(?<i5>.+?)\-(?<i6>.+?)\|(?<i7>.+?)\+(?<i8>.+?)"
                + @"/J\:(?<j1>.+?)_(?<j2>.+?)"
                + @"/K\:(?<k1>.+?)\+(?<k2>.+?)\-(?<k3>.+?)$"
            );

            return new Phoneme(
                labelRegex.Match(label).Groups
                    .Cast<Group>()
                    .Select((g) => new KeyValuePair<string, string>(g.Name, g.Value))
                    .ToDictionary(g => g.Key, g => g.Value));
        }

        private Phoneme(Dictionary<string, string> context)
        {
            _contexts = context;
        }

        public string Context(string key) => _contexts[key];

        public string Label
        {
            get
            {
                return $"{_contexts["p1"]}^{_contexts["p2"]}-{_contexts["p3"]}+{_contexts["p4"]}={_contexts["p5"]}"
                        + $"/A:{_contexts["a1"]}+{_contexts["a2"]}+{_contexts["a3"]}"
                        + $"/B:{_contexts["b1"]}-{_contexts["b2"]}_{_contexts["b3"]}"
                        + $"/C:{_contexts["c1"]}_{_contexts["c2"]}+{_contexts["c3"]}"
                        + $"/D:{_contexts["d1"]}+{_contexts["d2"]}_{_contexts["d3"]}"
                        + $"/E:{_contexts["e1"]}_{_contexts["e2"]}!{_contexts["e3"]}_{_contexts["e4"]}-{_contexts["e5"]}"
                        + $"/F:{_contexts["f1"]}_{_contexts["f2"]}#{_contexts["f3"]}_{_contexts["f4"]}@{_contexts["f5"]}_{_contexts["f6"]}|{_contexts["f7"]}_{_contexts["f8"]}"
                        + $"/G:{_contexts["g1"]}_{_contexts["g2"]}%{_contexts["g3"]}_{_contexts["g4"]}_{_contexts["g5"]}"
                        + $"/H:{_contexts["h1"]}_{_contexts["h2"]}"
                        + $"/I:{_contexts["i1"]}-{_contexts["i2"]}@{_contexts["i3"]}+{_contexts["i4"]}&{_contexts["i5"]}-{_contexts["i6"]}|{_contexts["i7"]}+{_contexts["i8"]}"
                        + $"/J:{_contexts["j1"]}_{_contexts["j2"]}"
                        + $"/K:{_contexts["k1"]}+{_contexts["k2"]}-{_contexts["k3"]}";
            }
        }

        public bool IsPose => _contexts["f1"] == "xx";
        public bool IsInterrogative => _contexts["f3"] == "1";

        public string GetPhoneme() => _contexts["p3"];

        public void SetContext(string key, string value)
        {
            _contexts[key] = value;
        }
    }
}
