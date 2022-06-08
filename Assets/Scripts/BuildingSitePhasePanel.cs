using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private List<VehiclePanel> _vehiclePanels = new List<VehiclePanel>();
    private float _creationDay;
    private bool _isProgressing;
    private TimeManager _timeManager;
    private Config.Config.BuildingPhaseStruct _struct;

    public void Initialize(Config.Config.BuildingPhaseStruct buildingPhaseStruct)
    {
        _type = buildingPhaseStruct.Type;
        _nameLabel.text = Enum.GetName(typeof(Config.Config.PhaseType), _type);
        foreach (var vehicleStruct in buildingPhaseStruct.Vehicles)
        {
            var vehicle = Instantiate(VehiclePrefab).GetComponent<VehiclePanel>();
            vehicle.transform.SetParent(_content, false);
            vehicle.Initalize(vehicleStruct);
            _vehiclePanels.Add(vehicle);
        }

        _struct = buildingPhaseStruct;
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
        
        var startDay = _timeManager.Day;
        _progressBar.value = 0f;

        StartCoroutine(Animate());
        IEnumerator Animate()
        {
            while (_timeManager.Day < startDay + 3)
            {
                yield return null;
                _progressBar.value = (_timeManager.Day - startDay)/3f;
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

        /*
        _timer.value = 1- (_timeManager.Day - _creationDay) / MaxDay;

        if (_timer.value <= float.Epsilon)
        {
            FindObjectOfType<PointsPanel>().Points -= 5;
            Destroy(gameObject);
        }
        */

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