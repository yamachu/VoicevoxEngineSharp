using VoicevoxEngineSharp.Core.Acoustic.Native;

namespace VoicevoxEngineSharp.Core.Acoustic.Usecases
{
    public class SynthesisEngine
    {
        public static bool Initialize(string yukarin_s_forwarder_path,
            string yukarin_sa_forwarder_path,
            string decode_forwarder_path,
            bool use_gpu)
        {
            return EachCppForwarder.Initialize(yukarin_s_forwarder_path, yukarin_sa_forwarder_path, decode_forwarder_path, use_gpu);
        }
    }
}
