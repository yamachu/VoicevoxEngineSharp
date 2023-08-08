using System;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Enum
{
    /// <summary>
    /// ハードウェアアクセラレーションモードを設定する設定値。
    /// </summary>
    public enum AccelerationMode : int
    {
        /// <summary>
        /// 実行環境に合った適切なハードウェアアクセラレーションモードを選択する
        /// </summary>
        AUTO = 0,
        /// <summary>
        /// ハードウェアアクセラレーションモードを"CPU"に設定する
        /// </summary>
        CPU = 1,
        /// <summary>
        /// ハードウェアアクセラレーションモードを"GPU"に設定する
        /// </summary>
        GPU = 2,
    }

    internal static class AccelerationModeExt
    {
        public static AccelerationMode FromNative(this VoicevoxAccelerationMode mode)
        {
            return mode switch
            {
                VoicevoxAccelerationMode.VOICEVOX_ACCELERATION_MODE_AUTO => AccelerationMode.AUTO,
                VoicevoxAccelerationMode.VOICEVOX_ACCELERATION_MODE_CPU => AccelerationMode.CPU,
                VoicevoxAccelerationMode.VOICEVOX_ACCELERATION_MODE_GPU => AccelerationMode.GPU,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
        }

        public static VoicevoxAccelerationMode ToNative(this AccelerationMode mode)
        {
            return mode switch
            {
                AccelerationMode.AUTO => VoicevoxAccelerationMode.VOICEVOX_ACCELERATION_MODE_AUTO,
                AccelerationMode.CPU => VoicevoxAccelerationMode.VOICEVOX_ACCELERATION_MODE_CPU,
                AccelerationMode.GPU => VoicevoxAccelerationMode.VOICEVOX_ACCELERATION_MODE_GPU,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
        }
    }
}
