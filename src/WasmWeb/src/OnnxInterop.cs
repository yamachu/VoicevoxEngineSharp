using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace VoicevoxEngineSharp.WasmWeb
{
    public static partial class OnnxInterop
    {
        #region YukarinSForward

        [JSImport("inference.yukarinSForward", "synthesis.js")]
        [return: JSMarshalAs<JSType.Promise<JSType.Object>>]
        private static partial Task<JSObject> YukarinSForwardAsyncInternal(
            int length,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] phonemeList,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] speakerId);

        public static async Task<float[]> YukarinSForwardAsync(int length, long[] phonemeList, long[] speakerId)
        {
            var phonemeListInt = Array.ConvertAll(phonemeList, v => (int)v);
            var speakerIdInt = Array.ConvertAll(speakerId, v => (int)v);
            var resultObj = await YukarinSForwardAsyncInternal(length, phonemeListInt, speakerIdInt);
            var result = PromiseArrayHelper.UnwrapFloatArray(resultObj);
            return Array.ConvertAll(result, v => (float)v);
        }

        #endregion

        #region YukarinSaForward

        [JSImport("inference.yukarinSaForward", "synthesis.js")]
        [return: JSMarshalAs<JSType.Promise<JSType.Object>>]
        private static partial Task<JSObject> YukarinSaForwardAsyncInternal(
            int length,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] vowelPhonemeList,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] consonantPhonemeList,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] startAccentList,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] endAccentList,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] startAccentPhraseList,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] endAccentPhraseList,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] speakerId);

        public static async Task<float[]> YukarinSaForwardAsync(
            int length,
            long[] vowelPhonemeList,
            long[] consonantPhonemeList,
            long[] startAccentList,
            long[] endAccentList,
            long[] startAccentPhraseList,
            long[] endAccentPhraseList,
            long[] speakerId)
        {
            var resultObj = await YukarinSaForwardAsyncInternal(
                length,
                Array.ConvertAll(vowelPhonemeList, v => (int)v),
                Array.ConvertAll(consonantPhonemeList, v => (int)v),
                Array.ConvertAll(startAccentList, v => (int)v),
                Array.ConvertAll(endAccentList, v => (int)v),
                Array.ConvertAll(startAccentPhraseList, v => (int)v),
                Array.ConvertAll(endAccentPhraseList, v => (int)v),
                Array.ConvertAll(speakerId, v => (int)v));
            var result = PromiseArrayHelper.UnwrapFloatArray(resultObj);
            return Array.ConvertAll(result, v => (float)v);
        }

        #endregion

        #region DecodeForward

        [JSImport("inference.decodeForward", "synthesis.js")]
        [return: JSMarshalAs<JSType.Promise<JSType.Object>>]
        private static partial Task<JSObject> DecodeForwardAsyncInternal(
            int length,
            int phonemeSize,
            [JSMarshalAs<JSType.Array<JSType.Number>>] double[] f0,
            [JSMarshalAs<JSType.Array<JSType.Number>>] double[] phoneme,
            [JSMarshalAs<JSType.Array<JSType.Number>>] int[] speakerId);
        public static async Task<float[]> DecodeForwardAsync(
            int length,
            int phonemeSize,
            float[] f0,
            float[] phoneme,
            long[] speakerId)
        {
            var resultObj = await DecodeForwardAsyncInternal(
                length,
                phonemeSize,
                Array.ConvertAll(f0, v => (double)v),
                Array.ConvertAll(phoneme, v => (double)v),
                Array.ConvertAll(speakerId, v => (int)v));
            var result = PromiseArrayHelper.UnwrapFloatArray(resultObj);
            return Array.ConvertAll(result, v => (float)v);
        }

        #endregion
    }
}
