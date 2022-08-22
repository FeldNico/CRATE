using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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

    private static bool GetVehiclesRecursive(List<VehicleType> types, int index, int difficultyPerDay, int days,
        Dictionary<VehicleType, int> previousTypes)
    {
        if (previousTypes.Select(pair => pair.Key.Value * pair.Value).Sum() > difficultyPerDay)
        {
            return false;
        }
        
        var possibleCounts = Enumerable.Range(0, CrateConfig.Instance.MaxVehiclesPerTypePerDay)
            .Where(count => count * types[index].Value <= difficultyPerDay && count <= CrateConfig.Instance.GetFleet()[types[index]]).OrderBy(_ => new System.Random().Next());

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
                    difficultyPerDay && previousTypes.Count >= CrateConfig.Instance.MinTypeCount && CrateConfig.Instance.AssignmentTypes.All(assignmentType => assignmentType.Days != days || assignmentType.Difficulty != difficultyPerDay*days))
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
    
    public static AssignmentType GenerateRandom(int difficulty)
    {
        var types = CrateConfig.Instance.VehicleTypes.OrderBy(_ => new System.Random().Next()).ToList();
        
        var possibleDays = Enumerable.Range(CrateConfig.Instance.MinMaxDays.x, CrateConfig.Instance.MinMaxDays.y).Where(day => difficulty % day ==0)
            .OrderBy(_ => new System.Random().Next());
        Dictionary<VehicleType, int> fleet = new Dictionary<VehicleType, int>();
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
            return null;
        }

        var assigmentType = new AssignmentType();
        var name = CrateConfig.Instance.Prefixes[Random.Range(0, CrateConfig.Instance.Prefixes.Count)] + " " +
                   CrateConfig.Instance.Suffixes[Random.Range(0, CrateConfig.Instance.Suffixes.Count)];
        while (CrateConfig.Instance.AssignmentTypes.Any(type => type.Name == name))
        {
            name = CrateConfig.Instance.Prefixes[Random.Range(0, CrateConfig.Instance.Prefixes.Count)] + " " +
                   CrateConfig.Instance.Suffixes[Random.Range(0, CrateConfig.Instance.Suffixes.Count)];
        }

        assigmentType._name = name;
        assigmentType._difficulty = difficulty;
        assigmentType._days = days;
        assigmentType.VehiclesPerDay = fleet;
        return assigmentType;
    }
}