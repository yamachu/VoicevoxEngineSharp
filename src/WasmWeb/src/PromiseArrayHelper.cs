using System;
using System.Runtime.InteropServices.JavaScript;

namespace VoicevoxEngineSharp.WasmWeb
{
    public static partial class PromiseArrayHelper
    {
        [JSImport("helper.unwrapFloatArray", "helper.js")]
        [return: JSMarshalAs<JSType.Array<JSType.Number>>]
        public static partial double[] UnwrapFloatArray(JSObject jsObject);

        [JSImport("helper.wrapFloatArray", "helper.js")]
        public static partial JSObject WrapFloatArray([JSMarshalAs<JSType.Array<JSType.Number>>] double[] array);
    }
}
