import { type InferenceSession, Tensor } from "onnxruntime-common";

// Helper function to convert number array (or TypedArray) to BigInt64Array
function toBigInt64Array(arr: ArrayLike<number>): BigInt64Array {
  return BigInt64Array.from(Array.from(arr), (v) => BigInt(v));
}

/**
 * YukarinS forward pass for phoneme length prediction.
 * @param {InferenceSession} session - ONNX inference session
 * @param {number} length - Length of phoneme list
 * @param {number[]} phonemeList - Array of phoneme IDs
 * @param {number[]} speakerId - Speaker ID array (length=1)
 * @returns {Promise<number[]>} Array of phoneme lengths wrapped in object for JSInterop
 */
export async function yukarinSForward(
  session: InferenceSession,
  length: number,
  phonemeList: ArrayLike<number>,
  speakerId: ArrayLike<number>
): Promise<number[]> {
  // Create input tensors
  const phonemeListTensor = new Tensor("int64", toBigInt64Array(phonemeList), [
    length,
  ]);
  const speakerIdTensor = new Tensor("int64", toBigInt64Array(speakerId), [1]);

  // Run inference
  const feeds = {
    phoneme_list: phonemeListTensor,
    speaker_id: speakerIdTensor,
  };

  const results = await session.run(feeds);

  // Extract output
  const outputName = session.outputNames[0];
  const output = results[outputName].data;

  if (output instanceof Float32Array === false) {
    throw new Error("Unexpected output type from yukarinS model");
  }

  return Array.from(output);
}

/**
 * YukarinSa forward pass for F0/pitch prediction.
 * @param {InferenceSession} session - ONNX inference session
 * @param {number} length - Length of vowel phoneme list
 * @param {number[]} vowelPhonemeList - Array of vowel phoneme IDs
 * @param {number[]} consonantPhonemeList - Array of consonant phoneme IDs
 * @param {number[]} startAccentList - Start accent positions
 * @param {number[]} endAccentList - End accent positions
 * @param {number[]} startAccentPhraseList - Start accent phrase positions
 * @param {number[]} endAccentPhraseList - End accent phrase positions
 * @param {number[]} speakerId - Speaker ID array (length=1)
 * @returns {Promise<number[]>} Array of F0 values wrapped in object for JSInterop
 */
export async function yukarinSaForward(
  session: InferenceSession,
  length: number,
  vowelPhonemeList: ArrayLike<number>,
  consonantPhonemeList: ArrayLike<number>,
  startAccentList: ArrayLike<number>,
  endAccentList: ArrayLike<number>,
  startAccentPhraseList: ArrayLike<number>,
  endAccentPhraseList: ArrayLike<number>,
  speakerId: ArrayLike<number>
): Promise<number[]> {
  // Create input tensors
  const feeds = {
    length: new Tensor("int64", BigInt64Array.from([BigInt(length)]), [
      /* Scalar */
    ]),
    vowel_phoneme_list: new Tensor("int64", toBigInt64Array(vowelPhonemeList), [
      length,
    ]),
    consonant_phoneme_list: new Tensor(
      "int64",
      toBigInt64Array(consonantPhonemeList),
      [length]
    ),
    start_accent_list: new Tensor("int64", toBigInt64Array(startAccentList), [
      length,
    ]),
    end_accent_list: new Tensor("int64", toBigInt64Array(endAccentList), [
      length,
    ]),
    start_accent_phrase_list: new Tensor(
      "int64",
      toBigInt64Array(startAccentPhraseList),
      [length]
    ),
    end_accent_phrase_list: new Tensor(
      "int64",
      toBigInt64Array(endAccentPhraseList),
      [length]
    ),
    speaker_id: new Tensor("int64", toBigInt64Array(speakerId), [1]),
  };

  const results = await session.run(feeds);

  // Extract output
  const outputName = session.outputNames[0];
  const output = results[outputName].data;

  if (output instanceof Float32Array === false) {
    throw new Error("Unexpected output type from yukarinSa model");
  }

  return Array.from(output);
}

/**
 * Decode forward pass for waveform synthesis.
 * @param {InferenceSession} spectrogramSession - ONNX inference session
 * @param {InferenceSession} vocoderSession - ONNX inference session
 * @param {number} length - Number of frames
 * @param {number} phonemeSize - Phoneme size (number of phoneme types)
 * @param {number[]} f0 - F0 array
 * @param {number[]} phoneme - One-hot encoded phoneme array (length * phonemeSize)
 * @param {number[]} speakerId - Speaker ID array (length=1)
 * @returns {Promise<number[]>} Waveform data wrapped in object for JSInterop
 */
export async function decodeForward(
  spectrogramSession: InferenceSession,
  vocoderSession: InferenceSession,
  length: number,
  phonemeSize: number,
  f0: ArrayLike<number>,
  phoneme: ArrayLike<number>,
  speakerId: ArrayLike<number>
): Promise<number[]> {
  // Create input tensors
  const specFeeds = {
    f0: new Tensor("float32", Float32Array.from(f0), [f0.length, 1]),
    phoneme: new Tensor("float32", Float32Array.from(phoneme), [
      length,
      phonemeSize,
    ]),
    speaker_id: new Tensor("int64", toBigInt64Array(speakerId), [1]),
  };

  const specResult = await spectrogramSession.run(specFeeds);

  // Extract output
  const specOutputName = spectrogramSession.outputNames[0];
  const specOutput = specResult[specOutputName].data;

  if (specOutput instanceof Float32Array === false) {
    throw new Error("Unexpected output type from decode model");
  }

  // Vocoder inference
  const vocoderFeeds = {
    spec: new Tensor("float32", specOutput, [
      length,
      // feat_size, from SOSOA
      specOutput.length / length,
    ]),
  };

  const vocoderResult = await vocoderSession.run(vocoderFeeds);

  // Extract output
  const vocoderOutputName = vocoderSession.outputNames[0];
  const vocoderOutput = vocoderResult[vocoderOutputName].data;

  if (vocoderOutput instanceof Float32Array === false) {
    throw new Error("Unexpected output type from vocoder model");
  }

  return Array.from(vocoderOutput);
}
