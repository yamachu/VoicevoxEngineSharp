using System;
using VoicevoxEngineSharp.Core.Enum;
using Xunit;

namespace VoicevoxEngineSharp.Core.Test
{
    public class UtilsTest
    {
        [Fact]
        public void CreateSupportedDevicesJson()
        {
            var result = Utils.CreateSupportedDevicesJson(out var supportedDevicesJson);
            Assert.Equal(ResultCode.RESULT_OK, result);
            Assert.NotNull(supportedDevicesJson);
        }
    }
}
