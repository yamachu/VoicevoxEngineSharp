using System;
using System.Runtime.InteropServices;
using VoicevoxEngineSharp.Core.Enum;
using VoicevoxEngineSharp.Core.Native;
using VoicevoxEngineSharp.Core.Struct;

namespace VoicevoxEngineSharp.Core
{
    internal class SynthesizerHandle : SafeHandle
    {
        public SynthesizerHandle(IntPtr intPtr) : base(IntPtr.Zero, true)
        {
            this.SetHandle(intPtr);
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            unsafe
            {
                CoreUnsafe.voicevox_synthesizer_delete((VoicevoxSynthesizer*)handle.ToPointer());
            }
            return true;
        }


        public static unsafe implicit operator VoicevoxSynthesizer*(SynthesizerHandle handle) => (VoicevoxSynthesizer*)handle.handle.ToPointer();
    }

    public class Synthesizer : IDisposable
    {
        internal SynthesizerHandle Handle { get; private set; }
        private bool _disposed = false;

        private unsafe Synthesizer(VoicevoxSynthesizer* synthesizerHandle)
        {
            Handle = new SynthesizerHandle(new IntPtr(synthesizerHandle));
        }

        public static ResultCode New(OpenJtalk openJtalk, InitializeOptions options, out Synthesizer synthesizer)
        {
            unsafe
            {
                var nativeOptions = options.ToNative();

                var synthesizerHandle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VoicevoxSynthesizer)));
                var p = (VoicevoxSynthesizer*)synthesizerHandle.ToPointer();
                var result = CoreUnsafe.voicevox_synthesizer_new_with_initialize((OpenJtalkRc*)openJtalk.Handle, nativeOptions, &p).FromNative();
                synthesizer = new Synthesizer(p);

                return result;
            }
        }

        public ResultCode LoadVoiceModel(VoiceModel voiceModel)
        {
            unsafe
            {
                return CoreUnsafe.voicevox_synthesizer_load_voice_model((VoicevoxSynthesizer*)Handle, (VoicevoxVoiceModel*)voiceModel.Handle).FromNative();
            }
        }

        public ResultCode UnloadVoiceModel(string modelId)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(modelId))
                {
                    return CoreUnsafe.voicevox_synthesizer_unload_voice_model((VoicevoxSynthesizer*)Handle, ptr).FromNative();
                }
            }
        }

        public bool IsGpuMode
        {
            get
            {
                unsafe
                {
                    return CoreUnsafe.voicevox_synthesizer_is_gpu_mode((VoicevoxSynthesizer*)Handle);
                }
            }
        }

        public bool IsLoadedVoiceModel(string modelId)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(modelId))
                {
                    return CoreUnsafe.voicevox_synthesizer_is_loaded_voice_model((VoicevoxSynthesizer*)Handle, ptr);
                }
            }
        }

        public string MetasJson
        {
            get
            {
                unsafe
                {
                    var ptr = CoreUnsafe.voicevox_synthesizer_get_metas_json((VoicevoxSynthesizer*)Handle);
                    return StringConvertCompat.ToUTF8String(ptr);
                }
            }
        }

        public ResultCode CreateAudioQuery(string text, uint styleId, AudioQueryOptions options, out string audioQueryJson)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(text))
                {
                    var nativeOptions = options.ToNative();
                    byte* resultJsonPtr;

                    var result = CoreUnsafe.voicevox_synthesizer_audio_query((VoicevoxSynthesizer*)Handle, ptr, styleId, nativeOptions, &resultJsonPtr).FromNative();
                    audioQueryJson = StringConvertCompat.ToUTF8String(resultJsonPtr);

                    return result;
                }
            }
        }

        public ResultCode CreateAccentPhrases(string text, uint styleId, AccentPhrasesOptions options, out string accentPhrasesJson)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(text))
                {
                    var nativeOptions = options.ToNative();
                    byte* resultJsonPtr;

                    var result = CoreUnsafe.voicevox_synthesizer_create_accent_phrases((VoicevoxSynthesizer*)Handle, ptr, styleId, nativeOptions, &resultJsonPtr).FromNative();
                    accentPhrasesJson = StringConvertCompat.ToUTF8String(resultJsonPtr);

                    return result;
                }
            }
        }

        public ResultCode ReplaceMoraData(string accentPhrasesJson, uint styleId, out string outputAccentPhrasesJson)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(accentPhrasesJson))
                {
                    byte* resultJsonPtr;

                    var result = CoreUnsafe.voicevox_synthesizer_replace_mora_data((VoicevoxSynthesizer*)Handle, ptr, styleId, &resultJsonPtr).FromNative();
                    outputAccentPhrasesJson = StringConvertCompat.ToUTF8String(resultJsonPtr);

                    return result;
                }
            }
        }

        public ResultCode ReplacePhonemeLength(string accentPhrasesJson, uint styleId, out string outputAccentPhrasesJson)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(accentPhrasesJson))
                {
                    byte* resultJsonPtr;

                    var result = CoreUnsafe.voicevox_synthesizer_replace_phoneme_length((VoicevoxSynthesizer*)Handle, ptr, styleId, &resultJsonPtr).FromNative();
                    outputAccentPhrasesJson = StringConvertCompat.ToUTF8String(resultJsonPtr);

                    return result;
                }
            }
        }

        public ResultCode ReplaceMoraPitch(string accentPhrasesJson, uint styleId, out string outputAccentPhrasesJson)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(accentPhrasesJson))
                {
                    byte* resultJsonPtr;

                    var result = CoreUnsafe.voicevox_synthesizer_replace_mora_pitch((VoicevoxSynthesizer*)Handle, ptr, styleId, &resultJsonPtr).FromNative();
                    outputAccentPhrasesJson = StringConvertCompat.ToUTF8String(resultJsonPtr);

                    return result;
                }
            }
        }

        public ResultCode Synthesis(string audioQueryJson, uint styleId, SynthesisOptions options, out nuint outputWavLength, out byte[]? outputWav)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(audioQueryJson))
                {
                    fixed (nuint* ptr2 = &outputWavLength)
                    {
                        var nativeOptions = options.ToNative();
                        byte* resultWavPtr;

                        var result = CoreUnsafe.voicevox_synthesizer_synthesis((VoicevoxSynthesizer*)Handle, ptr, styleId, nativeOptions, ptr2, &resultWavPtr);
                        if (result == VoicevoxResultCode.VOICEVOX_RESULT_OK)
                        {
                            var i = 0;
                            var outputWavLengthInt = (int)outputWavLength;
                            var outputWavTmp = new byte[outputWavLength];
                            while (i < outputWavLengthInt)
                            {
                                outputWavTmp[i] = resultWavPtr[i];
                                i++;
                            }
                            outputWav = outputWavTmp;
                            CoreUnsafe.voicevox_wav_free(resultWavPtr);
                        }
                        else
                        {
                            outputWav = null;
                        }

                        return result.FromNative();
                    }

                }
            }
        }

        public ResultCode Tts(string text, uint styleId, TtsOptions options, out nuint outputWavLength, out byte[]? outputWav)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(text))
                {
                    fixed (nuint* ptr2 = &outputWavLength)
                    {
                        var nativeOptions = options.ToNative();
                        byte* resultWavPtr;

                        var result = CoreUnsafe.voicevox_synthesizer_tts((VoicevoxSynthesizer*)Handle, ptr, styleId, nativeOptions, ptr2, &resultWavPtr);
                        if (result == VoicevoxResultCode.VOICEVOX_RESULT_OK)
                        {
                            var i = 0;
                            var outputWavLengthInt = (int)outputWavLength;
                            var outputWavTmp = new byte[outputWavLength];
                            while (i < outputWavLengthInt)
                            {
                                outputWavTmp[i] = resultWavPtr[i];
                                i++;
                            }
                            outputWav = outputWavTmp;
                            CoreUnsafe.voicevox_wav_free(resultWavPtr);
                        }
                        else
                        {
                            outputWav = null;
                        }

                        return result.FromNative();
                    }

                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Handle != null && !Handle.IsInvalid)
                    {
                        Handle.Dispose();
                    }
                }

                _disposed = true;
            }
        }
    }
}
