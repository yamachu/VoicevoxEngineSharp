using System;
using VoicevoxEngineSharp.Core.Enum;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Struct
{
    /// <summary>
    /// voicevox_synthesizer_new_with_initialize のオプション。
    /// </summary>
    public struct InitializeOptions
    {
        /// <summary>
        /// ハードウェアアクセラレーションモード
        /// </summary>
        public AccelerationMode AccelerationMode { get; set; }
        /// <summary>
        /// CPU利用数を指定
        /// 0を指定すると環境に合わせたCPUが利用される
        /// </summary>
        public ushort CpuNumThreads { get; set; }
        /// <summary>
        /// 全てのモデルを読み込む
        /// </summary>
        public bool LoadAllModels { get; set; }
    }

    internal static class InitializeOptionsExt
    {
        internal static VoicevoxInitializeOptions ToNative(this InitializeOptions initializeOptions)
        {
            return new VoicevoxInitializeOptions
            {
                acceleration_mode = initializeOptions.AccelerationMode.ToNative(),
                cpu_num_threads = initializeOptions.CpuNumThreads,
                load_all_models = initializeOptions.LoadAllModels
            };
        }
    }
}
