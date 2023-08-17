using System;
using System.Text;

namespace VoicevoxEngineSharp.Core
{
    internal static class StringConvertCompat
    {
        // Marshal.PtrToStringUTF8を使いたいけど、.netstandard2.0にはないので自前で実装
        internal static unsafe string ToUTF8String(byte* ptr)
        {
            var length = 0;
            while (ptr[length] != 0)
            {
                length++;
            }
            return Encoding.UTF8.GetString(ptr, length);
        }
    }
}
