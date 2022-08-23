using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    public UnityAction OnExperimentStart;
    
    public string SubjectID { private set; get; }
    
    private void Start()
    {
        #if UNITY_EDITOR
        OnExperimentStartMessage("11111");
        #endif
    }

    public void OnExperimentStartMessage(string sid)
    {
        SubjectID = sid;
        OnExperimentStart?.Invoke();
    }
}
