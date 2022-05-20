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

    private Dictionary<Config.Config.VEHICLE_TYPE, VehiclePanel> _vehicles = new();

    private void Start()
    {
        foreach (var fleetStruct in FindObjectOfType<MainManager>().Config.Fleet)
        {
            var vehicleTypeName = Enum.GetName(typeof(Config.Config.VEHICLE_TYPE), fleetStruct.Type);
            var vehiclePanel = Instantiate(VehiclePrefab).GetComponent<VehiclePanel>();
            vehiclePanel.transform.SetParent(_content, false);
            vehiclePanel.Count = fleetStruct.Count;
            vehiclePanel.SetName(vehicleTypeName);
            vehiclePanel.Type = fleetStruct.Type;
            _vehicles[fleetStruct.Type] = vehiclePanel;
            foreach (var button in vehiclePanel.GetComponentsInChildren<Button>())
            {
                button.gameObject.SetActive(false);
            }
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
    
    private int GetVehicleCountByType(Config.Config.VEHICLE_TYPE type)
    {
        switch (type)
        {
            case Config.Config.VEHICLE_TYPE.Excavator:
            {
                return 4;
            }
            case Config.Config.VEHICLE_TYPE.Roller:
            {
                return 3;
            }
            case Config.Config.VEHICLE_TYPE.Truck:
            {
                return 5;
            }
            case Config.Config.VEHICLE_TYPE.DemolitionCrane:
            {
                return 1;
            }
        }

        return 0;
    }
}
