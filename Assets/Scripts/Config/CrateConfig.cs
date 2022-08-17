using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "CRATE/Config", order = 1)]
public class CrateConfig :ScriptableObject
{
    [field: SerializeField, NonReorderable]
    public List<VehicleType> VehicleTypes { private set; get; } = new List<VehicleType>();
        
    [field: SerializeField, NonReorderable]
    public List<Vehicle> Vehicles { private set; get; } = new List<Vehicle>();
}