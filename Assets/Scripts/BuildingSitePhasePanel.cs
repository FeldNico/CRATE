using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSitePhasePanel : MonoBehaviour
{
    public Config.Config.PhaseType Type => _type;

    [SerializeField] private GameObject VehiclePrefab;

    [SerializeField] private TMP_Text _nameLabel;

    [SerializeField] private Slider _progressBar;
    
    [SerializeField] private Slider _timer;

    [SerializeField] private TMP_Text _daysLeftLabel;

    [SerializeField] private Button _startButton;

    [SerializeField] private RectTransform _content;

    private Config.Config.PhaseType _type;
    private List<VehiclePanel> _vehiclePanels = new();
    private float _creationDay;
    private bool _isProgressing;
    private TimeManager _timeManager;

    public void Initialize(Config.Config.BuildingPhaseStruct buildingPhaseStruct)
    {
        _type = buildingPhaseStruct.Type;
        _nameLabel.text = Enum.GetName(typeof(Config.Config.PhaseType), _type);
        foreach (var vehicleStruct in buildingPhaseStruct.Vehicles)
        {
            var vehicle = Instantiate(VehiclePrefab,_content,false).GetComponent<VehiclePanel>();
            vehicle.Initalize(vehicleStruct.Type,vehicleStruct.Count);
            _vehiclePanels.Add(vehicle);
        }
        
        _startButton.onClick.AddListener(PerformPhase);
        _timeManager = FindObjectOfType<TimeManager>();
    }

    private void PerformPhase()
    {
        _isProgressing = true;
        _startButton.interactable = false;
        foreach (var panel in _vehiclePanels)
        {
            panel.DisableButtons();
        }
        
        var startDay = Time.time;
        _progressBar.value = 0f;

        StartCoroutine(Animate());
        IEnumerator Animate()
        {
            var duration = _timeManager.GetTimeStampInDays(3) - Time.time;
            while (Time.time < startDay + duration)
            {
                yield return null;
                _progressBar.value = (Time.time - startDay)/duration;
            }
            _progressBar.value = 1;
            _startButton.interactable = true;
            _isProgressing = false;
            FindObjectOfType<PointsPanel>().Points += 10;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isProgressing)
        {
            return;
        }

        if (transform.parent.GetChild(0) == transform)
        {
            _startButton.interactable = _vehiclePanels.All(panel => panel.Count == panel.MaxCount);
            foreach (var panel in _vehiclePanels)
            {
                panel.EnableButtons();
            }
        }
        else
        {
            _startButton.interactable = false;
            foreach (var panel in _vehiclePanels)
            {
                panel.DisableButtons();
            }
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