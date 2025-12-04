// @ts-expect-error dotnet.d.ts が取得できるようになったらそれを参照して出来るようにする
import { dotnet } from "./_framework/dotnet.js";

import {
  getTypedAssemblyExports,
  setTypedModuleImports,
} from "dotnet-webassembly-type-helper";

type Exported = {
  VoicevoxEngineSharp: {
    WasmWeb: {
      SynthesisExports: {
        Initialize: (openJTalkDictPath: string) => void;
        CreateAccentPhrases: (
          text: string,
          speakerId: number
        ) => Promise<string>;
        AudioQuery: (text: string, speakerId: number) => Promise<string>;
        ReplaceMoraData: (
          accentPhrasesJson: string,
          speakerId: number
        ) => Promise<string>;
        ReplaceMoraLength: (
          accentPhrasesJson: string,
          speakerId: number
        ) => Promise<string>;
        ReplaceMoraPitch: (
          accentPhrasesJson: string,
          speakerId: number
        ) => Promise<string>;
        SynthesisWave: (
          audioQueryJson: string,
          speakerId: number
        ) => Promise<number[]>;
      };
      IOHelper: {
        MountDictionaryAsync: (dictionaryData: ArrayLike<any>) => Promise<void>;
      };
    };
  };
};

type Imports = {
  yukarinSForward: (
    length: number,
    phonemeList: number[],
    speakerId: number[]
  ) => Promise<number[]>;
  yukarinSaForward: (
    length: number,
    vowelPhonemeList: number[],
    consonantPhonemeList: number[],
    startAccentList: number[],
    endAccentList: number[],
    startAccentPhraseList: number[],
    endAccentPhraseList: number[],
    speakerId: number[]
  ) => Promise<number[]>;
  decodeForward: (
    length: number,
    phonemeSize: number,
    f0: number[],
    phoneme: number[],
    speakerId: number[]
  ) => Promise<number[]>;
};

let dotnetExports: Exported | undefined = undefined;

export const initialize = async (
  imports: Imports,
  options?: { diagnosticTracing?: boolean },
): Promise<Exported> => {
  if (dotnetExports) {
    return dotnetExports;
  }

  const dotnetBuilder = options
    ? Object.entries(options).reduce((acc, curr) => {
        const [key, value] = curr;
        switch (key) {
          case "diagnosticTracing": {
            return value === undefined ? acc : acc.withDiagnosticTracing(value);
          }
          default: {
            return acc;
          }
        }
      }, dotnet)
    : dotnet;

  const { setModuleImports, getAssemblyExports, getConfig } =
    await dotnetBuilder.create();

  setTypedModuleImports(setModuleImports, "synthesis.js", {
    inference: {
      yukarinSForward: imports.yukarinSForward,
      yukarinSaForward: imports.yukarinSaForward,
      decodeForward: imports.decodeForward,
    },
  });
  setTypedModuleImports(setModuleImports, "helper.js", {
    helper: {
      // see: https://learn.microsoft.com/ja-jp/aspnet/core/client-side/dotnet-interop/?view=aspnetcore-10.0#type-mapping-limitations
      unwrapFloatArray: (jsObject) => jsObject,
      wrapFloatArray: (array) => array,
    },
  });

  const config = getConfig();
  const exports = await getTypedAssemblyExports(
    getAssemblyExports(config.mainAssemblyName)
  );
  dotnetExports = exports;

  return dotnetExports;
};
