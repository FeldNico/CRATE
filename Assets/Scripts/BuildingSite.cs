using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BuildingSite : MonoBehaviour
{
    
    public static UnityAction<BuildingSite> OnConstructionFinished;

    [SerializeField] private GameObject _phasePrefab;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _daysLabel;
    [SerializeField] private RectTransform _content;
    
    
    private Dictionary<Config.Config.VEHICLE_TYPE, int> _vehicleDict = new();
    private Config.Config.BuildingSiteStruct _struct;
    private TimeManager _timeManager;
    private float _startDay;

    public void Instantiate(Config.Config.BuildingSiteStruct buildingSiteStruct)
    {
        _struct = buildingSiteStruct;
        _nameLabel.text = Enum.GetName(typeof(Config.Config.BuildingSiteCategory),buildingSiteStruct.Category);
        _timeManager = FindObjectOfType<TimeManager>();
        _startDay = _timeManager.Day;
        (_daysLabel as TextMeshProUGUI).faceColor = Color.red;
        foreach (var buildingPhaseStruct in buildingSiteStruct.Phases)
        {
            var vehiclePanel = Instantiate(_phasePrefab).GetComponent<BuildingSitePhasePanel>();
            vehiclePanel.transform.SetParent(_content, false);
            vehiclePanel.Initialize(buildingPhaseStruct);
        }
    }

    private void Update()
    {
        var daysLeft = _struct.Phases.Count * 5 - (_timeManager.Day - _startDay);
        if (_content.childCount == 0 || daysLeft <= 0)
        {
            if (daysLeft <= 0)
            {
                FindObjectOfType<PointsPanel>().Points -= _struct.Phases.Count * 5;
            }
            Destroy(gameObject);
            FindObjectOfType<BuildinSitesPanel>().OnBuildingSiteDelete?.Invoke(_struct);
        }
        
        _daysLabel.text = daysLeft + " Days Left.";
    }

}
