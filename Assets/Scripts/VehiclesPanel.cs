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
    private Fuelstation _fuelstation;

    private List<VehiclePanel> _vehicles = new();

    private void Awake()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
        _fuelstation = FindObjectOfType<Fuelstation>();
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            var go = Instantiate(ExcavatorPrefab);
            go.transform.SetParent(_scrollRect.content.transform,false);
            go = Instantiate(TruckPrefab);
            go.transform.SetParent(_scrollRect.content.transform,false);
            go = Instantiate(TruckPrefab);
            go.transform.SetParent(_scrollRect.content.transform,false);
        }
    }

    private void Update()
    {
        foreach (var vehicle in _vehicles)
        {
            if (_fuelstation.Fuel > 0 && vehicle.Fuel < 1f)
            {
                vehicle.Fuel += 0.0002f;
                _fuelstation.Fuel -= 0.0002f * 0.1f;
            }
            vehicle.Status += 0.0002f;
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
