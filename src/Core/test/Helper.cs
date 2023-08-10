using System;
using System.IO;
using System.Reflection;

namespace VoicevoxEngineSharp.Core.Test
{
    public class Helper
    {
        public static string GetProjectDirectory()
        {
            var maybeProjectDir = Environment.GetEnvironmentVariable("_TEST_PROJECT_DIR_");
            var projectDir = maybeProjectDir ?? Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", "..", ".."));
            return projectDir;
        }

        public static string GetTestFixturesDirectory()
        {
            return Path.Combine(GetProjectDirectory(), "fixtures");
        }

        public static string GetBaseVoicevoxCoreDirectory()
        {
            return Path.Combine(GetProjectDirectory(), "..", "binding", "voicevox_core");
        }

        public static string GetTestResultsDirectory()
        {
            return Path.Combine(GetProjectDirectory(), "results");
        }
    }
}
