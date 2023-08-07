using System;
using System.Text;
using VoicevoxEngineSharp.Core.Enum;
using VoicevoxEngineSharp.Core.Native;

namespace VoicevoxEngineSharp.Core.Struct
{
    /// <summary>
    /// ユーザー辞書の単語。
    /// </summary>
    public struct UserDictWord
    {
        internal UserDictWord(string surface, string pronunciation)
        {
            Surface = surface;
            Pronunciation = pronunciation;
            AccentType = 0;
            WordType = UserDictWordType.PROPER_NOUN;
            Priority = 0;
        }

        /// <summary>
        /// 表記
        /// </summary>
        public string Surface { get; }
        /// <summary>
        /// 読み
        /// </summary>
        public string Pronunciation { get; }
        /// <summary>
        /// アクセント型
        /// </summary>
        public nuint AccentType { get; set; }
        /// <summary>
        /// 単語の種類
        /// </summary>
        public UserDictWordType WordType { get; set; }
        /// <summary>
        /// 優先度
        /// </summary>
        public uint Priority { get; set; }

        public static UserDictWord Create(string surface, string pronunciation)
        {
            unsafe
            {
                fixed (byte* ptrSurface = System.Text.Encoding.UTF8.GetBytes(surface))
                {
                    fixed (byte* ptrPronunciation = System.Text.Encoding.UTF8.GetBytes(pronunciation))
                    {
                        var unsafeUserDictWord = CoreUnsafe.voicevox_user_dict_word_make(ptrSurface, ptrPronunciation);
                        return unsafeUserDictWord.FromNative();
                    }
                }
            }
        }
    }

    internal static class UserDictWordExt
    {
        internal static UserDictWord FromNative(this VoicevoxUserDictWord userDictWord)
        {
            unsafe
            {
                return new UserDictWord(
                    StringConvertCompat.ToUTF8String(userDictWord.surface),
                    StringConvertCompat.ToUTF8String(userDictWord.pronunciation)
                )
                {
                    AccentType = userDictWord.accent_type,
                    WordType = userDictWord.word_type.FromNative(),
                    Priority = userDictWord.priority
                };
            }
        }

        internal static VoicevoxUserDictWord ToNative(this UserDictWord userDictWord)
        {
            unsafe
            {
                fixed (byte* ptrSurface = Encoding.UTF8.GetBytes(userDictWord.Surface))
                {
                    fixed (byte* ptrPronunciation = Encoding.UTF8.GetBytes(userDictWord.Pronunciation))
                    {
                        return new VoicevoxUserDictWord
                        {
                            surface = ptrSurface,
                            pronunciation = ptrPronunciation,
                            accent_type = userDictWord.AccentType,
                            word_type = userDictWord.WordType.ToNative(),
                            priority = userDictWord.Priority
                        };
                    }
                }
            }
        }
    }
}
