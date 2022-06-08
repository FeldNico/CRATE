using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AssignmentsPanel : MonoBehaviour
{
    [SerializeField] private GameObject _assignementPrefab;
    [SerializeField] private RectTransform _content;

    private List<RectTransform> assignements;

    // Start is called before the first frame update
    void Start()
    {
        var conf = FindObjectOfType<MainManager>().Config.Conf;
        for (int i = 0; i < 8; i++)
        {
            var buildingSiteStruct = conf[Random.Range(0, conf.Count)];
            var assignment = Instantiate(_assignementPrefab, _content, false);
            assignment.transform.Find("Label").GetComponent<TMP_Text>().text =
                Enum.GetName(typeof(Config.Config.BuildingSiteCategory), buildingSiteStruct.Category);
            var dayslabel = assignment.transform.Find("DaysLabel").GetComponent<TMP_Text>();
            dayslabel.text = (buildingSiteStruct.Phases.Count * 5f) + " Days Needed";
            assignment.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                var bsp = FindObjectOfType<BuildinSitesPanel>();
                var vehiclePanel = Instantiate(bsp.ConstructionPrefab).GetComponent<BuildingSite>();
                vehiclePanel.transform.SetParent(bsp.Content, false);
                vehiclePanel.Instantiate(buildingSiteStruct);
                Destroy(assignment.gameObject);
            });
        }
    }
}