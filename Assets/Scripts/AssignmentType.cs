using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

[Serializable]
public class AssignmentType
{
    public static UnityAction<AssignmentType> OnEventQuit;
    public static UnityAction<AssignmentType> OnWaitPerfom; 
    public static UnityAction<AssignmentType> OnStartPerfom; 
    public static UnityAction<AssignmentType> OnEventEnd; 
    public static UnityAction<AssignmentType> OnNewAssignmentGenerated;
    public static UnityAction<AssignmentType, int> OnNewEventAssignment;
    public static UnityAction<AssignmentType, bool> OnAssignmentDeadline;
    
    [Serializable]
    private struct VehicleTypePair
    {
        public VehicleType Type;
        public int Count;
    }

    private static Random _random = null;
    
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private int _difficulty = 1;
    public int Difficulty => _difficulty;

    [SerializeField] private int _days = 1;
    public int Days => _days;

    [SerializeField]
    private List<VehicleTypePair> _vehiclesPerDay = null;
    public Dictionary<VehicleType, int> VehiclesPerDay {
        get
        {
            var dict = new Dictionary<VehicleType, int>();
            foreach (var pair in _vehiclesPerDay)
            {
                dict[pair.Type] = pair.Count;
            }
            
            return dict;
        }
        set
        {
            if (_vehiclesPerDay == null)
            {
                _vehiclesPerDay = new List<VehicleTypePair>();
            }
            _vehiclesPerDay.Clear();
            foreach (var (type,count) in value.OrderBy(pair => pair.Key.Value))
            {
                _vehiclesPerDay.Add(new VehicleTypePair()
                {
                    Type = type,
                    Count = count
                });
            }
            
        }
    }

    public override string ToString()
    {
        return String.Join(";", new[]
        {
            _name,
            _difficulty.ToString(),
            _days.ToString(),
            String.Join(";",_vehiclesPerDay.Select(pair => pair.Type+";"+pair.Count)), 
        });
    }

    public static AssignmentType GenerateRandom()
    {
        if (_random == null)
        {
            var isTest = GameObject.FindObjectOfType<MainManager>().IsTest;
            _random = isTest ? new Random(CrateConfig.Instance.DemoSeed) : new Random(CrateConfig.Instance.Seed);
        }

        (int, int, Dictionary<VehicleType, int>) result = (-1, 0, null);
        while (result.Item1 == -1)
        {
            result = TryRandomConfiguration();
        }
        
        var assigmentType = new AssignmentType();
        var name = CrateConfig.Instance.Prefixes[_random.Next(0,CrateConfig.Instance.Prefixes.Count)] + " " +
                   CrateConfig.Instance.Suffixes[_random.Next(0,CrateConfig.Instance.Suffixes.Count)];

        assigmentType._name = name;
        assigmentType._difficulty = result.Item2;
        assigmentType._days = result.Item1;
        assigmentType.VehiclesPerDay = result.Item3;
        return assigmentType;
    }

    private static (int,int,Dictionary<VehicleType, int>) TryRandomConfiguration()
    {
        var difficulty = _random.Next(1, 70);
        var possibleDays = Enumerable.Range(CrateConfig.Instance.MinMaxDays.x, CrateConfig.Instance.MinMaxDays.y).Where(day => difficulty % day ==0);
        var types = CrateConfig.Instance.VehicleTypes.OrderBy(_ => _random.Next()).ToList();
        
        while (!possibleDays.Any())
        {
            difficulty = _random.Next(1, 70);
            possibleDays = Enumerable.Range(CrateConfig.Instance.MinMaxDays.x, CrateConfig.Instance.MinMaxDays.y).Where(day => difficulty % day ==0);
        }
        Dictionary<VehicleType, int> fleet = new Dictionary<VehicleType, int>();
        possibleDays = possibleDays.OrderBy(_ => _random.Next());
        var days = -1;
        foreach (var possibleDay in possibleDays)
        {
            fleet.Clear();
            if (GetVehiclesRecursive(types, 0, difficulty / possibleDay,possibleDay, fleet))
            {
                days = possibleDay;
                break;
            }
        }

        if (days == -1)
        {
            return (-1, 0, null);
        }

        return (days, difficulty, fleet);
    }
    
    private static bool GetVehiclesRecursive(List<VehicleType> types, int index, int difficultyPerDay, int days,
        Dictionary<VehicleType, int> previousTypes)
    {
        if (previousTypes.Select(pair => pair.Key.Value * pair.Value).Sum() > difficultyPerDay)
        {
            return false;
        }
        
        var possibleCounts = Enumerable.Range(0, CrateConfig.Instance.MaxVehiclesPerTypePerDay)
            .Where(count => count * types[index].Value <= difficultyPerDay && count <= CrateConfig.Instance.GetFleet()[types[index]]).OrderBy(_ => _random.Next());

        foreach (var count in possibleCounts)
        {
            if (count > 0)
            {
                previousTypes[types[index]] = count;
            }
            if (index < types.Count - 1)
            {
                if (GetVehiclesRecursive(types, index + 1, difficultyPerDay,days, previousTypes))
                {
                    return true;
                }
            }
            else
            {
                if (previousTypes.Select(pair => pair.Key.Value * pair.Value).Sum() ==
                    difficultyPerDay && previousTypes.Count >= CrateConfig.Instance.MinTypeCount)
                {
                    return true;
                }
            }
        }

        if (previousTypes.ContainsKey(types[index]))
        {
            previousTypes.Remove(types[index]);
        }
        return false;
    }
}