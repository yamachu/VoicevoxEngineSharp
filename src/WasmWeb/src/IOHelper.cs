using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Tar;

namespace VoicevoxEngineSharp.WasmWeb
{
    public static partial class IOHelper
    {
        [JSExport]
        public static Task MountDictionaryAsync(byte[] dictionaryData /* expected to be a gzipped tarball, e.g. open_jtalk_dic_utf_8-1.11.tar.gz */)
        {
            using var memoryStream = new MemoryStream(dictionaryData);
            using var gzStream = new System.IO.Compression.GZipStream(memoryStream, CompressionMode.Decompress);
            using var tarArchive = TarArchive.CreateInputTarArchive(gzStream, Encoding.UTF8);
            tarArchive.ExtractContents(@"/tmp/");

            return Task.CompletedTask;
        }
    }
}
