using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Config
{
    
    [CreateAssetMenu(fileName = "LanguageDictionary", menuName = "CRATE/LanguageDictionary", order = 1)]
    public class LanguageDictionary : ScriptableObject
    {
        [Serializable]
        public enum Language
        {
            EN,
            DE
        }
        
        [Serializable]
        public struct LanguageEntry
        {
            [SerializeField] internal string key;
            [SerializeField]
            internal List<TranslationEntry> translations;
        }

        [Serializable]
        public struct TranslationEntry
        {
            [SerializeField]
            internal Language lang;
            [SerializeField]
            internal string value;
        }
        
        public Language CurrentLanguage = Language.EN;
        
        [SerializeField]
        private List<LanguageEntry> entries;

        public string Translate(string key, Language language)
        {
            if (entries.Any(entry => entry.key.Equals(key)))
            {
                var entry = entries.First(entry => entry.key.Equals(key));
                foreach (var translation in entry.translations)
                {
                    if (translation.lang == language)
                    {
                        return translation.value;
                    }
                }
            }
            
            return key;
        }
        
        public string Translate(string key)
        {
            return Translate(key, CurrentLanguage);
        }
    }
}
