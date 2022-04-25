using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
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
        get
        {
            return _fuel;
        }
    }
    
    [SerializeField,Range(0f,1f)]
    private float _status = 1f;

    private MainManager _mainManager;
    private Canvas _canvas;
    
    public float Status
    {
        set
        {
            value = Math.Clamp(value, 0, 1f);
            _status = value;
            _statusSlider.value = value;
        }
        get
        {
            return _status;
        }
    }

    public void Awake()
    {
        _mainManager = FindObjectOfType<MainManager>();
        _canvas = FindObjectOfType<Canvas>();
        _nameLabel.text = MainManager.Translate(Enum.GetName(typeof(VEHICLE_TYPE), _type));
        _fuelLabel.text = MainManager.Translate("Fuel");
        _statusLabel.text = MainManager.Translate("Status");
    }

    public void Update()
    {
        Fuel -= 0.0001f;
        Status -= 0.0001f;
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
        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<ConstructionPanel>() || result.gameObject.GetComponent<VehiclesPanel>())
            {
                
                var rect = result.gameObject.GetComponentInChildren<ScrollRect>();
                transform.SetParent(rect.content,false);
                return;
            }
        }
        transform.parent = _prevParent;
        transform.position = _prevPos;
    }

    
    
}
