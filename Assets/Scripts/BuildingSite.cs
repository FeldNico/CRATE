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

    public enum BuildSiteType
    {
        Construction,
        Roadwork,
        Demolition
    }
    
    public static UnityAction<BuildingSite> OnConstructionFinished;
    
    [SerializeField] private GameObject _vehiclePrefab;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _hintLabel;
    [SerializeField] private RectTransform _content;
    
    

    private Dictionary<VehiclePanel.VEHICLE_TYPE, int> _vehicleDict = new();

    private void Start()
    {
        _nameLabel.text = "Construction";
        _hintLabel.text = "AddVehicles";
        
        foreach (var vehicleType in Enum.GetValues(typeof(VehiclePanel.VEHICLE_TYPE)).Cast<VehiclePanel.VEHICLE_TYPE>())
        {
            var vehicleTypeName = Enum.GetName(typeof(VehiclePanel.VEHICLE_TYPE), vehicleType);
            var vehiclePanel = Instantiate(_vehiclePrefab).GetComponent<VehiclePanel>();
            vehiclePanel.SetCount(0);
            vehiclePanel.SetName(vehicleTypeName);
            vehiclePanel.Type = vehicleType;
            vehiclePanel.transform.SetParent(_content, false);
        }
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
