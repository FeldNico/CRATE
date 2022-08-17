using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CrateConfig", menuName = "CRATE/CrateConfig", order = 1)]
public class CrateConfig :ScriptableObject
{
    private static CrateConfig _instance = null;
    public static CrateConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CrateConfig>("Config/CrateConfig");
            }

            return _instance;
        }
    }

    [SerializeField]
    private List<VehicleType> _vehicleTypes = new List<VehicleType>();

    public List<VehicleType> VehicleTypes => _vehicleTypes;

    [SerializeField]
    private Vector2Int _minMaxDays= new Vector2Int(1, 5);
    public Vector2Int MinMaxDays => _minMaxDays;
    
    [SerializeField,Min(1)]
    private int _minVehicleCount= 2;

    public int MinVehicleCount => _minVehicleCount;
    
    [SerializeField,Min(1)]
    private int _maxVehicleCountPerTypePerDay= 2;

    public int MaxVehicleCountPerTypePerDay => _maxVehicleCountPerTypePerDay;
    
    public Dictionary<VehicleType,int> GetFleet()
    {
        var lcm = calculateLCM(VehicleTypes.Select(vehicleType => vehicleType.Value).ToArray());
        var maxValue = VehicleTypes.Max(type => type.Value);
        var maxCount = lcm / maxValue;
        while (maxCount < MinVehicleCount)
        {
            lcm += lcm;
            maxCount = lcm / maxValue;
        }
        var dict = new Dictionary<VehicleType, int>();
        foreach (var type in VehicleTypes)
        {
            dict[type] = lcm / type.Value;
        }

        return dict;
    }
    
    private int calculateLCM(int[] arr)
    {
        int gcd(int a, int b)
        {
            if (b == 0)
                return a;
            return gcd(b, a % b);
        }
        
        var ans = arr[0];
        
        for (int i = 0; i < arr.Length; i++)
            ans = (((arr[i] * ans)) /
                   (gcd(arr[i], ans)));

        return ans;
    }
}