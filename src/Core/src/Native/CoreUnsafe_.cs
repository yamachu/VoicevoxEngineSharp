using System;
using System.Runtime.InteropServices;

namespace VoicevoxEngineSharp.Core.Native
{
    // bindgenやcsbindgenで生成されなかったコードを手書きしている
    internal static unsafe partial class CoreUnsafe
    {
        [DllImport(__DllName, EntryPoint = "voicevox_user_dict_add_word", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern VoicevoxResultCode voicevox_user_dict_add_word(VoicevoxUserDict* user_dict, VoicevoxUserDictWord* word, byte* /* [16] */ output_word_uuid);

        [DllImport(__DllName, EntryPoint = "voicevox_user_dict_update_word", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern VoicevoxResultCode voicevox_user_dict_update_word(VoicevoxUserDict* user_dict, byte* /* [16] */ word_uuid, VoicevoxUserDictWord* word);

        [DllImport(__DllName, EntryPoint = "voicevox_user_dict_remove_word", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern VoicevoxResultCode voicevox_user_dict_remove_word(VoicevoxUserDict* user_dict, byte* /* [16] */ word_uuid);
    }
}
