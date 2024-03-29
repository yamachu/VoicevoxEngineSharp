﻿using NumSharp;

namespace VoicevoxEngineSharp.Core.Acoustic.Models
{
    internal class SamplingData
    {
        private NDArray array { get; init; }
        private float rate { get; init; }

        public SamplingData(NDArray array, float rate)
        {
            this.array = array;
            this.rate = rate;
        }

        public NDArray Resample(float resampleRate, int index = 0, int? length = null)
        {
            var _length = length;
            if (!length.HasValue)
            {
                _length = (int)(array.shape[0] / rate * resampleRate);
            }

            var indexes = (np.random.rand() + index + np.arange(_length.Value)) * (rate / resampleRate);
            // NOTE: Index外になることが稀にあるので、いい感じにしておく
            while (indexes.max().GetDouble() > array.shape[0])
            {
                indexes = indexes * 0.999;
            }
            return array[np.floor(indexes, NPTypeCode.Double).astype(NPTypeCode.Int32)];
        }
    }
}
