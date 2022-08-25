using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AssignmentPanel : MonoBehaviour
{
    
    [SerializeField] private GameObject _vehicleAssignmentPanelPrefab;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _daysLabel;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Button _button;

    private AssignmentType _assignmentType;
    private int _startDay = 0;
    private MainManager _mainManager;

    public void Initialize(AssignmentType assignmentType)
    {
        _mainManager = FindObjectOfType<MainManager>();
        name = assignmentType.Name;
        AssignmentType.OnNewAssignmentGenerated?.Invoke(assignmentType);
        _assignmentType = assignmentType;
        _label.text = assignmentType.Name;
        _startDay = (int) FindObjectOfType<TimeManager>().Day;
        
        foreach (var (vehicleType, count) in assignmentType.VehiclesPerDay)
        {
            var vehicle = Instantiate(_vehicleAssignmentPanelPrefab).GetComponent<VehicleAssignmentPanel>();
            (vehicle.transform as RectTransform).sizeDelta *= 0.6f;
            (vehicle.transform as RectTransform).localScale *= 0.6f;
            vehicle.transform.SetParent(_content,false);
            vehicle.Initialize(vehicleType,count);
        }

        _button.onClick.AddListener(() =>
        {
            _mainManager.PlaySound(_mainManager.EventFinished);
            var deadline = (int) (_assignmentType.Days * 3f) - ((int) FindObjectOfType<TimeManager>().Day - _startDay);
            FindObjectOfType<AssignedEventsPanel>().AddEvent(_assignmentType,deadline);
            AssignmentType.OnNewEventAssignment?.Invoke(_assignmentType,deadline);
            Destroy(gameObject);
        });
    }

    private void Update()
    {
        var interactable = FindObjectOfType<AssignedEventsPanel>().Content.childCount < 3;
        _button.interactable = interactable;

        var deadline = (int) (_assignmentType.Days * 3f) - ((int) FindObjectOfType<TimeManager>().Day - _startDay);
        _daysLabel.text = "Dauer:\t "+_assignmentType.Days+" Tage\nDeadline in:\t "+deadline+" Tagen\nPunkte:\t "+_assignmentType.Difficulty+" Punkte";
        if (deadline < 0)
        {
            FindObjectOfType<PointsPanel>().Points -= _assignmentType.Difficulty / 4;
            AssignmentType.OnAssignmentDeadline?.Invoke(_assignmentType,false);
            Destroy(gameObject);
        }
    }
}
