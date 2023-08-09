using System;
using VoicevoxEngineSharp.Core.Enum;
using VoicevoxEngineSharp.Core.Native;
using VoicevoxEngineSharp.Core.Struct;

namespace VoicevoxEngineSharp.Core
{
    public class Utils
    {
        public static ResultCode CreateSupportedDevicesJson(out string? supportedDevicesJson)
        {
            unsafe
            {
                byte* jsonPtr;
                var result = CoreUnsafe.voicevox_create_supported_devices_json(&jsonPtr);
                if (result == VoicevoxResultCode.VOICEVOX_RESULT_OK)
                {
                    supportedDevicesJson = StringConvertCompat.ToUTF8String(jsonPtr);
                }
                else
                {
                    supportedDevicesJson = null;
                }

                CoreUnsafe.voicevox_json_free(jsonPtr);

                return result.FromNative();
            }
        }
    }
}
