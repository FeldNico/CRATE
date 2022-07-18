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

    private Dictionary<Config.Config.VEHICLE_TYPE, VehiclePanel> _vehicles = new();

    private void Start()
    {
        foreach (var fleetStruct in FindObjectOfType<MainManager>().Config.Fleet)
        {
            var vehiclePanel = Instantiate(VehiclePrefab,_content,false).GetComponent<VehiclePanel>();
            vehiclePanel.Initalize(fleetStruct.Type,fleetStruct.Count);
            _vehicles[fleetStruct.Type] = vehiclePanel;
        }
    }

    public bool RemoveVehicle(Config.Config.VEHICLE_TYPE type)
    {
        var count = GetVehicleCount(type);
        if (count == 0)
        {
            return false;
        }

        _vehicles[type].Count--;
        return true;
    }

    public void AddVehicle(Config.Config.VEHICLE_TYPE type)
    {
        _vehicles[type].Count++;
    }

    public int GetVehicleCount(Config.Config.VEHICLE_TYPE type)
    {
        return _vehicles[type].Count;
    }
}
