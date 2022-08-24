using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    public UnityAction OnExperimentStart;
    public bool IsTest { private set; get; }
    public int Duration { private set; get; }
    
    private void Start()
    {
        #if UNITY_EDITOR
        OnExperimentStartMessage("0;60");
        #endif
    }

    public void OnExperimentStartMessage(string config)
    {
        IsTest = config.Split(";")[0] != "0";
        Duration = int.Parse(config.Split(";")[1]);
        OnExperimentStart?.Invoke();
        StartCoroutine(WaitForEnd());
        IEnumerator WaitForEnd()
        {
            yield return new WaitForSeconds(Duration);
            FindObjectOfType<LogManager>().DownloadFiles();
        }
    }
}
