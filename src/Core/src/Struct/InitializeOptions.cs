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

        public static InitializeOptions Default()
        {
            return InitializeOptionsDefault.Value;
        }
    }

    internal static class InitializeOptionsDefault
    {
        public static readonly InitializeOptions Value = CoreUnsafe.voicevox_make_default_initialize_options().FromNative();
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

        internal static InitializeOptions FromNative(this VoicevoxInitializeOptions initializeOptions)
        {
            return new InitializeOptions
            {
                AccelerationMode = initializeOptions.acceleration_mode.FromNative(),
                CpuNumThreads = initializeOptions.cpu_num_threads,
                LoadAllModels = initializeOptions.load_all_models
            };
        }
    }
}
