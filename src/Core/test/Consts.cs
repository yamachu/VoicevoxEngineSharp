using System;
using System.IO;

namespace VoicevoxEngineSharp.Core.Test
{
    public class Consts
    {
        public static readonly string OpenJTalkDictDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "open_jtalk_dic_utf_8-1.11");
        public static readonly string SampleVoiceModel = Path.Combine(Helper.GetBaseVoicevoxCoreDirectory(), "model", "sample.vvm");
    }
}
