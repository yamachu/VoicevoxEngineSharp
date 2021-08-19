using SharpOpenJTalk;

namespace VoicevoxEngineSharp.Core.Language.Providers
{
    public class FullContextProvider : IFullContextProvider
    {
        private OpenJTalkAPI instance { get; init; }
        public FullContextProvider(string dictPath, string modelPath)
        {
            var instance = new OpenJTalkAPI();
            if (!instance.Initialize(dictPath, modelPath))
            {
                throw new ArgumentException("OpenJTalk initialize failed, dictPath or modelPath is incorrect");
            }

            this.instance = instance;
        }

        IEnumerable<string> IFullContextProvider.ToFullContextLabels(string text)
        {
            return instance.GetLabels(text);
        }
    }
}
