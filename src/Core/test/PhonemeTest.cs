using System;
using VoicevoxEngineSharp.Core.Language.Models;
using Xunit;

namespace VoicevoxEngineSharp.Core.Test
{
    public class PhonemeTest
    {
        [Fact]
        public void PhonemeCanParse()
        {
            var passLabel = "xx^xx-sil+ch=i/A:xx+xx+xx/B:xx-xx_xx/C:xx_xx+xx/D:07+xx_xx/E:xx_xx!xx_xx-xx/F:xx_xx#xx_xx@xx_xx|xx_xx/G:4_1%0_xx_xx/H:xx_xx/I:xx-xx@xx+xx&xx-xx|xx+xx/J:2_9/K:2+6-23";
            var phoneme = Phoneme.FromLabel(passLabel);
            Assert.Equal(passLabel, phoneme.Label);
        }
    }
}
