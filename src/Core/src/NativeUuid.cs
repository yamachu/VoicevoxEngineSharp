using System;
using System.Text;

namespace VoicevoxEngineSharp.Core
{
    internal static class NativeUuid
    {
        static readonly char[] hexChars = "0123456789abcdef".ToCharArray();

        internal static unsafe string ToUUIDv4(byte* ptr)
        {
            var length = 0;
            var uuid = new char[32];

            while (length < 16)
            {
                var x = ptr[length];
                var even = x >> 4;
                var odds = x & 0x0f;
                uuid[length * 2] = hexChars[even];
                uuid[length * 2 + 1] = hexChars[odds];
                length++;
            }

            var uuidString = string.Concat(uuid);
            return uuidString.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
        }

        internal static byte[] ToUUIDv4ByteArray(string uuid)
        {
            var trimmedUuid = uuid.Replace("-", "");
            var uuidBytes = new byte[16];
            var length = 0;

            while (length < 16)
            {
                var even = trimmedUuid[length * 2];
                var odds = trimmedUuid[length * 2 + 1];
                var evenIndex = Array.IndexOf(hexChars, even);
                var oddsIndex = Array.IndexOf(hexChars, odds);
                uuidBytes[length] = (byte)((evenIndex << 4) | oddsIndex);
                length++;
            }

            return uuidBytes;
        }
    }
}
