using System;
using System.Collections.Generic;
using System.IO;

namespace ArabizeCli
{
    public static class Defaults
    {
        public static readonly string applicationName = "Arabize CLI";
        public static readonly string configurationPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "arabize-cli");
        public static readonly string macrosFileName = "macros.json";
        public static readonly Dictionary<string, string> diacritics = new()
        {
            { ".@''", "\u0655\u064B" }, // hamza below fathatan
            { ".@__", "\u0655\u064D" }, // hamza below kasratan
            { ".@%%", "\u0655\u064C" }, // hamza below dammatan
            { ".@#", "\u0655\u0652" },  // hamza below sukun
            { ".@'", "\u0655\u064E" },  // hamza below fatha
            { ".@_", "\u0655\u0650" },  // hamza below kasra
            { ".@%", "\u0655\u064F" },  // hamza below damma
            { "@''", "\u0654\u064B" },  // hamza above fathatan
            { "@__", "\u0654\u064D" },  // hamza above kasratan
            { "@%%", "\u0654\u064C" },  // hamza above dammatan
            { ".@", "\u0655" },         // hamza below
            { "@#", "\u0654\u0652" },   // hamza above sukun
            { "@'", "\u0654\u064E" },   // hamza above fatha
            { "@_", "\u0654\u0650" },   // hamza above kasra
            { "@%", "\u0654\u064F" },   // hamza above damma
            { "''", "\u064B" },         // fathatan
            { "__", "\u064D" },         // kasratan
            { "%%", "\u064C" },         // dammatan
            { "$'", "\u0651\u064E" },   // shadda fatha
            { "$%", "\u0651\u064F" },   // shadda damma
            { "$_", "\u0651\u0650" },   // shadda kasra
            { "@", "\u0654" },          // hamza above
            { "~", "\u0653" },          // maddah above
            { "$", "\u0651" },          // shadda
            { "'", "\u064E" },          // fatha
            { "_", "\u0650" },          // kasra
            { "%", "\u064F" },          // damma
            { "#", "\u0652" }           // sukun
        };
        public static readonly Dictionary<string, string> letters = new()
        {
            { "alif", "ا" },        // Alif
            { "ba", "ب" },          // Ba
            { "ta", "ت" },          // Ta
            { "tha", "ث" },         // Tha
            { "jeem", "ج" },        // Jeem
            { "ha", "ح" },          // Ha
            { "kha", "خ" },         // Kha
            { "dal", "د" },         // Dal
            { "thal", "ذ" },        // Thal
            { "ra", "ر" },          // Ra
            { "zay", "ز" },         // Zay
            { "seen", "س" },        // Seen
            { "sheen", "ش" },       // Sheen
            { "sad", "ص" },         // Sad
            { "dad", "ض" },         // Dad
            { "tta", "ط" },         // Tta
            { "dha", "ظ" },         // Dha
            { "ayn", "ع" },         // Ayn
            { "ghayn", "غ" },       // Ghayn
            { "fa", "ف" },          // Fa
            { "qaf", "ق" },         // Qaf
            { "kaf", "ك" },         // Kaf
            { "lam", "ل" },         // Lam
            { "meem", "م" },        // Meem
            { "noon", "ن" },        // Noon
            { "haa", "ه" },         // Haa
            { "waw", "و" },         // Waw
            { "ya", "ي" },          // Ya
            { "hamza", "ء" },       // Hamza
            { "wasla", "\u0651" },  // Wasla (&#1649;)
            { "marbuta", "ة" },     // Ta Marbuta
            { "maksura", "ى" },     // Alif Maksura
            { "(?)", "◌" }          // Placeholder or unknown symbol
        };
    }
}