using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSitePhasePanel : MonoBehaviour
{
    public enum BuildSiteType
    {
        Construction,
        Roadwork,
        Demolition
    }
    
    [SerializeField] private GameObject VehiclePrefab;
    
    [SerializeField] private TMP_Text _nameLabel;
    
    [SerializeField] private Slider _progressBar;

    [SerializeField] private RectTransform _content;

    private BuildSiteType _type;

    public BuildSiteType Type => _type;

    public void Initalize(BuildSiteType type)
    {
        _type = type;
        _nameLabel.text = Enum.GetName(typeof(BuildSiteType), type);
        foreach (var (vehicleType, count) in getVehiclesByType(type))
        {
            var vehicle = Instantiate(VehiclePrefab).GetComponent<VehiclePanel>();
            vehicle.Type = vehicleType;
            vehicle.SetCount(-1*count);
            vehicle.SetName(Enum.GetName(typeof(VehiclePanel.VEHICLE_TYPE),vehicleType));
            vehicle.transform.SetParent(_content, false);
        }
    }

    private Dictionary<VehiclePanel.VEHICLE_TYPE,int> getVehiclesByType(BuildSiteType type)
    {
        Dictionary<VehiclePanel.VEHICLE_TYPE, int> dict = new Dictionary<VehiclePanel.VEHICLE_TYPE, int>();

        switch (type)
        {
            case BuildSiteType.Roadwork:
            {
                dict[VehiclePanel.VEHICLE_TYPE.Truck] = 3;
                dict[VehiclePanel.VEHICLE_TYPE.Roller] = 2;
                break;
            }
            case BuildSiteType.Demolition:
            {
                dict[VehiclePanel.VEHICLE_TYPE.DemolitionCrane] = 1;
                dict[VehiclePanel.VEHICLE_TYPE.Truck] = 4;
                break;
            }
            case BuildSiteType.Construction:
            {
                dict[VehiclePanel.VEHICLE_TYPE.Excavator] = 2;
                dict[VehiclePanel.VEHICLE_TYPE.Truck] = 2;
                break;
            }
        }

        return dict;
    }
    
}
