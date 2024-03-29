﻿using System.Runtime.InteropServices;

namespace VoicevoxEngineSharp.Core.Acoustic.Native
{
    internal class Core : IAcousticDNNLibrary
    {
        public static bool Initialize(string root_dir_path, bool use_gpu)
            => CoreUnmanaged.initialize(root_dir_path, use_gpu);

        public static string Metas()
        {
            var resultPtr = CoreUnmanaged.metas();
            var result = Marshal.PtrToStringUTF8(resultPtr);
            Marshal.FreeHGlobal(resultPtr);

            return result;
        }

        public static float[] YukarinSForward(int length, long[] phoneme_list, long[] speaker_id)
        {
            var output = new float[length];
            CoreUnmanaged.yukarin_s_forward(length, phoneme_list, speaker_id, output);
            return output;
        }

        public static float[] YukarinSaForward(int length,
            long[] vowel_phoneme_list, long[] consonant_phoneme_list,
            long[] start_accent_list, long[] end_accent_list,
            long[] start_accent_phrase_list, long[] end_accent_phrase_list,
            long[] speaker_id)
        {
            var output = new float[length];
            CoreUnmanaged.yukarin_sa_forward(
                length, vowel_phoneme_list, consonant_phoneme_list, start_accent_list, end_accent_list, start_accent_phrase_list, end_accent_phrase_list, speaker_id, output);
            return output;
        }

        public static float[] DecodeForward(int length, int phoneme_size, float[] f0, float[] phoneme, long[] speaker_id)
        {
            var output = new float[length * 256];
            CoreUnmanaged.decode_forward(length, phoneme_size, f0, phoneme, speaker_id, output);
            return output;
        }
    }
}
