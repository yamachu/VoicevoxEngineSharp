using System;
using System.Runtime.InteropServices;
using VoicevoxEngineSharp.Core.Enum;
using VoicevoxEngineSharp.Core.Native;
using VoicevoxEngineSharp.Core.Struct;

namespace VoicevoxEngineSharp.Core
{
    internal class VoiceModelHandle : SafeHandle
    {
        public VoiceModelHandle(IntPtr intPtr) : base(IntPtr.Zero, true)
        {
            this.SetHandle(intPtr);
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            unsafe
            {
                CoreUnsafe.voicevox_voice_model_delete((VoicevoxVoiceModel*)handle.ToPointer());
            }
            return true;
        }


        public static unsafe implicit operator VoicevoxVoiceModel*(VoiceModelHandle handle) => (VoicevoxVoiceModel*)handle.handle.ToPointer();
    }

    public class VoiceModel
    {
        internal VoiceModelHandle Handle { get; private set; }

        private unsafe VoiceModel(VoicevoxVoiceModel* modelHandle)
        {
            Handle = new VoiceModelHandle(new IntPtr(modelHandle));
        }

        public static ResultCode New(string modelPath, out VoiceModel voiceModel)
        {
            unsafe
            {
                var modelHandle = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VoicevoxVoiceModel)));
                var p = (VoicevoxVoiceModel*)modelHandle.ToPointer();
                fixed (byte* ptr = System.Text.Encoding.UTF8.GetBytes(modelPath))
                {
                    var result = CoreUnsafe.voicevox_voice_model_new_from_path(ptr, &p).FromNative();
                    voiceModel = new VoiceModel(p);

                    return result;
                }
            }
        }

        public string Id
        {
            get
            {
                unsafe
                {
                    var ptr = CoreUnsafe.voicevox_voice_model_id((VoicevoxVoiceModel*)Handle);
                    return StringConvertCompat.ToUTF8String(ptr);
                }
            }
        }

        public string MetasJson
        {
            get
            {
                unsafe
                {
                    var ptr = CoreUnsafe.voicevox_voice_model_get_metas_json((VoicevoxVoiceModel*)Handle);
                    return StringConvertCompat.ToUTF8String(ptr);
                }
            }
        }
    }
}
