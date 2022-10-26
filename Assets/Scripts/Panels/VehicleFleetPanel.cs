using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleFleetPanel: MonoBehaviour
{
    public VehicleType Type { private set; get; }

    private List<Vehicle> _vehicles = new List<Vehicle>();

    [SerializeField] private TMP_Text _name;
    
    [SerializeField] private Image _image;

    [SerializeField] private TMP_Text _countLabel;
    
    [SerializeField] private TMP_Text _bonusCountLabel;

    public void Initialize(VehicleType type,int count)
    {
        name = type.VehicleName;
        Type = type;
        _name.text = type.VehicleName;
        _image.sprite = type.VehicleImage;
        _bonusCountLabel.faceColor = new Color(0,0.8f,0,1);
        for (int i = 0; i < count; i++)
        {
            _vehicles.Add(new Vehicle(type,false));
        }
    }

    public Vehicle RequestVehicle()
    {
        var vehicle = _vehicles.OrderBy(v => !v.IsBonus).FirstOrDefault();
        if (vehicle != null)
        {
            _vehicles.Remove(vehicle);
        }
        return vehicle;
    }

    public void ReturnVehicle(Vehicle vehicle)
    {
        _vehicles.Add(vehicle);
    }
    
    private void Update()
    {
        _countLabel.text = _vehicles.Count(vehicle => !vehicle.IsBonus).ToString();
        
        if (_vehicles.Count(vehicle => !vehicle.IsBonus) == 0)
        {
            _countLabel.faceColor = Color.red;
        }
        else if (_vehicles.Count(vehicle => !vehicle.IsBonus) == 1)
        {
            _countLabel.faceColor = new Color(1f,0.533f,0f);
        }
        else
        {
            _countLabel.faceColor = Color.black;
        }
        var bonusCount = _vehicles.Count(vehicle => vehicle.IsBonus);
        _bonusCountLabel.text = bonusCount == 0 ? "" : "+"+bonusCount;
    }
}