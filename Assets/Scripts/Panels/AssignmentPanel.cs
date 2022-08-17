using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssignmentPanel : MonoBehaviour
{
    [SerializeField] private GameObject _vehiclePrefab;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _days;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Button _button;

    /*
    private Config.Config.EventStruct _info;
    private int _startDay = 0;

    public void Initialize(Config.Config.EventStruct info)
    {
        _info = info;
        _label.text = Enum.GetName(typeof(Config.Config.EVENT_CATEGORY), info.Category);
        _startDay = (int) FindObjectOfType<TimeManager>().Day;
        
        foreach (var vehicleStruct in info.Vehicles)
        {
            var vehicle = Instantiate(_vehiclePrefab).GetComponent<VehiclePanel>();
            (vehicle.transform as RectTransform).sizeDelta *= 0.6f;
            (vehicle.transform as RectTransform).localScale *= 0.6f;
            vehicle.transform.SetParent(_content,false);
            vehicle.Initalize(vehicleStruct.Type,vehicleStruct.Count);
        }

        _button.onClick.AddListener(() =>
        {
            var bsp = FindObjectOfType<AssignedEventsPanel>();
            var vehiclePanel = Instantiate(bsp.EventPrefab,bsp.Content,false).GetComponent<EventPanel>();
            vehiclePanel.Initialize(info,(int) (_info.DurationInDays * 3f) - ((int) FindObjectOfType<TimeManager>().Day - _startDay));
            Destroy(gameObject);
        });
    }

    private void Update()
    {
        var interactable = FindObjectOfType<AssignedEventsPanel>().Content.childCount < 3;
        _button.interactable = interactable;

        var deadline = (int) (_info.DurationInDays * 3f) - ((int) FindObjectOfType<TimeManager>().Day - _startDay);
        _days.text = "Dauer:\t "+_info.DurationInDays+" Tage.\nDeadline in:\t "+deadline+" Tagen.";
        if (deadline < 0)
        {
            FindObjectOfType<PointsPanel>().Points -= _info.DurationInDays * 5;
            Destroy(gameObject);
        }
    }
    */
}
