using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FleetPanel : MonoBehaviour
{

    [SerializeField] private GameObject _vehicleFleetPrefab;

    [SerializeField] private RectTransform _content;

    private void Awake()
    {
        FindObjectOfType<MainManager>().OnExperimentStart += () =>
        {
            var fleet = CrateConfig.Instance.GetFleet();
            foreach (var (type,count) in fleet)
            {
                var vehiclePanel = Instantiate(_vehicleFleetPrefab).GetComponent<VehicleFleetPanel>();
                vehiclePanel.Initialize(type,count);
                vehiclePanel.transform.SetParent(_content,false);
            }
        };
    }

    public Vehicle RequestVehicle(VehicleType type)
    {
        var panel = GetComponentsInChildren<VehicleFleetPanel>().FirstOrDefault(p => p.Type == type);
        if (panel == null)
        {
            return null;
        }
        return panel.RequestVehicle();
    }

    public void ReturnVehicle(Vehicle vehicle)
    {
        var panel = GetComponentsInChildren<VehicleFleetPanel>().FirstOrDefault(p => p.Type == vehicle.Type);
        if (panel != null)
        {
            panel.ReturnVehicle(vehicle);
        }
    }
    
    
}

