using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using TMPro;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    [SerializeField]
    private LanguageDictionary _dictionary;
    
    private Dictionary<TMP_Text, (LanguageDictionary.Language, string,string)> _cachedTexts = new();

    private void Awake()
    {
        if (_dictionary == null)
        {
            _dictionary = Resources.Load<LanguageDictionary>("Config/LanguageDictionary");
        }
    }

    public void ChangeLanguage(LanguageDictionary.Language language)
    {
        _dictionary.CurrentLanguage = language;
    }
    
    private void Update()
    {
        foreach (var text in FindObjectsOfType<TMP_Text>())
        {
            if (!_cachedTexts.ContainsKey(text) || _cachedTexts[text].Item1 != _dictionary.CurrentLanguage || _cachedTexts[text].Item3 != _dictionary.Translate(_cachedTexts[text].Item2) )
            {
                var key = _cachedTexts.ContainsKey(text) ? _cachedTexts[text].Item2 : text.text;
                text.text = _dictionary.Translate(key);
                _cachedTexts[text] = (_dictionary.CurrentLanguage, key,text.text);
            }
        }
    }
}
