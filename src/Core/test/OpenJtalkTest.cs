using System;
using VoicevoxEngineSharp.Core.Enum;
using VoicevoxEngineSharp.Core.Struct;
using Xunit;

namespace VoicevoxEngineSharp.Core.Test
{
    public class OpenJtalkTest
    {
        [Fact]
        public void Open()
        {
            var openResult = OpenJtalk.New(Consts.OpenJTalkDictDir, out var openJtalk);

            Assert.Equal(ResultCode.RESULT_OK, openResult);
            Assert.NotNull(openJtalk);
        }

        [Fact]
        public void UseUserDict()
        {
            OpenJtalk.New(Consts.OpenJTalkDictDir, out var openJtalk);
            // 空っぽの辞書をコンパイルしようとするとクラッシュするので足す
            var userDict = new UserDict();
            userDict.AddWord(UserDictWord.Create("hoge", "ホゲ"), out var _);

            var result = openJtalk.UseUserDict(userDict);

            Assert.Equal(ResultCode.RESULT_OK, result);
        }
    }
}
