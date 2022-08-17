using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehiclePanel: MonoBehaviour
{

    public bool AreButtonsEnabled = true;
/*
    public Config.Config.VEHICLE_TYPE Type
    {
        set
        {
            if (_image != null)
            {
                var name = Enum.GetName(typeof(Config.Config.VEHICLE_TYPE), value);
                _image.sprite = Resources.Load<Sprite>(Path.Combine("Images", name));
                _name.text = name;
            }

            _type = value;
        }
        get
        {
            return _type;
        }
    }
    
    private Config.Config.VEHICLE_TYPE _type;

    [SerializeField] private TMP_Text _name;
    
    [SerializeField] private Image _image;

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
            if (GetComponentInParent<FleetPanel>() != null || GetComponentInParent<AssignmentPanel>() != null)
            {
                _countLabel.text = _count.ToString();
            }
            
            if (GetComponentInParent<EventPanel>() != null)
            {
                _countLabel.text = _count+"/"+_maxCount;
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
            
            if (GetComponentInParent<EventPanel>() != null )
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
        else
        {
            FindObjectOfType<PointsPanel>().Points--;
        }
    }
    
    private void RemoveVehicle()
    {
        if (Count > 0)
        {
            _fleetPanel.AddVehicle(Type);
            Count--;
        }
        else
        {
            FindObjectOfType<PointsPanel>().Points--;
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

    public void Initalize(Config.Config.VEHICLE_TYPE type, int count)
    {
        Type = type;
        if (GetComponentInParent<AssignedEventsPanel>())
        {
            MaxCount = count;
        }
        else
        {
            Count = count;
            _addButton.gameObject.SetActive(false);
            _removeButton.gameObject.SetActive(false);
        }
    }
    */
}