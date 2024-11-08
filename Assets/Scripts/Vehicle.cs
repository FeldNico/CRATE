using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle
{
    public VehicleType Type { private set; get; }
    
    public bool IsBonus { private set; get; }

    public Vehicle(VehicleType type, bool isBonus)
    {
        Type = type;
        IsBonus = isBonus;
    }

    public override string ToString()
    {
        return String.Join(";",new []
        {
            Type.ToString(),
            IsBonus.ToString()
        });
    }
}
