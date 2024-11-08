using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventPanel : MonoBehaviour
{

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
    public AssignmentType AssignmentType { private set; get; }
    private int _deadline;
    private int _startDay;
    private bool _isProgressing;
    private TimeManager _timeManager;
    private MainManager _mainManager;
    private GameObject _hover;

    private LanguageDictionary _languageDictionary;

    public void Initialize(AssignmentType type, int deadline)
    {
        name = type.Name;
        _timeManager = FindObjectOfType<TimeManager>();
        _mainManager = FindObjectOfType<MainManager>();
        _languageDictionary = Resources.Load<LanguageDictionary>("Config/LanguageDictionary");
        AssignmentType = type;
        _deadline = deadline;
        _nameLabel.text = AssignmentType.Name;
        _deadlineLabel.text = _deadline.ToString();
        _durationLabel.text = AssignmentType.Days.ToString();
        _pointsLabel.text = AssignmentType.Difficulty.ToString();
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
            _mainManager.PlaySound(_mainManager.EventClosed);
            AssignmentType.OnEventQuit?.Invoke(AssignmentType);
            FindObjectOfType<PointsPanel>().Points -= AssignmentType.Difficulty;
            Destroy(gameObject);
        });
    }

    
    private void PerformPhase()
    {
        AssignmentType.OnWaitPerfom.Invoke(AssignmentType);
        _isProgressing = true;
        _deadlineLabel.faceColor = Color.black;
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
            _startButton.GetComponentInChildren<TMP_Text>().text = _languageDictionary.Translate("event_next_day");
            var duration = _timeManager.GetTimeUntilNextDay();
            _progressBar.value = 0f;
            while (Time.time < startDay + duration)
            {
                yield return null;
                _progressBar.value = (Time.time - startDay)/duration;
            }
            AssignmentType.OnStartPerfom.Invoke(AssignmentType);
            _progressBar.value = 1;
            _startButton.GetComponentInChildren<TMP_Text>().text = _languageDictionary.Translate("event_running");
            startDay = Time.time;
            duration = _timeManager.GetTimeStampInDays(AssignmentType.Days) - Time.time;
            _progressBar.value = 0f;
            while (Time.time < startDay + duration)
            {
                yield return null;
                _durationLabel.text = ((int) (((duration+startDay) -Time.time) / _timeManager.DayDuration) +1).ToString();
                _progressBar.value = (Time.time - startDay)/duration;
            }
            _progressBar.value = 1;
            _startButton.interactable = true;
            FindObjectOfType<PointsPanel>().Points += AssignmentType.Difficulty;
            _mainManager.PlaySound(_mainManager.EventFinished);
            AssignmentType.OnEventEnd?.Invoke(AssignmentType);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isProgressing || _languageDictionary == null)
        {
            return;
        }
        var daysLeft = _deadline - ((int) _timeManager.Day - _startDay);
        _deadlineLabel.text = daysLeft.ToString();
        if (daysLeft <= 3)
        {
            if (daysLeft <= 1)
            {
                foreach (var child in _deadlineLabel.GetComponentsInChildren<TMP_Text>())
                {
                    child.faceColor = Color.red;
                }
            }
            else
            {
                foreach (var child in _deadlineLabel.GetComponentsInChildren<TMP_Text>())
                {
                    child.faceColor = new Color(1f,0.533f,0f);
                }
            }
        }
        else
        {
            foreach (var child in _deadlineLabel.GetComponentsInChildren<TMP_Text>())
            {
                child.faceColor = Color.black;
            }
        }
        if (daysLeft < 0 && _progressBar.value <=0.8f)
        {
            AssignmentType.OnAssignmentDeadline.Invoke(AssignmentType,true);
            FindObjectOfType<PointsPanel>().Points -= AssignmentType.Difficulty / 2;
            Destroy(gameObject);
            return;
        }

        _deleteButton.interactable = _vehiclePanels.All(panel => panel.IsEmpty);
        _startButton.interactable = _vehiclePanels.All(panel => panel.IsSatisfied);
        if (_startButton.interactable)
        {
            _startButton.GetComponentInChildren<TMP_Text>().text = _languageDictionary.Translate("send_rides");
        }
        else
        {
           
            _startButton.GetComponentInChildren<TMP_Text>().text =  _languageDictionary.Translate("rides_not_assigned");
        }
    }

    private void OnDestroy()
    {
        _isProgressing = false;
        if (_hover != null)
        {
            Destroy(_hover);
        }
    }
    
}
