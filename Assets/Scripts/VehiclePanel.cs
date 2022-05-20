using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehiclePanel: MonoBehaviour
{

    public bool AreButtonsEnabled = true;
    
    public Config.Config.VEHICLE_TYPE Type;
    
    [SerializeField] private TMP_Text _name;

    [SerializeField] private TMP_Text _countLabel;

    [SerializeField] private Button _addButton;
    
    [SerializeField] private Button _removeButton;

    private int _count = 0;

    public int Count
    {
        get => _count;
        set
        {
            _count = Math.Clamp(value,0,_maxCount);
            if (GetComponentInParent<FleetPanel>() != null)
            {
                _countLabel.text = _count.ToString();
            }

            if (GetComponentInParent<BuildingSitePhasePanel>() != null)
            {
                _countLabel.text = _count.ToString()+"/"+_maxCount;
            }
        }
    }
    
    private int _maxCount = Int32.MaxValue;
    public int MaxCount
    {
        get => _maxCount;
        set
        {
            _maxCount = Math.Clamp(value,0,Int32.MaxValue);
            if (GetComponentInParent<BuildingSitePhasePanel>() != null)
            {
                _countLabel.text = _count.ToString()+"/"+_maxCount;
            }
        }
    }

    private FleetPanel _fleetPanel;

    private void Awake()
    {
        _fleetPanel = FindObjectOfType<FleetPanel>();
    }

    private void AddVehicle()
    {
        if (Count < MaxCount && _fleetPanel.RemoveVehicle(Type))
        {
            Count++;
        }
    }
    
    private void RemoveVehicle()
    {
        if (Count > 0)
        {
            _fleetPanel.AddVehicle(Type);
            Count--;
        }
    }
    
    public void DisableButtons()
    {
        AreButtonsEnabled = false;
        _addButton.interactable = false;
        _removeButton.interactable = false;
    }
    
    public void EnableButtons()
    {
        AreButtonsEnabled = true;
        _addButton.interactable = true;
        _removeButton.interactable = true;
    }
    
    public void SetName(string name)
    {
        _name.text = name;
    }

    private void OnEnable()
    {
        _addButton.onClick.AddListener(AddVehicle);
        _removeButton.onClick.AddListener(RemoveVehicle);
    }
    
    private void OnDisable()
    {
        _addButton.onClick.RemoveListener(AddVehicle);
        _removeButton.onClick.RemoveListener(RemoveVehicle);
    }

    public void Initalize(Config.Config.VehicleStruct vehicleStruct)
    {
        Type = vehicleStruct.Type;
        MaxCount = vehicleStruct.Count;
        SetName(Enum.GetName(typeof(Config.Config.VEHICLE_TYPE), Type));
    }
}