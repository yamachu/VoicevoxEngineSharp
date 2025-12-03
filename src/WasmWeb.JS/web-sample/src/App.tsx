import { initialize } from "@voicevoxenginesharp-wasm-web/core";
import {
  decodeForward,
  yukarinSForward,
  yukarinSaForward,
} from "@voicevoxenginesharp-wasm-web/inference";
import { InferenceSession } from "onnxruntime-web";
import { useState } from "react";

type SpeakerOnnxSessions = Map<number, InferenceSession>;
type ModelType = "yukarinS" | "yukarinSa" | "spectrogram" | "vocoder";

const sessions = {
  yukarinS: new Map(),
  yukarinSa: new Map(),
  spectrogram: new Map(),
  vocoder: new Map(),
} as const satisfies Record<ModelType, SpeakerOnnxSessions>;

const modelPaths = {
  /* eslint-disable @typescript-eslint/no-unused-vars */
  yukarinS: (_speakerId: number) => `./models/duration.onnx`,
  yukarinSa: (_speakerId: number) => `./models/intonation.onnx`,
  spectrogram: (_speakerId: number) => `./models/spectrogram.onnx`,
  vocoder: (_speakerId: number) => `./models/vocoder.onnx`,
  /* eslint-enable @typescript-eslint/no-unused-vars */
};

async function getSession(
  modelType: ModelType,
  speakerId: number
): Promise<InferenceSession> {
  const sessionMap = sessions[modelType];
  const maybeSession = sessionMap.get(speakerId);
  if (maybeSession) {
    return maybeSession;
  }

  const modelPath = modelPaths[modelType](speakerId);

  const session = await InferenceSession.create(modelPath, {
    executionProviders: ["webgpu", "wasm"],
  });
  sessionMap.set(speakerId, session);

  return session;
}

function sessionInjectedYukarinSForward(
  length: number,
  phonemeList: number[],
  speakerId: number[]
): Promise<number[]> {
  return getSession("yukarinS", speakerId[0]).then(async (session) =>
    yukarinSForward(session, length, phonemeList, speakerId)
  );
}

function sessionInjectedYukarinSaForward(
  length: number,
  vowelPhonemeList: number[],
  consonantPhonemeList: number[],
  startAccentList: number[],
  endAccentList: number[],
  startAccentPhraseList: number[],
  endAccentPhraseList: number[],
  speakerId: number[]
): Promise<number[]> {
  return getSession("yukarinSa", speakerId[0]).then(async (session) =>
    yukarinSaForward(
      session,
      length,
      vowelPhonemeList,
      consonantPhonemeList,
      startAccentList,
      endAccentList,
      startAccentPhraseList,
      endAccentPhraseList,
      speakerId
    )
  );
}

function sessionInjectedDecodeForward(
  length: number,
  phonemeSize: number,
  f0: number[],
  phoneme: number[],
  speakerId: number[]
): Promise<number[]> {
  return Promise.all([
    getSession("spectrogram", speakerId[0]),
    getSession("vocoder", speakerId[0]),
  ]).then(([spectrogramSession, vocoderSession]) =>
    decodeForward(
      spectrogramSession,
      vocoderSession,
      length,
      phonemeSize,
      f0,
      phoneme,
      speakerId
    )
  );
}

function App() {
  const [engine, setEngine] =
    useState<Awaited<ReturnType<typeof initialize>>>();
  const [dictionaryMounted, setDictionaryMounted] = useState(false);

  return (
    <>
      <button
        disabled={engine !== undefined}
        onClick={async () => {
          const engine = await initialize({
            decodeForward: sessionInjectedDecodeForward,
            yukarinSForward: sessionInjectedYukarinSForward,
            yukarinSaForward: sessionInjectedYukarinSaForward,
          });
          setEngine(engine);
        }}
      >
        Set ONNX Runtime Imports
      </button>
      <button
        disabled={engine === undefined || dictionaryMounted}
        onClick={async () => {
          if (!engine) return;
          const response = await fetch("./open_jtalk_dic_utf_8-1.11.tgz");
          const arrayBuffer = await response.arrayBuffer();
          const uint8Array = new Uint8Array(arrayBuffer);

          await engine.VoicevoxEngineSharp.WasmWeb.IOHelper.MountDictionaryAsync(
            uint8Array
          )
            .then(() => {
              console.log("✓ Mounted dictionary");
            })
            .then(() => {
              engine.VoicevoxEngineSharp.WasmWeb.SynthesisExports.Initialize(
                "/tmp/open_jtalk_dic_utf_8-1.11"
              );
              console.log("✓ Initialized synthesis exports");
            })
            .then(() => {
              setDictionaryMounted(true);
            });
        }}
      >
        Initialize
      </button>
      <textarea
        id="inputText"
        defaultValue="こんにちは"
        style={{ width: "100%", height: "100px" }}
      ></textarea>
      <button
        disabled={engine === undefined || !dictionaryMounted}
        onClick={async () => {
          if (!engine) return;
          const inputText = (
            document.getElementById("inputText") as HTMLTextAreaElement
          ).value;
          const audioQueryJson =
            await engine.VoicevoxEngineSharp.WasmWeb.SynthesisExports.AudioQuery(
              inputText,
              1
            );

          const waveData =
            await engine.VoicevoxEngineSharp.WasmWeb.SynthesisExports.SynthesisWave(
              audioQueryJson,
              1
            );

          // Float32Array to AudioBuffer and play
          const audioContext = new AudioContext();
          const audioBuffer = audioContext.createBuffer(
            1,
            waveData.length,
            24000
          );
          audioBuffer.getChannelData(0).set(waveData);
          const source = audioContext.createBufferSource();
          source.buffer = audioBuffer;
          source.connect(audioContext.destination);
          source.start();

          console.log("✓ Playing audio");
        }}
      >
        Synthesize
      </button>
    </>
  );
}

export default App;
