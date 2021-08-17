using System;
using System.Collections.Generic;
using System.Linq;

namespace VoicevoxEngineSharp.Core.Language.Extensions
{
    public static class LINQExtensions
    {
        public static int FindIndex<T>(this IEnumerable<T> collections, Func<T?, bool> func)
        {
            var maybeFound = collections
                .Select((elm, index) => (elm, index))
                .First((elmWithIndex) => func(elmWithIndex.elm));
            return maybeFound.Item2;
        }

        public static int FindIndexOrDefault<T>(this IEnumerable<T> collections, Func<T, bool> func, int defaultIndex = -1)
        {
            var maybeFound = collections
                .Select((elm, index) => (elm, index))
                .FirstOrDefault((elmWithIndex) => func(elmWithIndex.elm), (elm: default(T), index: defaultIndex));
            return maybeFound.Item2;
        }
    }
}
