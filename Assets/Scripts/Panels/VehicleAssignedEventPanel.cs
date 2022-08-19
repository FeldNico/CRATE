using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleAssignedEventPanel: MonoBehaviour
{
    public VehicleType Type { private set; get; }

    private List<Vehicle> _vehicles = new List<Vehicle>();
    
    public bool AreButtonsEnabled = true;

    [SerializeField] private TMP_Text _name;
    
    [SerializeField] private Image _image;

    [SerializeField] private TMP_Text _countLabel;

    [SerializeField] private Button _addButton;
    
    [SerializeField] private Button _removeButton;
    
    public bool IsSatisfied => _vehicles.Count == _maxCount;
    public bool IsEmpty => _vehicles.Count == 0;

    private int _maxCount = 0;
    private FleetPanel _fleetPanel;

    private void Awake()
    {
        _fleetPanel = FindObjectOfType<FleetPanel>();
        _addButton.onClick.AddListener(() =>
        {
            if (IsSatisfied)
            {
                FindObjectOfType<PointsPanel>().Points -= 1;
                return;
            }
            var vehicle = _fleetPanel.RequestVehicle(Type);
            if (vehicle != null)
            {
                _vehicles.Add(vehicle);
            }
            else
            {
                FindObjectOfType<PointsPanel>().Points -= 1;
            }
        });
        _removeButton.onClick.AddListener(() =>
        {
            var vehicle = _vehicles.OrderBy(v => !v.IsBonus).FirstOrDefault();
            if (vehicle != null)
            {
                _vehicles.Remove(vehicle);
                _fleetPanel.ReturnVehicle(vehicle);
            }
        });
    }

    public void Initialize(VehicleType type, int count)
    {
        Type = type;
        _maxCount = count;
        _name.text = type.VehicleName;
        _image.sprite = type.VehicleImage;
    }

    private void Update()
    {
        _addButton.interactable = AreButtonsEnabled;
        _removeButton.interactable = AreButtonsEnabled && !IsEmpty;
        _countLabel.text = _vehicles.Count + "/" + _maxCount;
    }

    private void OnDestroy()
    {
        foreach (var vehicle in _vehicles.Where(vehicle => !vehicle.IsBonus))
        {
            _fleetPanel.ReturnVehicle(vehicle);
        }
    }
}