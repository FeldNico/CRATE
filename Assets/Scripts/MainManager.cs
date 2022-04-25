using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MainManager : MonoBehaviour
{
    public UnityAction OnExperimentStart;

    [SerializeField]
    private LanguageDictionary _languageDictionary;

    public LanguageDictionary LanguageDictionary => _languageDictionary;
    
    public void OnExperimentStartMessage()
    {
        OnExperimentStart?.Invoke();
    }

    public static string Translate(string key)
    {
        return FindObjectOfType<MainManager>()._languageDictionary.Translate(key);
    }
    
    public static string Translate(string key,LanguageDictionary.Language language)
    {
        return FindObjectOfType<MainManager>()._languageDictionary.Translate(key,language);
    }
    
}
