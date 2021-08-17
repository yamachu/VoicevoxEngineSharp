using System.Text.RegularExpressions;

namespace VoicevoxEngineSharp.Core.Language.Domains.Romkan
{
    // https://github.com/soimort/python-romkan
    // Re-Implemented
    internal class LibRomkan
    {
        private static LibRomkan? _instance;

        private IDictionary<string, string> ROMKAN { get; set; }
        private Regex? ROMPAT { get; set; }

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
            var groupedKanaRomasDict = Regex.Split(KUNREITAB + HEPBURNTAB, @"\s+")
                .Where(v => v.Trim() != "")
                .Select((s, i) => (s, i))
                .GroupBy(x => x.i / 2)
                .Select(g => g.Select(v => v.s))
                .GroupBy(g => g.Skip(1).First(), g => g.First())
                .ToDictionary(v => v.Key, v => v.First());

            groupedKanaRomasDict["du"] = "ヅ";
            groupedKanaRomasDict["di"] = "ヂ";
            groupedKanaRomasDict["fu"] = "フ";
            groupedKanaRomasDict["ti"] = "チ";
            groupedKanaRomasDict["wi"] = "ウィ";
            groupedKanaRomasDict["we"] = "ウェ";
            groupedKanaRomasDict["wo"] = "ヲ";

            ROMKAN = groupedKanaRomasDict;
            ROMPAT = new Regex(string.Join("|", groupedKanaRomasDict.Keys.OrderBy(v => -v.Length)));
        }

        private static string NormalizeDoubleN(string str)
        {
            var removeDoubleN = Regex.Replace(str, "nn", "n'");
            return Regex.Replace(removeDoubleN, @"n'(?=[^aiueoyn]|$)", "n");
        }

        public string ToKatakana(string romaji)
        {
            var normalized = NormalizeDoubleN(romaji.ToLower());
            if (ROMPAT == null)
            {
                throw new Exception();
            }
            return ROMPAT.Replace(normalized, (matched) => ROMKAN[matched.Groups[0].Value]);
        }

        ///
        /// Ruby/Romkan - a Romaji <-> Kana conversion library for Ruby.
        ///
        /// Copyright (C) 2001 Satoru Takabayashi <satoru@namazu.org>
        ///     All rights reserved.
        ///     This is free software with ABSOLUTELY NO WARRANTY.
        ///
        /// You can redistribute it and/or modify it under the terms of 
        /// the Ruby's licence.
        ///

        /// This table is imported from KAKASI <http://kakasi.namazu.org/> and modified.

        readonly string KUNREITAB = @"ァ       xa      ア       a       ィ       xi      イ       i       ゥ       xu
ウ       u       ヴ       vu      ヴァ      va      ヴィ      vi      ヴェ      ve
ヴォ      vo      ェ       xe      エ       e       ォ       xo      オ       o 

カ       ka      ガ       ga      キ       ki      キャ      kya     キュ      kyu 
キョ      kyo     ギ       gi      ギャ      gya     ギュ      gyu     ギョ      gyo 
ク       ku      グ       gu      ケ       ke      ゲ       ge      コ       ko
ゴ       go 

サ       sa      ザ       za      シ       si      シャ      sya     シュ      syu 
ショ      syo     シェ    sye
ジ       zi      ジャ      zya     ジュ      zyu     ジョ      zyo 
ス       su      ズ       zu      セ       se      ゼ       ze      ソ       so
ゾ       zo 

タ       ta      ダ       da      チ       ti      チャ      tya     チュ      tyu 
チョ      tyo     ヂ       di      ヂャ      dya     ヂュ      dyu     ヂョ      dyo 
ティ    ti

ッ       xtu 
ッヴ      vvu     ッヴァ     vva     ッヴィ     vvi 
ッヴェ     vve     ッヴォ     vvo 
ッカ      kka     ッガ      gga     ッキ      kki     ッキャ     kkya 
ッキュ     kkyu    ッキョ     kkyo    ッギ      ggi     ッギャ     ggya 
ッギュ     ggyu    ッギョ     ggyo    ック      kku     ッグ      ggu 
ッケ      kke     ッゲ      gge     ッコ      kko     ッゴ      ggo     ッサ      ssa 
ッザ      zza     ッシ      ssi     ッシャ     ssya 
ッシュ     ssyu    ッショ     ssyo    ッシェ     ssye
ッジ      zzi     ッジャ     zzya    ッジュ     zzyu    ッジョ     zzyo
ッス      ssu     ッズ      zzu     ッセ      sse     ッゼ      zze     ッソ      sso 
ッゾ      zzo     ッタ      tta     ッダ      dda     ッチ      tti     ッティ  tti
ッチャ     ttya    ッチュ     ttyu    ッチョ     ttyo    ッヂ      ddi 
ッヂャ     ddya    ッヂュ     ddyu    ッヂョ     ddyo    ッツ      ttu 
ッヅ      ddu     ッテ      tte     ッデ      dde     ット      tto     ッド      ddo 
ッドゥ  ddu
ッハ      hha     ッバ      bba     ッパ      ppa     ッヒ      hhi 
ッヒャ     hhya    ッヒュ     hhyu    ッヒョ     hhyo    ッビ      bbi 
ッビャ     bbya    ッビュ     bbyu    ッビョ     bbyo    ッピ      ppi 
ッピャ     ppya    ッピュ     ppyu    ッピョ     ppyo    ッフ      hhu     ッフュ  ffu
ッファ     ffa     ッフィ     ffi     ッフェ     ffe     ッフォ     ffo 
ッブ      bbu     ップ      ppu     ッヘ      hhe     ッベ      bbe     ッペ    ppe
ッホ      hho     ッボ      bbo     ッポ      ppo     ッヤ      yya     ッユ      yyu 
ッヨ      yyo     ッラ      rra     ッリ      rri     ッリャ     rrya 
ッリュ     rryu    ッリョ     rryo    ッル      rru     ッレ      rre 
ッロ      rro 

ツ       tu      ヅ       du      テ       te      デ       de      ト       to
ド       do      ドゥ    du

ナ       na      ニ       ni      ニャ      nya     ニュ      nyu     ニョ      nyo 
ヌ       nu      ネ       ne      ノ       no 

ハ       ha      バ       ba      パ       pa      ヒ       hi      ヒャ      hya 
ヒュ      hyu     ヒョ      hyo     ビ       bi      ビャ      bya     ビュ      byu 
ビョ      byo     ピ       pi      ピャ      pya     ピュ      pyu     ピョ      pyo 
フ       hu      ファ      fa      フィ      fi      フェ      fe      フォ      fo
フュ    fu
ブ       bu      プ       pu      ヘ       he      ベ       be      ペ       pe
ホ       ho      ボ       bo      ポ       po 

マ       ma      ミ       mi      ミャ      mya     ミュ      myu     ミョ      myo 
ム       mu      メ       me      モ       mo 

ャ       xya     ヤ       ya      ュ       xyu     ユ       yu      ョ       xyo
ヨ       yo

ラ       ra      リ       ri      リャ      rya     リュ      ryu     リョ      ryo 
ル       ru      レ       re      ロ       ro 

ヮ       xwa     ワ       wa      ウィ    wi      ヰ wi      ヱ       we      ウェ      we
ヲ       wo      ウォ    wo      ン n 

ン     n'
ディ   dyi
ー     -
チェ    tye
ッチェ     ttye
ジェ      zye
";

        readonly string HEPBURNTAB = @"ァ      xa      ア       a       ィ       xi      イ       i       ゥ       xu
ウ       u       ヴ       vu      ヴァ      va      ヴィ      vi      ヴェ      ve
ヴォ      vo      ェ       xe      エ       e       ォ       xo      オ       o
        

カ       ka      ガ       ga      キ       ki      キャ      kya     キュ      kyu
キョ      kyo     ギ       gi      ギャ      gya     ギュ      gyu     ギョ      gyo
ク       ku      グ       gu      ケ       ke      ゲ       ge      コ       ko
ゴ       go      

サ       sa      ザ       za      シ       shi     シャ      sha     シュ      shu
ショ      sho     シェ    she
ジ       ji      ジャ      ja      ジュ      ju      ジョ      jo
ス       su      ズ       zu      セ       se      ゼ       ze      ソ       so
ゾ       zo

タ       ta      ダ       da      チ       chi     チャ      cha     チュ      chu
チョ      cho     ヂ       di      ヂャ      dya     ヂュ      dyu     ヂョ      dyo
ティ    ti

ッ       xtsu    
ッヴ      vvu     ッヴァ     vva     ッヴィ     vvi     
ッヴェ     vve     ッヴォ     vvo     
ッカ      kka     ッガ      gga     ッキ      kki     ッキャ     kkya    
ッキュ     kkyu    ッキョ     kkyo    ッギ      ggi     ッギャ     ggya    
ッギュ     ggyu    ッギョ     ggyo    ック      kku     ッグ      ggu     
ッケ      kke     ッゲ      gge     ッコ      kko     ッゴ      ggo     ッサ      ssa
ッザ      zza     ッシ      sshi    ッシャ     ssha    
ッシュ     sshu    ッショ     ssho    ッシェ  sshe
ッジ      jji     ッジャ     jja     ッジュ     jju     ッジョ     jjo     
ッス      ssu     ッズ      zzu     ッセ      sse     ッゼ      zze     ッソ      sso
ッゾ      zzo     ッタ      tta     ッダ      dda     ッチ      cchi    ッティ  tti
ッチャ     ccha    ッチュ     cchu    ッチョ     ccho    ッヂ      ddi     
ッヂャ     ddya    ッヂュ     ddyu    ッヂョ     ddyo    ッツ      ttsu    
ッヅ      ddu     ッテ      tte     ッデ      dde     ット      tto     ッド      ddo
ッドゥ  ddu
ッハ      hha     ッバ      bba     ッパ      ppa     ッヒ      hhi     
ッヒャ     hhya    ッヒュ     hhyu    ッヒョ     hhyo    ッビ      bbi     
ッビャ     bbya    ッビュ     bbyu    ッビョ     bbyo    ッピ      ppi     
ッピャ     ppya    ッピュ     ppyu    ッピョ     ppyo    ッフ      ffu     ッフュ  ffu
ッファ     ffa     ッフィ     ffi     ッフェ     ffe     ッフォ     ffo     
ッブ      bbu     ップ      ppu     ッヘ      hhe     ッベ      bbe     ッペ      ppe
ッホ      hho     ッボ      bbo     ッポ      ppo     ッヤ      yya     ッユ      yyu
ッヨ      yyo     ッラ      rra     ッリ      rri     ッリャ     rrya    
ッリュ     rryu    ッリョ     rryo    ッル      rru     ッレ      rre     
ッロ      rro     

ツ       tsu     ヅ       du      テ       te      デ       de      ト       to
ド       do      ドゥ    du

ナ       na      ニ       ni      ニャ      nya     ニュ      nyu     ニョ      nyo
ヌ       nu      ネ       ne      ノ       no      

ハ       ha      バ       ba      パ       pa      ヒ       hi      ヒャ      hya
ヒュ      hyu     ヒョ      hyo     ビ       bi      ビャ      bya     ビュ      byu
ビョ      byo     ピ       pi      ピャ      pya     ピュ      pyu     ピョ      pyo
フ       fu      ファ      fa      フィ      fi      フェ      fe      フォ      fo
フュ    fu
ブ       bu      プ       pu      ヘ       he      ベ       be      ペ       pe
ホ       ho      ボ       bo      ポ       po      

マ       ma      ミ       mi      ミャ      mya     ミュ      myu     ミョ      myo
ム       mu      メ       me      モ       mo

ャ       xya     ヤ       ya      ュ       xyu     ユ       yu      ョ       xyo
ヨ       yo      

ラ       ra      リ       ri      リャ      rya     リュ      ryu     リョ      ryo
ル       ru      レ       re      ロ       ro      

ヮ       xwa     ワ       wa      ウィ    wi      ヰ wi      ヱ       we      ウェ    we
ヲ       wo      ウォ    wo      ン n       

ン     n'
ディ   di
ー     -
チェ    che
ッチェ     cche
ジェ      je
";

    }
}
