using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VehiclePanel : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public enum VEHICLE_TYPE{
        Excavator,
        Truck,
    }

    [SerializeField]
    private VEHICLE_TYPE _type;

    public VEHICLE_TYPE Type => _type;
    
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_Text _fuelLabel;
    [SerializeField] private Slider _fuelSlider;
    [SerializeField] private TMP_Text _statusLabel;
    [SerializeField] private Slider _statusSlider;

    [SerializeField,Range(0f,1f)]
    private float _fuel = 1f;

    public float Fuel
    {
        set
        {
            value = Math.Clamp(value, 0, 1f);
            _fuel = value;
            _fuelSlider.value = value;
        }
        get => _fuel;
    }
    
    [SerializeField,Range(0f,1f)]
    private float _status = 1f;

    private MainManager _mainManager;
    private Canvas _canvas;
    private IVehicleContainer _currentContainer;
    
    public float Status
    {
        set
        {
            value = Math.Clamp(value, 0, 1f);
            _status = value;
            _statusSlider.value = value;
        }
        get => _status;
    }

    private void Awake()
    {
        _mainManager = FindObjectOfType<MainManager>();
        _canvas = FindObjectOfType<Canvas>();
        _nameLabel.text = MainManager.Translate(Enum.GetName(typeof(VEHICLE_TYPE), _type));
        _fuelLabel.text = MainManager.Translate("Fuel");
        _statusLabel.text = MainManager.Translate("Status");
        _fuelSlider.value = Fuel;
        _statusSlider.value = Status;
    }

    private void Start()
    {
        _currentContainer = GetComponentInParent<IVehicleContainer>();
        _currentContainer.AddVehicle(this);
    }

    public void OnDrag(PointerEventData data)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(data.pointerEnter.transform as RectTransform, data.position, data.pressEventCamera, out globalMousePos))
        {
            transform.position = globalMousePos;
        }
    }

    private Transform _prevParent;
    private Vector3 _prevPos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        _prevParent = transform.parent;
        _prevPos = transform.position;
        transform.SetParent(_canvas.transform,false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        _canvas.GetComponent<GraphicRaycaster>().Raycast(eventData,results);
        var vehicleContainers = results.Where(result => result.gameObject.GetComponent<IVehicleContainer>() != null && result.gameObject.GetComponent<IVehicleContainer>() != _currentContainer).ToArray();
        if (vehicleContainers.Length > 0)
        {
            var container = vehicleContainers[0].gameObject.GetComponent<IVehicleContainer>();
            _currentContainer?.RemoveVehicle(this);
            _currentContainer = container;
            _currentContainer.AddVehicle(this);
        }
        else
        {
            transform.parent.SetParent(_prevParent);
            transform.position = _prevPos;
        }
    }
    
}
