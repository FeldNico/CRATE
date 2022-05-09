using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehiclePanel: MonoBehaviour
{
    public enum VEHICLE_TYPE{
        Roller,
        Excavator,
        Truck,
        DemolitionCrane,
    }
    
    public VEHICLE_TYPE Type;
    
    [SerializeField] private TMP_Text _name;

    [SerializeField] private TMP_Text _count;

    [SerializeField] private Button _addButton;
    
    [SerializeField] private Button _removeButton;

    private FleetPanel _fleetPanel;

    private void Awake()
    {
        _fleetPanel = FindObjectOfType<FleetPanel>();
    }

    private void AddVehicle()
    {
        if (_fleetPanel.RemoveVehicle(Type))
        {
            SetCount(GetCount()+1);
        }
    }
    
    private void RemoveVehicle()
    {
        if (GetCount() == 0)
        {
            return;
        }
        _fleetPanel.AddVehicle(Type);
        SetCount(GetCount()-1);
    }

    public void SetName(string name)
    {
        _name.text = name;
    }
    
    public void SetCount(int count)
    {
        _count.text = count.ToString();
    }

    public int GetCount()
    {
        return int.Parse(_count.text);
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
}