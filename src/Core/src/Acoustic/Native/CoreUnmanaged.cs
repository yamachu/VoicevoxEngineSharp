using System.Runtime.InteropServices;

namespace VoicevoxEngineSharp.Core.Acoustic.Native
{
    internal class CoreUnmanaged
    {
        private const string DllName = "core";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool initialize(string root_dir_path, bool use_gpu);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr metas();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool yukarin_s_forward(
            int length,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] long[] phoneme_list,
            [In][MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] long[] speaker_id,
            [In][Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] float[] output);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool yukarin_sa_forward(
            int length,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] long[] vowel_phoneme_list,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] long[] consonant_phoneme_list,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] long[] start_accent_list,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] long[] end_accent_list,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] long[] start_accent_phrase_list,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] long[] end_accent_phrase_list,
            [In][MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] long[] speaker_id,
            [In][Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] float[] output);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool decode_forward(
            int length,
            int phoneme_size,
            [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] float[] f0,
            [In] float[] phoneme,
            [In][MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] long[] speaker_id,
            float[] output);
    }
}
