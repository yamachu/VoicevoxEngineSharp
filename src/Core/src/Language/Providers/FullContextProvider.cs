using System;
using System.Collections.Generic;
using SharpOpenJTalk.Lang;

namespace VoicevoxEngineSharp.Core.Language.Providers
{
    public class FullContextProvider : IFullContextProvider
    {
        private OpenJTalkAPI instance { get; init; }
        public FullContextProvider(string dictPath)
        {
            var instance = new OpenJTalkAPI();
            if (!instance.Initialize(dictPath))
            {
                throw new ArgumentException("OpenJTalk initialize failed, dictPath is incorrect");
            }

            this.instance = instance;
        }

        IEnumerable<string> IFullContextProvider.ToFullContextLabels(string text)
        {
            return instance.GetLabels(text);
        }
    }
}
