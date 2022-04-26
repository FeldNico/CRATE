using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VehiclesPanel : MonoBehaviour, IVehicleContainer
{
    public GameObject ExcavatorPrefab;
    public GameObject TruckPrefab;
    
    private ScrollRect _scrollRect;

    private List<VehiclePanel> _vehicles = new();

    private void Awake()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            var go = Instantiate(ExcavatorPrefab);
            go.transform.SetParent(_scrollRect.content.transform,false);
            go = Instantiate(TruckPrefab);
            go.transform.SetParent(_scrollRect.content.transform,false);
        }
    }

    private void Update()
    {
        foreach (var vehicle in _vehicles)
        {
            vehicle.Fuel += 0.0001f;
            vehicle.Status += 0.0001f;
        }
    }

    public void AddVehicle(VehiclePanel vehicle)
    {
        _vehicles.Add(vehicle);
        vehicle.transform.SetParent(_scrollRect.content.transform,false);
    }

    public void RemoveVehicle(VehiclePanel vehicle)
    {
        if (_vehicles.Contains(vehicle))
        {
            _vehicles.Remove(vehicle);
        }
    }

    public List<VehiclePanel> GetVehicles()
    {
        return _vehicles;
    }
}
