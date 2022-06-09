using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssignmentPanel : MonoBehaviour
{
    [SerializeField] private GameObject _divider;
    [SerializeField] private GameObject _vehiclePrefab;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _days;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Button _button;

    public void Initialize(Config.Config.BuildingSiteStruct info)
    {
        _label.text = Enum.GetName(typeof(Config.Config.BuildingSiteCategory), info.Category);
        _days.text = info.Phases.Count * 5f + " Days Needed";
        foreach (var phaseStruct in info.Phases)
        {
            if (info.Phases.IndexOf(phaseStruct) != 0)
            {
                Instantiate(_divider,_content,false);
            }
            foreach (var vehicleStruct in phaseStruct.Vehicles)
            {
                var vehicle = Instantiate(_vehiclePrefab).GetComponent<VehiclePanel>();
                (vehicle.transform as RectTransform).sizeDelta *= 0.6f;
                (vehicle.transform as RectTransform).localScale *= 0.6f;
                vehicle.transform.SetParent(_content,false);
                vehicle.Initalize(vehicleStruct.Type,vehicleStruct.Count);
            }
        }
        _button.onClick.AddListener(() =>
        {
            var bsp = FindObjectOfType<BuildinSitesPanel>();
            var vehiclePanel = Instantiate(bsp.ConstructionPrefab,bsp.Content,false).GetComponent<BuildingSite>();
            vehiclePanel.Instantiate(info);
            Destroy(gameObject);
        });
    }
}
