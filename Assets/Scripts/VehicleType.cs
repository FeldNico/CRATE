using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleType : MonoBehaviour
{
    [field: SerializeField]
    public int Name { private set; get; }
    
    [field: SerializeField]
    public int Value { private set; get; }
}
