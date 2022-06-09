using System;
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

    // Start is called before the first frame update
    void Start()
    {
        var conf = FindObjectOfType<MainManager>().Config.Conf;
        for (int i = 0; i < 3; i++)
        {
            var buildingSiteStruct = conf[Random.Range(0, conf.Count)];
            var assignment = Instantiate(_assignementPrefab, _content1, false).GetComponent<AssignmentPanel>();
            assignment.Initialize(buildingSiteStruct);
        }
        for (int i = 0; i < 3; i++)
        {
            var buildingSiteStruct = conf[Random.Range(0, conf.Count)];
            var assignment = Instantiate(_assignementPrefab, _content2, false).GetComponent<AssignmentPanel>();
            assignment.Initialize(buildingSiteStruct);
        }
    }
}