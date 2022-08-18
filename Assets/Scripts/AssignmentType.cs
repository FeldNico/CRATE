using System;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AssignmentType
{
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private int _difficulty = 1;
    public int Difficulty => _difficulty;

    [SerializeField] private int _days = 1;
    public int Days => _days;

    [SerializeField]
    private Dictionary<VehicleType, int> _vehiclesPerDay = null;
    public Dictionary<VehicleType, int> VehiclesPerDay => _vehiclesPerDay;

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
        assigmentType._vehiclesPerDay = fleet;
        return assigmentType;
    }
}