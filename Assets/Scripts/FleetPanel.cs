using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FleetPanel : MonoBehaviour
{

    [SerializeField]
    private GameObject VehiclePrefab;
    
    [SerializeField]
    private RectTransform _content;
    public RectTransform Content => _content;

    private Dictionary<VehiclePanel.VEHICLE_TYPE, VehiclePanel> _vehicles = new();

    private void Start()
    {
        foreach (var vehicleType in Enum.GetValues(typeof(VehiclePanel.VEHICLE_TYPE)).Cast<VehiclePanel.VEHICLE_TYPE>())
        {
            var vehicleTypeName = Enum.GetName(typeof(VehiclePanel.VEHICLE_TYPE), vehicleType);
            var vehiclePanel = Instantiate(VehiclePrefab).GetComponent<VehiclePanel>();
            vehiclePanel.SetCount(10);
            vehiclePanel.SetName(vehicleTypeName);
            vehiclePanel.Type = vehicleType;
            _vehicles[vehicleType] = vehiclePanel;
            vehiclePanel.transform.SetParent(_content, false);
            
            foreach (var button in vehiclePanel.GetComponentsInChildren<Button>())
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    public bool RemoveVehicle(VehiclePanel.VEHICLE_TYPE type)
    {
        var count = GetVehicleCount(type);
        if (count == 0)
        {
            return false;
        }
        
        _vehicles[type].SetCount(count-1);
        return true;
    }

    public void AddVehicle(VehiclePanel.VEHICLE_TYPE type)
    {
        _vehicles[type].SetCount(GetVehicleCount(type)+1);
    }

    public int GetVehicleCount(VehiclePanel.VEHICLE_TYPE type)
    {
        return _vehicles[type].GetCount();
    }
}
