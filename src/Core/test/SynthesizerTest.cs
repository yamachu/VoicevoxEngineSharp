using System;
using VoicevoxEngineSharp.Core.Enum;
using VoicevoxEngineSharp.Core.Struct;
using Xunit;

namespace VoicevoxEngineSharp.Core.Test
{
    public class SynthesizerTest
    {
        [Fact]
        public void Tts()
        {
            OpenJtalk.New(Consts.OpenJTalkDictDir, out var openJtalk);
            var initializeOptions = InitializeOptions.Default();
            var synthesizerResult = Synthesizer.New(openJtalk, initializeOptions, out var synthesizer);

            VoiceModel.New(Consts.SampleVoiceModel, out var voiceModel);
            synthesizer.LoadVoiceModel(voiceModel);

            Assert.Equal(ResultCode.RESULT_OK, synthesizerResult);
            Assert.NotEmpty(synthesizer.MetasJson);

            var ttsResult = synthesizer.Tts("こんにちは", 0, TtsOptions.Default(), out var wavLength, out var wav);

            Assert.Equal(ResultCode.RESULT_OK, ttsResult);
            Assert.True(wavLength > 0);
            Assert.NotNull(wav);
        }
    }
}
