using System;
using Xunit;
using VoicevoxEngineSharp.Core.Language.Extensions;

namespace VoicevoxEngineSharp.Core.Test
{
    public class MoraExtensionTest
    {
        [Fact]
        public void ToTextTest()
        {
            // Test case
            // ref: https://github.com/soimort/python-romkan/blob/c82dc16f1154794fab765cceb2c036d20251717d/tests/test.py#L20-L74
            Assert.Equal("カンジ", MoraExtensions._ToTextFromJoinedMora("kanji"));
            Assert.Equal("カンジ", MoraExtensions._ToTextFromJoinedMora("kanzi"));
            Assert.Equal("カンジ", MoraExtensions._ToTextFromJoinedMora("kannji"));
            Assert.Equal("チエ", MoraExtensions._ToTextFromJoinedMora("chie"));
            Assert.Equal("チエ", MoraExtensions._ToTextFromJoinedMora("tie"));
            Assert.Equal("キョウジュ", MoraExtensions._ToTextFromJoinedMora("kyouju"));
            Assert.Equal("シュウキョウ", MoraExtensions._ToTextFromJoinedMora("syuukyou"));
            Assert.Equal("シュウキョウ", MoraExtensions._ToTextFromJoinedMora("shuukyou"));
            Assert.Equal("サイチュウ", MoraExtensions._ToTextFromJoinedMora("saichuu"));
            Assert.Equal("サイチュウ", MoraExtensions._ToTextFromJoinedMora("saityuu"));
            Assert.Equal("チェリー", MoraExtensions._ToTextFromJoinedMora("cheri-"));
            Assert.Equal("チェリー", MoraExtensions._ToTextFromJoinedMora("tyeri-"));
            Assert.Equal("シンライ", MoraExtensions._ToTextFromJoinedMora("shinrai"));
            Assert.Equal("シンライ", MoraExtensions._ToTextFromJoinedMora("sinrai"));
            Assert.Equal("ハンノウ", MoraExtensions._ToTextFromJoinedMora("hannnou"));
            Assert.Equal("ハンノウ", MoraExtensions._ToTextFromJoinedMora("han'nou"));

            Assert.Equal("ヲ", MoraExtensions._ToTextFromJoinedMora("wo"));
            Assert.Equal("ウェ", MoraExtensions._ToTextFromJoinedMora("we"));
            Assert.Equal("ドゥ", MoraExtensions._ToTextFromJoinedMora("du")); // LibRomkanは "ヅ"
            Assert.Equal("シェ", MoraExtensions._ToTextFromJoinedMora("she"));
            Assert.Equal("ディ", MoraExtensions._ToTextFromJoinedMora("di")); // LibRomkanは "ヂ"
            Assert.Equal("フ", MoraExtensions._ToTextFromJoinedMora("fu"));
            Assert.Equal("ティ", MoraExtensions._ToTextFromJoinedMora("ti")); // LibRomkanは "チ"
            Assert.Equal("ウィ", MoraExtensions._ToTextFromJoinedMora("wi"));

            Assert.Equal("ジェ", MoraExtensions._ToTextFromJoinedMora("je"));
            Assert.Equal("エージェント", MoraExtensions._ToTextFromJoinedMora("e-jento"));
        }
    }
}
