using System;
using VoicevoxEngineSharp.Core.Enum;
using Xunit;

namespace VoicevoxEngineSharp.Core.Test
{
    public class ResultCodeTest
    {
        [Fact]
        public void ToMessage()
        {
            Assert.NotEmpty(ResultCode.ALREADY_LOADED_MODEL_ERROR.ToMessage());
        }
    }
}
