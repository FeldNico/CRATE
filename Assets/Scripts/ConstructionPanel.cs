using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour, IVehicleContainer
{

    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _hintLabel;
    [SerializeField] private TMP_Text _excavationsLabel;
    [SerializeField] private Slider _excavationSlider;
    [SerializeField] private TMP_Text _dirtLabel;
    [SerializeField] private Slider _dirtSlider;
    private ScrollRect _scrollRect;
    
    [SerializeField, Range(0,1f)]
    private float _excavations = 1f;

    public float Excavations
    {
        set
        {
            value = Math.Clamp(value, 0, 1f);
            _excavations = value;
            _excavationSlider.value = value;
        }
        get => _excavations;
    }
    
    [SerializeField, Range(0,1f)]
    public float _dirt = 0f;
    
    public float Dirt
    {
        set
        {
            value = Math.Clamp(value, 0, 1f);
            _dirt = value;
            _dirtSlider.value = value;
        }
        get => _dirt;
    }
    
    private List<VehiclePanel> _vehicles = new();
    
    private void Awake()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
        _nameLabel.text = MainManager.Translate("Construction");
        _hintLabel.text = MainManager.Translate("AddVehicles");
        _excavationsLabel.text = MainManager.Translate("Excavations");
        _dirtLabel.text = MainManager.Translate("Dirt");
        _excavationSlider.value = _excavations;
        _dirtSlider.value = _dirt;
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
    
    private void Update()
    {
        if (_excavations == 0 && _dirt == 0)
        {
            if (_vehicles.Count > 0)
            {
                _hintLabel.text = MainManager.Translate("RemoveVehicles");
            }
            else
            {
                Destroy(gameObject);
            }
            
            return;
        }

        if (_vehicles.Count > 0)
        {
            _hintLabel.text = "";
        }
        else
        {
            _hintLabel.text = MainManager.Translate("AddVehicles");
        }
        
        foreach (var vehicle in _vehicles)
        {
            if (vehicle.Fuel == 0 || vehicle.Status == 0)
            {
                continue;
            }
            
            switch (vehicle.Type)
            {
                case VehiclePanel.VEHICLE_TYPE.Excavator:
                {
                    if (_excavations > 0 && Math.Abs(_dirt - 1) > float.Epsilon)
                    {
                        Excavations -= 0.0001f * vehicle.Status;
                        Dirt += 0.0001f * vehicle.Status;
                        vehicle.Fuel -= 0.0001f;
                        vehicle.Status -= 0.0001f;
                    }
                    break;
                }
                case VehiclePanel.VEHICLE_TYPE.Truck:
                {
                    if (_dirt > 0)
                    {
                        Dirt -= 0.0001f * vehicle.Status;
                        vehicle.Fuel -= 0.0001f;
                        vehicle.Status -= 0.0001f;
                    }
                    break;
                }
            }
        }
    }
}
