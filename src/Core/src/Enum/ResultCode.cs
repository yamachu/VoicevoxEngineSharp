using System;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Enum
{
    /// <summary>
    /// 処理結果を示す結果コード。
    /// </summary>
    public enum ResultCode : int
    {
        /// <summary>
        /// 成功
        /// </summary>
        RESULT_OK = 0,
        /// <summary>
        /// open_jtalk辞書ファイルが読み込まれていない
        /// </summary>
        RESULT_NOT_LOADED_OPENJTALK_DICT_ERROR = 1,
        /// <summary>
        /// modelの読み込みに失敗した
        /// </summary>
        RESULT_LOAD_MODEL_ERROR = 2,
        /// <summary>
        /// サポートされているデバイス情報取得に失敗した
        /// </summary>
        RESULT_GET_SUPPORTED_DEVICES_ERROR = 3,
        /// <summary>
        /// GPUモードがサポートされていない
        /// </summary>
        RESULT_GPU_SUPPORT_ERROR = 4,
        /// <summary>
        /// メタ情報読み込みに失敗した
        /// </summary>
        RESULT_LOAD_METAS_ERROR = 5,
        /// <summary>
        /// 無効なstyle_idが指定された
        /// </summary>
        RESULT_INVALID_STYLE_ID_ERROR = 6,
        /// <summary>
        /// 無効なmodel_idが指定された
        /// </summary>
        RESULT_INVALID_MODEL_ID_ERROR = 7,
        /// <summary>
        /// 推論に失敗した
        /// </summary>
        RESULT_INFERENCE_ERROR = 8,
        /// <summary>
        /// コンテキストラベル出力に失敗した
        /// </summary>
        RESULT_EXTRACT_FULL_CONTEXT_LABEL_ERROR = 11,
        /// <summary>
        /// 無効なutf8文字列が入力された
        /// </summary>
        RESULT_INVALID_UTF8_INPUT_ERROR = 12,
        /// <summary>
        /// AquesTalk風記法のテキストの解析に失敗した
        /// </summary>
        RESULT_PARSE_KANA_ERROR = 13,
        /// <summary>
        /// 無効なAudioQuery
        /// </summary>
        RESULT_INVALID_AUDIO_QUERY_ERROR = 14,
        /// <summary>
        /// 無効なAccentPhrase
        /// </summary>
        RESULT_INVALID_ACCENT_PHRASE_ERROR = 15,
        /// <summary>
        /// ファイルオープンエラー
        /// </summary>
        OPEN_FILE_ERROR = 16,
        /// <summary>
        /// Modelを読み込めなかった
        /// </summary>
        VVM_MODEL_READ_ERROR = 17,
        /// <summary>
        /// すでに読み込まれているModelを読み込もうとした
        /// </summary>
        ALREADY_LOADED_MODEL_ERROR = 18,
        /// <summary>
        /// Modelが読み込まれていない
        /// </summary>
        UNLOADED_MODEL_ERROR = 19,
        /// <summary>
        /// ユーザー辞書を読み込めなかった
        /// </summary>
        LOAD_USER_DICT_ERROR = 20,
        /// <summary>
        /// ユーザー辞書を書き込めなかった
        /// </summary>
        SAVE_USER_DICT_ERROR = 21,
        /// <summary>
        /// ユーザー辞書に単語が見つからなかった
        /// </summary>
        UNKNOWN_USER_DICT_WORD_ERROR = 22,
        /// <summary>
        /// OpenJTalkのユーザー辞書の設定に失敗した
        /// </summary>
        USE_USER_DICT_ERROR = 23,
        /// <summary>
        /// ユーザー辞書の単語のバリデーションに失敗した
        /// </summary>
        INVALID_USER_DICT_WORD_ERROR = 24,
        /// <summary>
        /// UUIDの変換に失敗した
        /// </summary>
        RESULT_INVALID_UUID_ERROR = 25,
    }

    internal static class ResultCodeExt
    {
        public static ResultCode FromNative(this VoicevoxResultCode code)
        {
            return code switch
            {
                VoicevoxResultCode.VOICEVOX_RESULT_OK => ResultCode.RESULT_OK,
                VoicevoxResultCode.VOICEVOX_RESULT_NOT_LOADED_OPENJTALK_DICT_ERROR => ResultCode.RESULT_NOT_LOADED_OPENJTALK_DICT_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_LOAD_MODEL_ERROR => ResultCode.RESULT_LOAD_MODEL_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_GET_SUPPORTED_DEVICES_ERROR => ResultCode.RESULT_GET_SUPPORTED_DEVICES_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_GPU_SUPPORT_ERROR => ResultCode.RESULT_GPU_SUPPORT_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_LOAD_METAS_ERROR => ResultCode.RESULT_LOAD_METAS_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_INVALID_STYLE_ID_ERROR => ResultCode.RESULT_INVALID_STYLE_ID_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_INVALID_MODEL_ID_ERROR => ResultCode.RESULT_INVALID_MODEL_ID_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_INFERENCE_ERROR => ResultCode.RESULT_INFERENCE_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_EXTRACT_FULL_CONTEXT_LABEL_ERROR => ResultCode.RESULT_EXTRACT_FULL_CONTEXT_LABEL_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_INVALID_UTF8_INPUT_ERROR => ResultCode.RESULT_INVALID_UTF8_INPUT_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_PARSE_KANA_ERROR => ResultCode.RESULT_PARSE_KANA_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_INVALID_AUDIO_QUERY_ERROR => ResultCode.RESULT_INVALID_AUDIO_QUERY_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_INVALID_ACCENT_PHRASE_ERROR => ResultCode.RESULT_INVALID_ACCENT_PHRASE_ERROR,
                VoicevoxResultCode.VOICEVOX_OPEN_FILE_ERROR => ResultCode.OPEN_FILE_ERROR,
                VoicevoxResultCode.VOICEVOX_VVM_MODEL_READ_ERROR => ResultCode.VVM_MODEL_READ_ERROR,
                VoicevoxResultCode.VOICEVOX_ALREADY_LOADED_MODEL_ERROR => ResultCode.ALREADY_LOADED_MODEL_ERROR,
                VoicevoxResultCode.VOICEVOX_UNLOADED_MODEL_ERROR => ResultCode.UNLOADED_MODEL_ERROR,
                VoicevoxResultCode.VOICEVOX_LOAD_USER_DICT_ERROR => ResultCode.LOAD_USER_DICT_ERROR,
                VoicevoxResultCode.VOICEVOX_SAVE_USER_DICT_ERROR => ResultCode.SAVE_USER_DICT_ERROR,
                VoicevoxResultCode.VOICEVOX_UNKNOWN_USER_DICT_WORD_ERROR => ResultCode.UNKNOWN_USER_DICT_WORD_ERROR,
                VoicevoxResultCode.VOICEVOX_USE_USER_DICT_ERROR => ResultCode.USE_USER_DICT_ERROR,
                VoicevoxResultCode.VOICEVOX_INVALID_USER_DICT_WORD_ERROR => ResultCode.INVALID_USER_DICT_WORD_ERROR,
                VoicevoxResultCode.VOICEVOX_RESULT_INVALID_UUID_ERROR => ResultCode.RESULT_INVALID_UUID_ERROR,
                _ => throw new ArgumentOutOfRangeException(nameof(code), code, null),
            };
        }

        public static VoicevoxResultCode ToNative(this ResultCode code)
        {
            return code switch
            {
                ResultCode.RESULT_OK => VoicevoxResultCode.VOICEVOX_RESULT_OK,
                ResultCode.RESULT_NOT_LOADED_OPENJTALK_DICT_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_NOT_LOADED_OPENJTALK_DICT_ERROR,
                ResultCode.RESULT_LOAD_MODEL_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_LOAD_MODEL_ERROR,
                ResultCode.RESULT_GET_SUPPORTED_DEVICES_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_GET_SUPPORTED_DEVICES_ERROR,
                ResultCode.RESULT_GPU_SUPPORT_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_GPU_SUPPORT_ERROR,
                ResultCode.RESULT_LOAD_METAS_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_LOAD_METAS_ERROR,
                ResultCode.RESULT_INVALID_STYLE_ID_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_INVALID_STYLE_ID_ERROR,
                ResultCode.RESULT_INVALID_MODEL_ID_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_INVALID_MODEL_ID_ERROR,
                ResultCode.RESULT_INFERENCE_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_INFERENCE_ERROR,
                ResultCode.RESULT_EXTRACT_FULL_CONTEXT_LABEL_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_EXTRACT_FULL_CONTEXT_LABEL_ERROR,
                ResultCode.RESULT_INVALID_UTF8_INPUT_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_INVALID_UTF8_INPUT_ERROR,
                ResultCode.RESULT_PARSE_KANA_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_PARSE_KANA_ERROR,
                ResultCode.RESULT_INVALID_AUDIO_QUERY_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_INVALID_AUDIO_QUERY_ERROR,
                ResultCode.RESULT_INVALID_ACCENT_PHRASE_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_INVALID_ACCENT_PHRASE_ERROR,
                ResultCode.OPEN_FILE_ERROR => VoicevoxResultCode.VOICEVOX_OPEN_FILE_ERROR,
                ResultCode.VVM_MODEL_READ_ERROR => VoicevoxResultCode.VOICEVOX_VVM_MODEL_READ_ERROR,
                ResultCode.ALREADY_LOADED_MODEL_ERROR => VoicevoxResultCode.VOICEVOX_ALREADY_LOADED_MODEL_ERROR,
                ResultCode.UNLOADED_MODEL_ERROR => VoicevoxResultCode.VOICEVOX_UNLOADED_MODEL_ERROR,
                ResultCode.LOAD_USER_DICT_ERROR => VoicevoxResultCode.VOICEVOX_LOAD_USER_DICT_ERROR,
                ResultCode.SAVE_USER_DICT_ERROR => VoicevoxResultCode.VOICEVOX_SAVE_USER_DICT_ERROR,
                ResultCode.UNKNOWN_USER_DICT_WORD_ERROR => VoicevoxResultCode.VOICEVOX_UNKNOWN_USER_DICT_WORD_ERROR,
                ResultCode.USE_USER_DICT_ERROR => VoicevoxResultCode.VOICEVOX_USE_USER_DICT_ERROR,
                ResultCode.INVALID_USER_DICT_WORD_ERROR => VoicevoxResultCode.VOICEVOX_INVALID_USER_DICT_WORD_ERROR,
                ResultCode.RESULT_INVALID_UUID_ERROR => VoicevoxResultCode.VOICEVOX_RESULT_INVALID_UUID_ERROR,
                _ => throw new ArgumentOutOfRangeException(nameof(code), code, null),
            };
        }
    }
}
