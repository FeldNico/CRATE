using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingSite : MonoBehaviour
{
    
    public static UnityAction<BuildingSite> OnConstructionFinished;
    
    [SerializeField] private GameObject _phasePrefab;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private RectTransform _content;
    
    
    private Dictionary<Config.Config.VEHICLE_TYPE, int> _vehicleDict = new();
    private Config.Config.BuildingSiteStruct _struct;

    public void Instantiate(Config.Config.BuildingSiteStruct buildingSiteStruct)
    {
        _struct = buildingSiteStruct;
        _nameLabel.text = Enum.GetName(typeof(Config.Config.BuildingSiteCategory),buildingSiteStruct.Category);
        foreach (var buildingPhaseStruct in buildingSiteStruct.Phases)
        {
            var vehiclePanel = Instantiate(_phasePrefab).GetComponent<BuildingSitePhasePanel>();
            vehiclePanel.transform.SetParent(_content, false);
            vehiclePanel.Initalize(buildingPhaseStruct);
        }
    }

    private void Update()
    {
        if (_content.childCount == 0)
        {
            Destroy(gameObject);
            FindObjectOfType<BuildinSitesPanel>().OnBuildingSiteDelete?.Invoke(_struct);
        }
    }

    public void AddVehicle(Config.Config.VEHICLE_TYPE type)
    {
        _vehicleDict[type]++;
    }

    public void RemoveVehicle(Config.Config.VEHICLE_TYPE type)
    {
        _vehicleDict[type]--;
    }
    
    
    
}
