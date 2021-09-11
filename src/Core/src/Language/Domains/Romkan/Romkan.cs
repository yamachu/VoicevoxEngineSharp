using System;
using System.Collections.Generic;
using System.Linq;

namespace VoicevoxEngineSharp.Core.Language.Domains.Romkan
{
    internal class LibRomkan
    {
        private static LibRomkan? _instance;

        private IDictionary<string, string> ROMKAN { get; set; }

        public static LibRomkan Instance()
        {
            if (_instance == null)
            {
                _instance = new LibRomkan();
            }

            return _instance;
        }

        private LibRomkan()
        {
            ROMKAN = new Dictionary<string, string>();
            Initialize();
        }

        private void Initialize()
        {
            ROMKAN = OpenJTalkMora2TextArray
                .Cast<string>()
                .Select((s, i) => (i, s))
                .GroupBy((v) => v.i / 3)
                .Select(v => (mora: v.ElementAt(1).s + v.ElementAt(2).s, text: v.ElementAt(0).s))
                .ToDictionary(v => v.mora, v => v.text);
        }

        readonly char[] unvoicedChar = new char[] { 'A', 'I', 'U', 'E', 'O' };

        public string ToKatakana(string romaji)
        {
            var unvoicedToVoicedMora = new string(romaji.Reverse().Select((s, i) =>
            {
                if (i == 0 && unvoicedChar.Contains(s))
                {
                    return s.ToString().ToLower()[0];
                }
                return s;
            }).Reverse().ToArray());

            if (ROMKAN.TryGetValue(unvoicedToVoicedMora, out var result))
            {
                return result;
            }
            return romaji;
        }

        // ref: https://github.com/Hiroshiba/voicevox_engine/pull/63
        // ref: https://github.com/Hiroshiba/voicevox_engine/pull/70
        readonly string[,] OpenJTalkMora2TextArray = new string[,]{
            // {"ヴョ", "by", "o"},
            // {"ヴュ", "by", "u"},
            // {"ヴャ", "by", "a"},
            {"ヴォ", "v", "o"},
            {"ヴェ", "v", "e"},
            {"ヴィ", "v", "i"},
            {"ヴァ", "v", "a"},
            {"ヴ", "v", "u"},
            {"ン", "", "N"},
            // {"ヲ", "", "o"},
            // {"ヱ", "", "e"},
            // {"ヰ", "", "i"},
            {"ワ", "w", "a"},
            // {"ヮ", "w", "a"},
            {"ロ", "r", "o"},
            {"レ", "r", "e"},
            {"ル", "r", "u"},
            {"リョ", "ry", "o"},
            {"リュ", "ry", "u"},
            {"リャ", "ry", "a"},
            {"リェ", "ry", "e"},
            {"リ", "r", "i"},
            {"ラ", "r", "a"},
            {"ヨ", "y", "o"},
            // {"ョ", "y", "o"},
            {"ユ", "y", "u"},
            // {"ュ", "y", "u"},
            {"ヤ", "y", "a"},
            // {"ャ", "y", "a"},
            {"モ", "m", "o"},
            {"メ", "m", "e"},
            {"ム", "m", "u"},
            {"ミョ", "my", "o"},
            {"ミュ", "my", "u"},
            {"ミャ", "my", "a"},
            {"ミェ", "my", "e"},
            {"ミ", "m", "i"},
            {"マ", "m", "a"},
            {"ポ", "p", "o"},
            {"ボ", "b", "o"},
            {"ホ", "h", "o"},
            {"ペ", "p", "e"},
            {"ベ", "b", "e"},
            {"ヘ", "h", "e"},
            {"プ", "p", "u"},
            {"ブ", "b", "u"},
            {"フォ", "f", "o"},
            {"フェ", "f", "e"},
            {"フィ", "f", "i"},
            {"ファ", "f", "a"},
            {"フ", "f", "u"},
            {"ピョ", "py", "o"},
            {"ピュ", "py", "u"},
            {"ピャ", "py", "a"},
            {"ピェ", "py", "e"},
            {"ピ", "p", "i"},
            {"ビョ", "by", "o"},
            {"ビュ", "by", "u"},
            {"ビャ", "by", "a"},
            {"ビェ", "by", "e"},
            {"ビ", "b", "i"},
            {"ヒョ", "hy", "o"},
            {"ヒュ", "hy", "u"},
            {"ヒャ", "hy", "a"},
            {"ヒェ", "hy", "e"},
            {"ヒ", "h", "i"},
            {"パ", "p", "a"},
            {"バ", "b", "a"},
            {"ハ", "h", "a"},
            {"ノ", "n", "o"},
            {"ネ", "n", "e"},
            {"ヌ", "n", "u"},
            {"ニョ", "ny", "o"},
            {"ニュ", "ny", "u"},
            {"ニャ", "ny", "a"},
            {"ニェ", "ny", "e"},
            {"ニ", "n", "i"},
            {"ナ", "n", "a"},
            {"ドゥ", "d", "u"},
            {"ド", "d", "o"},
            {"トゥ", "t", "u"},
            {"ト", "t", "o"},
            {"デョ", "dy", "o"},
            {"デュ", "dy", "u"},
            {"デャ", "dy", "a"},
            {"デェ", "dy", "e"},
            {"ディ", "d", "i"},
            {"デ", "d", "e"},
            {"テョ", "ty", "o"},
            {"テュ", "ty", "u"},
            {"テャ", "ty", "a"},
            {"ティ", "t", "i"},
            {"テ", "t", "e"},
            // {"ヅ", "z", "u"},
            {"ツォ", "ts", "o"},
            {"ツェ", "ts", "e"},
            {"ツィ", "ts", "i"},
            {"ツァ", "ts", "a"},
            {"ツ", "ts", "u"},
            {"ッ", "", "cl"},
            // {"ヂ", "j", "i"},
            {"チョ", "ch", "o"},
            {"チュ", "ch", "u"},
            {"チャ", "ch", "a"},
            {"チェ", "ch", "e"},
            {"チ", "ch", "i"},
            {"ダ", "d", "a"},
            {"タ", "t", "a"},
            {"ゾ", "z", "o"},
            {"ソ", "s", "o"},
            {"ゼ", "z", "e"},
            {"セ", "s", "e"},
            {"ズィ", "z", "i"},
            {"ズ", "z", "u"},
            {"スィ", "s", "i"},
            {"ス", "s", "u"},
            {"ジョ", "j", "o"},
            {"ジュ", "j", "u"},
            {"ジャ", "j", "a"},
            {"ジェ", "j", "e"},
            {"ジ", "j", "i"},
            {"ショ", "sh", "o"},
            {"シュ", "sh", "u"},
            {"シャ", "sh", "a"},
            {"シェ", "sh", "e"},
            {"シ", "sh", "i"},
            {"ザ", "z", "a"},
            {"サ", "s", "a"},
            {"ゴ", "g", "o"},
            {"コ", "k", "o"},
            {"ゲ", "g", "e"},
            {"ケ", "k", "e"},
            // {"ヶ", "k", "e"},
            {"グヮ", "gw", "a"},
            {"グ", "g", "u"},
            {"クヮ", "kw", "a"},
            {"ク", "k", "u"},
            {"ギョ", "gy", "o"},
            {"ギュ", "gy", "u"},
            {"ギャ", "gy", "a"},
            {"ギェ", "gy", "e"},
            {"ギ", "g", "i"},
            {"キョ", "ky", "o"},
            {"キュ", "ky", "u"},
            {"キャ", "ky", "a"},
            {"キェ", "ky", "e"},
            {"キ", "k", "i"},
            {"ガ", "g", "a"},
            {"カ", "k", "a"},
            {"オ", "", "o"},
            // {"ォ", "", "o"},
            {"エ", "", "e"},
            // {"ェ", "", "e"},
            {"ウォ", "w", "o"},
            {"ウェ", "w", "e"},
            {"ウィ", "w", "i"},
            {"ウ", "", "u"},
            // {"ゥ", "", "u"},
            {"イェ", "y", "e"},
            {"イ", "", "i"},
            // {"ィ", "", "i"},
            {"ア", "", "a"},
            // {"ァ", "", "a"},
        };
    }
}
