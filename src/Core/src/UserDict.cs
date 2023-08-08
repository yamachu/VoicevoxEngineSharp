using System;
using System.Runtime.InteropServices;
using VoicevoxEngineSharp.Core.Enum;
using VoicevoxEngineSharp.Core.Native;
using VoicevoxEngineSharp.Core.Struct;

namespace VoicevoxEngineSharp.Core
{
    internal class UserDictHandle : SafeHandle
    {
        public UserDictHandle(IntPtr intPtr) : base(IntPtr.Zero, true)
        {
            this.SetHandle(intPtr);
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        public static UserDictHandle New()
        {
            unsafe
            {
                return new UserDictHandle(new IntPtr(CoreUnsafe.voicevox_user_dict_new()));
            }
        }

        protected override bool ReleaseHandle()
        {
            unsafe
            {
                CoreUnsafe.voicevox_user_dict_delete((VoicevoxUserDict*)handle.ToPointer());
            }
            return true;
        }


        public static unsafe implicit operator VoicevoxUserDict*(UserDictHandle handle) => (VoicevoxUserDict*)handle.handle.ToPointer();
    }

    public class UserDict
    {
        internal UserDictHandle Handle { get; private set; }

        public UserDict()
        {
            Handle = UserDictHandle.New();
        }

        public ResultCode Import(UserDict otherDict)
        {
            unsafe
            {
                return CoreUnsafe.voicevox_user_dict_import((VoicevoxUserDict*)Handle, (VoicevoxUserDict*)otherDict.Handle).FromNative();
            }
        }

        public ResultCode Load(string dictPath)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(dictPath))
                {
                    return CoreUnsafe.voicevox_user_dict_load((VoicevoxUserDict*)Handle, ptr).FromNative();
                }
            }
        }

        public ResultCode Save(string dictPath)
        {
            unsafe
            {
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(dictPath))
                {
                    return CoreUnsafe.voicevox_user_dict_save((VoicevoxUserDict*)Handle, ptr).FromNative();
                }
            }
        }

        public ResultCode ToJson(out string? json)
        {
            unsafe
            {
                byte* ptr;
                var result = CoreUnsafe.voicevox_user_dict_to_json((VoicevoxUserDict*)Handle, &ptr);
                if (result == VoicevoxResultCode.VOICEVOX_RESULT_OK)
                {
                    json = StringConvertCompat.ToUTF8String(ptr);
                    CoreUnsafe.voicevox_json_free(ptr);
                }
                else
                {
                    json = null;
                }
                return result.FromNative();
            }
        }

        public ResultCode AddWord(UserDictWord word, out string? wordUuid)
        {
            unsafe
            {
                byte* ptr = stackalloc byte[16];
                var wordNative = word.ToNative();
                var result = CoreUnsafe.voicevox_user_dict_add_word((VoicevoxUserDict*)Handle, &wordNative, ptr);
                if (result == VoicevoxResultCode.VOICEVOX_RESULT_OK)
                {
                    wordUuid = NativeUuid.ToUUIDv4(ptr);
                }
                else
                {
                    wordUuid = null;
                }
                return result.FromNative();
            }
        }

        public ResultCode UpdateWord(string wordUuid, UserDictWord word)
        {
            unsafe
            {
                var wordNative = word.ToNative();
                fixed (byte* ptr = NativeUuid.ToUUIDv4ByteArray(wordUuid))
                {
                    return CoreUnsafe.voicevox_user_dict_update_word((VoicevoxUserDict*)Handle, ptr, &wordNative).FromNative();
                }
            }
        }

        public ResultCode RemoveWord(string wordUuid)
        {
            unsafe
            {
                fixed (byte* ptr = NativeUuid.ToUUIDv4ByteArray(wordUuid))
                {
                    return CoreUnsafe.voicevox_user_dict_remove_word((VoicevoxUserDict*)Handle, ptr).FromNative();
                }
            }
        }
    }
}
