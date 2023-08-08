using System;
using System.IO;
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
            OpenJtalk.New(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "open_jtalk_dic_utf_8-1.11"), out var openJtalk);
            var synthesizerResult = Synthesizer.New(openJtalk, new InitializeOptions
            {
                AccelerationMode = AccelerationMode.AUTO,
                CpuNumThreads = 0,
                LoadAllModels = false, // trueだと配置問題があるのでcrashする
            }, out var synthesizer);

            VoiceModel.New("/Users/yamachu/Projects/github.com/yamachu/VoicevoxEngineSharp/src/Core/test/bin/Debug/net6.0/runtimes/osx/native/model/0.vvm", out var voiceModel);
            synthesizer.LoadVoiceModel(voiceModel);

            Assert.Equal(ResultCode.RESULT_OK, synthesizerResult);
            Assert.NotEmpty(synthesizer.MetasJson);

            var ttsResult = synthesizer.Tts("こんにちは", 0, new TtsOptions
            {
                EnableInterrogativeUpspeak = false,
                Kana = false,
            }, out var wavLength, out var wav);
            Assert.Equal(ResultCode.RESULT_OK, ttsResult);
            Assert.True(wavLength > 0);
            Assert.NotNull(wav);

            using var writer = new BinaryWriter(File.OpenWrite("test.wav"));
            writer.Write(wav);
        }
    }
}
