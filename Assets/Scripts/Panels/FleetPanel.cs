using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FleetPanel : MonoBehaviour
{

    [SerializeField] private GameObject VehiclePanelPrefab;

    [SerializeField] private RectTransform _content;

    private List<Vehicle> _vehicles = new List<Vehicle>();

    private void Awake()
    {
        FindObjectOfType<MainManager>().OnExperimentStart += () =>
        {
            var fleet = CrateConfig.Instance.GetFleet();
            foreach (var type in fleet.Keys)
            {
                for (int i = 0; i < fleet[type]; i++)
                {
                    _vehicles.Add(new Vehicle(type, false));
                }
            }
        };
    }
}

