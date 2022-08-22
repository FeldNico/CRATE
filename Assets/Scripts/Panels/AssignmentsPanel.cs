using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AssignmentsPanel : MonoBehaviour
{
    [SerializeField] private GameObject _assignementPanelPrefab;
    [SerializeField] private RectTransform _content1;
    [SerializeField] private RectTransform _content2;
    
    private TimeManager _timeManager;
    private MainManager _mainManager;
    private int _assignmentTypeIndex = 0; 

    private void Awake()
    {
        _timeManager = FindObjectOfType<TimeManager>();
        _mainManager = FindObjectOfType<MainManager>();
        _mainManager.OnExperimentStart += () =>
        {
            _assignmentTypeIndex = 0;
            for (int i = 0; i < 3; i++)
            {
                var assignmentPanel = Instantiate(_assignementPanelPrefab).GetComponent<AssignmentPanel>();
                assignmentPanel.Initialize(CrateConfig.Instance.AssignmentTypes[_assignmentTypeIndex++]);
                assignmentPanel.transform.SetParent(_content1,false);
            }
            for (int i = 0; i < 3; i++)
            {
                var assignmentPanel = Instantiate(_assignementPanelPrefab).GetComponent<AssignmentPanel>();
                assignmentPanel.Initialize(CrateConfig.Instance.AssignmentTypes[_assignmentTypeIndex++]);
                assignmentPanel.transform.SetParent(_content2,false);
            }
        };

        AssignmentType.OnEventEnd += _ =>
        {
            StartCoroutine(Wait());
            IEnumerator Wait()
            {
                yield return new WaitForSeconds(_timeManager.GetTimeUntilNextDay());
                var assignmentPanel = Instantiate(_assignementPanelPrefab).GetComponent<AssignmentPanel>();
                assignmentPanel.Initialize(CrateConfig.Instance.AssignmentTypes[_assignmentTypeIndex++]);
                if (_content1.childCount < _content2.childCount)
                {
                    assignmentPanel.transform.SetParent(_content1,false);
                }
                else
                {
                    assignmentPanel.transform.SetParent(_content2,false);
                }
            }
        };
    }
    
}