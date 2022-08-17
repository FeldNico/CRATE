using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [field: SerializeField]
    public VehicleType Type { private set; get; }
    
    public bool IsBonus { private set; get; }
}
