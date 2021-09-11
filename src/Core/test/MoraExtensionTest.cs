using System;
using VoicevoxEngineSharp.Core.Language.Extensions;
using Xunit;

namespace VoicevoxEngineSharp.Core.Test
{
    public class MoraExtensionTest
    {
        [Fact]
        public void ToTextTest()
        {
            Assert.Equal("ッ", MoraExtensions._ToTextFromJoinedMora("cl"));
            Assert.Equal("ティ", MoraExtensions._ToTextFromJoinedMora("ti"));
            Assert.Equal("トゥ", MoraExtensions._ToTextFromJoinedMora("tu"));
            Assert.Equal("ディ", MoraExtensions._ToTextFromJoinedMora("di"));
            Assert.Equal("ギェ", MoraExtensions._ToTextFromJoinedMora("gye"));
            Assert.Equal("イェ", MoraExtensions._ToTextFromJoinedMora("ye"));

            // Convert Unvoice to Voiced
            Assert.Equal("シ", MoraExtensions._ToTextFromJoinedMora("shI"));

            // Return unassigned mora
            Assert.Equal("", MoraExtensions._ToTextFromJoinedMora(""));
            Assert.Equal("xxx", MoraExtensions._ToTextFromJoinedMora("xxx"));
        }
    }
}
