using System;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Enum
{
    /// <summary>
    /// ユーザー辞書の単語の種類。
    /// </summary>
    public enum UserDictWordType : int
    {
        /// <summary>
        /// 固有名詞。
        /// </summary>
        PROPER_NOUN = 0,
        /// <summary>
        /// 一般名詞。
        /// </summary>
        COMMON_NOUN = 1,
        /// <summary>
        /// 動詞。
        /// </summary>
        VERB = 2,
        /// <summary>
        /// 形容詞。
        /// </summary>
        ADJECTIVE = 3,
        /// <summary>
        /// 接尾辞。
        /// </summary>
        SUFFIX = 4,
    }

    internal static class UserDictWordTypeExt
    {
        public static UserDictWordType FromNative(this VoicevoxUserDictWordType wordType)
        {
            return wordType switch
            {
                VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_PROPER_NOUN => UserDictWordType.PROPER_NOUN,
                VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_COMMON_NOUN => UserDictWordType.COMMON_NOUN,
                VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_VERB => UserDictWordType.VERB,
                VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_ADJECTIVE => UserDictWordType.ADJECTIVE,
                VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_SUFFIX => UserDictWordType.SUFFIX,
                _ => throw new ArgumentOutOfRangeException(nameof(wordType), wordType, null),
            };
        }

        public static VoicevoxUserDictWordType ToNative(this UserDictWordType wordType)
        {
            return wordType switch
            {
                UserDictWordType.PROPER_NOUN => VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_PROPER_NOUN,
                UserDictWordType.COMMON_NOUN => VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_COMMON_NOUN,
                UserDictWordType.VERB => VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_VERB,
                UserDictWordType.ADJECTIVE => VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_ADJECTIVE,
                UserDictWordType.SUFFIX => VoicevoxUserDictWordType.VOICEVOX_USER_DICT_WORD_TYPE_SUFFIX,
                _ => throw new ArgumentOutOfRangeException(nameof(wordType), wordType, null),
            };
        }
    }
}
