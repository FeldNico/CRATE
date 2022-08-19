using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class VehicleType : IEquatable<VehicleType>
{
    [field: SerializeField] public string VehicleName { private set; get; } = "No Name";
    
    [field: SerializeField,Min(1)] public int Value { private set; get; } = 1;

    [field: SerializeField] public Sprite VehicleImage { private set; get; }
    
    public VehicleType(string name, int value)
    {
        VehicleName = name;
        Value = value;
    }

    
    public static bool operator ==(VehicleType obj1, VehicleType obj2)
    {
        if (ReferenceEquals(obj1, obj2)) 
            return true;
        if (ReferenceEquals(obj1, null)) 
            return false;
        if (ReferenceEquals(obj2, null))
            return false;
        return obj1.Equals(obj2);
    }
    public static bool operator !=(VehicleType obj1, VehicleType obj2) => !(obj1 == obj2);
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((VehicleType) obj);
    }

    public bool Equals(VehicleType other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return VehicleName == other.VehicleName && Value == other.Value && Equals(VehicleImage, other.VehicleImage);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(VehicleName, Value, VehicleImage);
    }
}
