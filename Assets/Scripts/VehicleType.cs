using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class VehicleType
{
    [field: SerializeField] public string VehicleName { private set; get; } = "No Name";
    
    [field: SerializeField,Min(1)] public int Value { private set; get; } = 1;

    [field: SerializeField] public Sprite VehicleImage { private set; get; }
    
    public VehicleType(string name, int value)
    {
        VehicleName = name;
        Value = value;
    }
}
