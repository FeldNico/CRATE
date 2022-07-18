using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPanel : MonoBehaviour
{
    [field: SerializeField] public Config.Config.EVENT_CATEGORY Category { get; private set; }
    
    [SerializeField] private GameObject VehiclePrefab;

    [SerializeField] private TMP_Text _nameLabel;

    [SerializeField] private Button _deleteButton;

    [SerializeField] private Slider _progressBar;

    [SerializeField] private TMP_Text _deadlineLabel;
    
    [SerializeField] private TMP_Text _durationLabel;
    
    [SerializeField] private TMP_Text _pointsLabel;

    [SerializeField] private Button _startButton;

    [SerializeField] private RectTransform _content;
    
    private List<VehiclePanel> _vehiclePanels = new();
    private Config.Config.EventStruct _eventStruct;
    private int _deadline;
    private int _startDay;
    private bool _isProgressing;
    private TimeManager _timeManager;
    

    public void Initialize(Config.Config.EventStruct eventStruct, int deadline)
    {
        _timeManager = FindObjectOfType<TimeManager>();
        _eventStruct = eventStruct;
        _deadline = deadline;
        Category = eventStruct.Category;
        _nameLabel.text = Enum.GetName(typeof(Config.Config.EVENT_CATEGORY), Category);
        _deadlineLabel.text = _deadline + " Tage";
        _durationLabel.text = eventStruct.DurationInDays+" Tage";
        _pointsLabel.text = eventStruct.DurationInDays * 5 + " Punkte";
        _startDay = (int) _timeManager.Day;
        var vehicles = eventStruct.Vehicles;
        foreach (var vehicleStruct in vehicles.OrderBy(s => s.Type))
        {
            var vehicle = Instantiate(VehiclePrefab,_content,false).GetComponent<VehiclePanel>();
            vehicle.Initalize(vehicleStruct.Type,vehicleStruct.Count);
            _vehiclePanels.Add(vehicle);
        }
        
        _startButton.onClick.AddListener(PerformPhase);
        _deleteButton.onClick.AddListener(() =>
        {
            FindObjectOfType<PointsPanel>().Points -= _eventStruct.DurationInDays * 5;
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
            panel.DisableButtons();
        }

        StartCoroutine(Animation());
        IEnumerator Animation()
        {
            var startDay = Time.time;
            _startButton.GetComponentInChildren<TMP_Text>().text = "Geschäfte versendet";
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
            duration = _timeManager.GetTimeStampInDays(_eventStruct.DurationInDays) - Time.time;
            _progressBar.value = 0f;
            while (Time.time < startDay + duration)
            {
                yield return null;
                _durationLabel.text = "Noch " + ((int) (((duration+startDay) -Time.time) / _timeManager.DayDuration) +1) + " Tag(e)";
                _progressBar.value = (Time.time - startDay)/duration;
            }
            _progressBar.value = 1;
            _startButton.interactable = true;
            _isProgressing = false;
            FindObjectOfType<PointsPanel>().Points += _eventStruct.DurationInDays * 5;
            Destroy(gameObject);
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
            FindObjectOfType<PointsPanel>().Points -= _eventStruct.DurationInDays * 5;
            Destroy(gameObject);
            return;
        }

        _startButton.interactable = _vehiclePanels.All(panel => panel.Count == panel.MaxCount);
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
        var fleetPanel = FindObjectOfType<FleetPanel>();
        if (fleetPanel != null)
        {
            foreach (var panel in _vehiclePanels)
            {
                for (int i = 0; i < panel.Count; i++)
                {
                    fleetPanel.AddVehicle(panel.Type);
                }
            }
        }
    }
}
