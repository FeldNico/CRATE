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
    
    
    private Dictionary<VehiclePanel.VEHICLE_TYPE, int> _vehicleDict = new();

    private void Start()
    {
        _nameLabel.text = "Construction";

        var vehiclePanel = Instantiate(_phasePrefab).GetComponent<BuildingSitePhasePanel>();
        vehiclePanel.Initalize(BuildingSitePhasePanel.BuildSiteType.Construction);
        vehiclePanel.transform.SetParent(_content, false);
        vehiclePanel = Instantiate(_phasePrefab).GetComponent<BuildingSitePhasePanel>();
        vehiclePanel.Initalize(BuildingSitePhasePanel.BuildSiteType.Roadwork);
        vehiclePanel.transform.SetParent(_content, false);
    }

    public void AddVehicle(VehiclePanel.VEHICLE_TYPE type)
    {
        _vehicleDict[type]++;
    }

    public void RemoveVehicle(VehiclePanel.VEHICLE_TYPE type)
    {
        _vehicleDict[type]--;
    }
    
}
