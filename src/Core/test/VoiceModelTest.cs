using System;
using System.IO;
using VoicevoxEngineSharp.Core.Enum;
using Xunit;

namespace VoicevoxEngineSharp.Core.Test
{
    public class VoiceModelTest
    {
        [Fact]
        public void Open()
        {
            var openResult = VoiceModel.New("/Users/yamachu/Projects/github.com/yamachu/VoicevoxEngineSharp/src/Core/test/bin/Debug/net6.0/runtimes/osx/native/model/0.vvm", out var voiceModel);
            Assert.Equal(ResultCode.RESULT_OK, openResult);
            Assert.NotNull(voiceModel);

            Assert.NotEmpty(voiceModel.Id);
            Assert.NotEmpty(voiceModel.MetasJson);
        }
    }
}
