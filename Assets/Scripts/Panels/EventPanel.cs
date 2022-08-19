using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventPanel : MonoBehaviour
{
    public static UnityAction<AssignmentType> OnEventClose; 

    [SerializeField] private GameObject VehiclePrefab;

    [SerializeField] private TMP_Text _nameLabel;

    [SerializeField] private Button _deleteButton;

    [SerializeField] private Slider _progressBar;

    [SerializeField] private TMP_Text _deadlineLabel;
    
    [SerializeField] private TMP_Text _durationLabel;
    
    [SerializeField] private TMP_Text _pointsLabel;

    [SerializeField] private Button _startButton;

    [SerializeField] private RectTransform _content;
    
    private List<VehicleAssignedEventPanel> _vehiclePanels = new();
    private AssignmentType _assignmentType;
    private int _deadline;
    private int _startDay;
    private bool _isProgressing;
    private TimeManager _timeManager;
    private GameObject _hover;

    
    public void Initialize(AssignmentType type, int deadline)
    {
        _timeManager = FindObjectOfType<TimeManager>();
        _assignmentType = type;
        _deadline = deadline;
        _nameLabel.text = _assignmentType.Name;
        _deadlineLabel.text = _deadline + " Tage";
        _durationLabel.text = _assignmentType.Days+" Tage";
        _pointsLabel.text = _assignmentType.Difficulty + " Punkte";
        _startDay = (int) _timeManager.Day;
        foreach (var (vehicleType,count) in type.VehiclesPerDay)
        {
            var vehicle = Instantiate(VehiclePrefab,_content,false).GetComponent<VehicleAssignedEventPanel>();
            vehicle.Initialize(vehicleType,count);
            _vehiclePanels.Add(vehicle);
        }
        
        _startButton.onClick.AddListener(PerformPhase);
        _deleteButton.onClick.AddListener(() =>
        {
            FindObjectOfType<PointsPanel>().Points -= _assignmentType.Difficulty / 2;
            Destroy(gameObject);
        });
    }

    
    private void PerformPhase()
    {
        _isProgressing = true;
        _deadlineLabel.text = "-";
        _startButton.interactable = false;
        foreach (var panel in _vehiclePanels)
        {
            panel.AreButtonsEnabled = false;
        }

        StartCoroutine(Animation());
        IEnumerator Animation()
        {
            var startDay = Time.time;
            _startButton.GetComponentInChildren<TMP_Text>().text = "Event startet am nächsten Tag.";
            var duration = _timeManager.GetTimeUntilNextDay();
            _progressBar.value = 0f;
            while (Time.time < startDay + duration)
            {
                yield return null;
                _progressBar.value = (Time.time - startDay)/duration;
            }
            _progressBar.value = 1;
            _startButton.GetComponentInChildren<TMP_Text>().text = "Event läuft...";
            startDay = Time.time;
            duration = _timeManager.GetTimeStampInDays(_assignmentType.Days) - Time.time;
            _progressBar.value = 0f;
            while (Time.time < startDay + duration)
            {
                yield return null;
                _durationLabel.text = "Noch " + ((int) (((duration+startDay) -Time.time) / _timeManager.DayDuration) +1) + " Tag(e)";
                _progressBar.value = (Time.time - startDay)/duration;
            }
            _progressBar.value = 1;
            _startButton.interactable = true;
            FindObjectOfType<PointsPanel>().Points += _assignmentType.Difficulty;
            Destroy(gameObject);
        }
    }
    
    private void ShowHover(int days)
    {
        if (_hover != null)
        {
            Destroy(_hover);
        }
        var canvas = FindObjectOfType<Canvas>();
        var go = new GameObject().AddComponent<TextMeshProUGUI>();
        _hover = go.gameObject;
        go.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        var rect = go.transform as RectTransform;
        rect.sizeDelta = new Vector2(100, 100);
        go.fontSize = 20;
        go.alignment = TextAlignmentOptions.Center;
        go.transform.SetParent(canvas.transform,false);
        go.transform.position = _durationLabel.transform.position + Vector3.up *20f;
        if (days > 0)
        {
            go.text = "+"+days;
            go.faceColor = Color.red;
        }
        else
        {
            go.text = days.ToString();
            go.faceColor = Color.green;
        }

        go.text += " Tag(e)";

        StartCoroutine(WaitDestroy());
        IEnumerator WaitDestroy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(_hover);
        }
    }

    private void Update()
    {
        if (_isProgressing)
        {
            return;
        }
        var daysLeft = _deadline - ((int) _timeManager.Day - _startDay);
        _deadlineLabel.text = daysLeft + " Tage";
        if (daysLeft < 0 && _progressBar.value <=0.8f)
        {
            FindObjectOfType<PointsPanel>().Points -= _assignmentType.Difficulty / 3;
            Destroy(gameObject);
            return;
        }

        _deleteButton.interactable = _vehiclePanels.All(panel => panel.IsEmpty);
        _startButton.interactable = _vehiclePanels.All(panel => panel.IsSatisfied);
        if (_startButton.interactable)
        {
            _startButton.GetComponentInChildren<TMP_Text>().text = "Sende Geschäfte";
        }
        else
        {
            _startButton.GetComponentInChildren<TMP_Text>().text = "Geschäfte nicht zugewiesen";
        }
    }

    private void OnDestroy()
    {
        OnEventClose?.Invoke(_assignmentType);
        _isProgressing = false;
        if (_hover != null)
        {
            Destroy(_hover);
        }
    }
    
}
