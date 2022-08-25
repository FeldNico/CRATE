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

    [field:SerializeField]
    public AudioClip EventFinished { private set; get; }
    
    [field:SerializeField]
    public AudioClip EventAssigned { private set; get; }
    
    [field:SerializeField]
    public AudioClip EventClosed { private set; get; }
    
    [field:SerializeField]
    public AudioClip VehicleClick { private set; get; }
    
    [field:SerializeField]
    public AudioClip PieceSwap { private set; get; }
    
    [field:SerializeField]
    public AudioClip PuzzleSolved { private set; get; }
    
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
        if (!IsTest)
        {
            StartCoroutine(WaitForEnd());
            IEnumerator WaitForEnd()
            {
                yield return new WaitForSeconds(Duration);
                FindObjectOfType<LogManager>().DownloadFiles();
            }
        }
        else
        {
            var finishedEvents = 0;
            var finishedPuzzles = 0;
            SlidingPuzzle.OnNewPuzzle += _ =>
            {
                finishedPuzzles++;
                if (finishedPuzzles > 1 && finishedEvents > 2)
                {
                    FindObjectOfType<LogManager>().DownloadFiles();
                }
            };
            AssignmentType.OnEventEnd += _ =>
            {
                finishedEvents++;
                if (finishedPuzzles > 1 && finishedEvents > 2)
                {
                    FindObjectOfType<LogManager>().DownloadFiles();
                }
            };
        }
    }

    public void PlaySound(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
