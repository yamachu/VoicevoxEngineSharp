namespace VoicevoxEngineSharp.Core.Acoustic.Native
{
    internal interface IAcousticDNNLibrary
    {
        static bool Initialize(
            string yukarin_s_forwarder_path,
            string yukarin_sa_forwarder_path,
            string decode_forwarder_path,
            bool use_gpu) => throw new NotImplementedException();

        static float[] YukarinSForward(int length, long[] phoneme_list, long[] speaker_id)
            => throw new NotImplementedException();

        static float[] YukarinSaForward(int length,
            long[] vowel_phoneme_list, long[] consonant_phoneme_list,
            long[] start_accent_list, long[] end_accent_list,
            long[] start_accent_phrase_list, long[] end_accent_phrase_list,
            long[] speaker_id)
            => throw new NotImplementedException();

        static float[] DecodeForward(int length, int phoneme_size, float[] f0, float[] phoneme, long[] speaker_id)
            => throw new NotImplementedException();
    }
}
