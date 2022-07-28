using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AssignmentsPanel : MonoBehaviour
{
    [SerializeField] private GameObject _assignementPrefab;
    [SerializeField] private RectTransform _content1;
    [SerializeField] private RectTransform _content2;

    private List<RectTransform> assignements;
    private TimeManager _timeManager;
    private MainManager _mainManager;
    private bool _doUpdate = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _timeManager = FindObjectOfType<TimeManager>();
        _mainManager = FindObjectOfType<MainManager>();
        _mainManager.OnExperimentStart += () =>
        {
            var conf = _mainManager.Config.Conf;
            for (int i = 0; i < 2; i++)
            {
                var buildingSiteStruct = conf[Random.Range(0, conf.Count)];
                var assignment = Instantiate(_assignementPrefab, _content1, false).GetComponent<AssignmentPanel>();
                assignment.Initialize(buildingSiteStruct);
            }

            for (int i = 0; i < 2; i++)
            {
                var buildingSiteStruct = conf[Random.Range(0, conf.Count)];
                var assignment = Instantiate(_assignementPrefab, _content2, false).GetComponent<AssignmentPanel>();
                assignment.Initialize(buildingSiteStruct);
            }

            _doUpdate = true;
        };
    }
    
    private void Update(){
        if (!_doUpdate)
        {
            return;
        }
        
        if (_content1.transform.childCount < 3)
        {
            var conf = FindObjectOfType<MainManager>().Config.Conf;
            var buildingSiteStruct = conf[Random.Range(0, conf.Count)];
            var assignment = Instantiate(_assignementPrefab, _content1, false).GetComponent<AssignmentPanel>();
            assignment.Initialize(buildingSiteStruct);
            assignment.gameObject.SetActive(false);
            StartCoroutine(WaitForContext1());
            IEnumerator WaitForContext1()
            {
                yield return new WaitForSeconds(_timeManager.GetTimeUntilNextDay() + _timeManager.DayDuration);
                assignment.gameObject.SetActive(true);
            }
        }
        if (_content2.transform.childCount < 3)
        {
            var conf = FindObjectOfType<MainManager>().Config.Conf;
            var buildingSiteStruct = conf[Random.Range(0, conf.Count)];
            var assignment = Instantiate(_assignementPrefab, _content2, false).GetComponent<AssignmentPanel>();
            assignment.Initialize(buildingSiteStruct);
            assignment.gameObject.SetActive(false);
            StartCoroutine(WaitForContext2());
            IEnumerator WaitForContext2()
            {
                yield return new WaitForSeconds(_timeManager.GetTimeUntilNextDay() + _timeManager.DayDuration);
                assignment.gameObject.SetActive(true);
            }
        }
    }
}