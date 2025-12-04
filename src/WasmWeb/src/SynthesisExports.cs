#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SharpOpenJTalk.Lang;
using VoicevoxEngineSharp.Core.Acoustic.Usecases;
using VoicevoxEngineSharp.Core.Language.Providers;
using VoicevoxEngineSharp.Core.Language.Usecases;
using VoicevoxEngineSharp.Core.Usecases;
using VoicevoxEngineSharp.WasmWeb.Models;

namespace VoicevoxEngineSharp.WasmWeb
{
    [JsonSerializable(typeof(AccentPhrase[]))]
    [JsonSerializable(typeof(AudioQuery))]
    [JsonSerializable(typeof(Mora))]
    internal partial class SynthesisJsonContext : JsonSerializerContext
    {
    }

    public static partial class SynthesisExports
    {
        private static Synthesis? _synthesis;
        private static bool _initialized;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            TypeInfoResolver = SynthesisJsonContext.Default
        };

        private class OpenJTalkFullContextProvider : IFullContextProvider
        {
            private readonly OpenJTalkAPI _instance;

            public OpenJTalkFullContextProvider(string dictPath)
            {
                _instance = new OpenJTalkAPI();
                // maybe /tmp/open_jtalk_dic_utf_8-1.11
                if (!_instance.Initialize(dictPath))
                {
                    throw new ArgumentException("OpenJTalk initialize failed, dictPath is incorrect");
                }
            }

            public IEnumerable<string> ToFullContextLabels(string text)
            {
                return _instance.GetLabels(text);
            }
        }

        [JSExport]
        public static void Initialize(string openJTalkDictPath)
        {
            if (_initialized)
            {
                return;
            }

            var fullContextProvider = new OpenJTalkFullContextProvider(openJTalkDictPath);
            var textToUtterance = new TextToUtterance(fullContextProvider);

            var synthesisEngine = new SynthesisEngine(
                OnnxInterop.YukarinSForwardAsync,
                OnnxInterop.YukarinSaForwardAsync,
                OnnxInterop.DecodeForwardAsync
            );

            _synthesis = new Synthesis(textToUtterance, synthesisEngine);
            _initialized = true;
        }

        [JSExport]
        public static async Task<string> CreateAccentPhrases(string text, int speakerId)
        {
            EnsureInitialized();
            var accentPhrases = await _synthesis!.CreateAccentPhrases(text, speakerId);
            var result = accentPhrases.Select(ap => AccentPhrase.FromDomain(ap)).ToArray();
            return JsonSerializer.Serialize(result, SynthesisJsonContext.Default.AccentPhraseArray);
        }

        [JSExport]
        public static async Task<string> AudioQuery(string text, int speakerId)
        {
            EnsureInitialized();
            var accentPhrases = await _synthesis!.CreateAccentPhrases(text, speakerId);

            var domainAudioQuery = new Core.Acoustic.Models.AudioQuery
            {
                AccentPhrases = accentPhrases,
                SpeedScale = 1,
                PitchScale = 0,
                IntonationScale = 1,
                VolumeScale = 1,
                PrePhonemeLength = 0.1f,
                PostPhonemeLength = 0.1f,
                PauseLength = null,
                PauseLengthScale = 1.0f,
                OutputSamplingRate = 24000,
                OutputStereo = false,
            };

            var audioQuery = Models.AudioQuery.FromDomain(domainAudioQuery);
            return JsonSerializer.Serialize(audioQuery, SynthesisJsonContext.Default.AudioQuery);
        }

        [JSExport]
        public static async Task<string> ReplaceMoraData(string accentPhrasesJson, int speakerId)
        {
            EnsureInitialized();
            var accentPhrases = JsonSerializer.Deserialize(accentPhrasesJson, SynthesisJsonContext.Default.AccentPhraseArray)!;
            var domainAccentPhrases = accentPhrases.Select(ap => AccentPhrase.ToDomain(ap));
            var result = await _synthesis!.ReplaceMoraData(domainAccentPhrases, speakerId);
            return JsonSerializer.Serialize(result.Select(ap => AccentPhrase.FromDomain(ap)).ToArray(), SynthesisJsonContext.Default.AccentPhraseArray);
        }

        [JSExport]
        public static async Task<string> ReplaceMoraLength(string accentPhrasesJson, int speakerId)
        {
            EnsureInitialized();
            var accentPhrases = JsonSerializer.Deserialize(accentPhrasesJson, SynthesisJsonContext.Default.AccentPhraseArray)!;
            var domainAccentPhrases = accentPhrases.Select(ap => AccentPhrase.ToDomain(ap));
            var result = await _synthesis!.ReplaceMoraLength(domainAccentPhrases, speakerId);
            return JsonSerializer.Serialize(result.Select(ap => AccentPhrase.FromDomain(ap)).ToArray(), SynthesisJsonContext.Default.AccentPhraseArray);
        }

        [JSExport]
        public static async Task<string> ReplaceMoraPitch(string accentPhrasesJson, int speakerId)
        {
            EnsureInitialized();
            var accentPhrases = JsonSerializer.Deserialize(accentPhrasesJson, SynthesisJsonContext.Default.AccentPhraseArray)!;
            var domainAccentPhrases = accentPhrases.Select(ap => AccentPhrase.ToDomain(ap));
            var result = await _synthesis!.ReplaceMoraPitch(domainAccentPhrases, speakerId);
            return JsonSerializer.Serialize(result.Select(ap => AccentPhrase.FromDomain(ap)).ToArray(), SynthesisJsonContext.Default.AccentPhraseArray);
        }

        [JSExport]
        [return: JSMarshalAs<JSType.Promise<JSType.Object>>]
        public static async Task<JSObject> SynthesisWave(string audioQueryJson, int speakerId)
        {
            EnsureInitialized();
            var audioQuery = JsonSerializer.Deserialize(audioQueryJson, SynthesisJsonContext.Default.AudioQuery)!;
            var domainAudioQuery = Models.AudioQuery.ToDomain(audioQuery);
            var wave = await _synthesis!.SynthesisWave(domainAudioQuery, speakerId);
            // Convert to double[] and wrap in JSObject
            var waveDouble = wave.Select(v => (double)v).ToArray();
            return PromiseArrayHelper.WrapFloatArray(waveDouble);
        }

        private static void EnsureInitialized()
        {
            if (!_initialized || _synthesis == null)
            {
                throw new InvalidOperationException("SynthesisExports is not initialized. Call Initialize() first.");
            }
        }
    }
}
