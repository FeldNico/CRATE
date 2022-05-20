using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Config", menuName = "CRATE/Config", order = 1)]
    public class Config :ScriptableObject
    {
        public enum VEHICLE_TYPE{
            Supervisor,
            Roller,
            Excavator,
            Truck,
            DemolitionCrane,
        }
        
        public enum PhaseType
        {
            Planning,
            Construction,
            Cleanup,
            Roadwork,
            Demolition
        }
        
        public enum BuildingSiteCategory
        {
            Construction,
            Roadwork,
            Demolition
        }
        
        [Serializable]
        public struct BuildingSiteStruct
        {
            public BuildingSiteCategory Category;
            public List<BuildingPhaseStruct> Phases;
        }
        
        [Serializable]
        public struct BuildingPhaseStruct
        {
            public PhaseType Type;
            public List<VehicleStruct> Vehicles;
        }

        [Serializable]
        public struct VehicleStruct
        {
            public VEHICLE_TYPE Type;
            public int Count;
        }

        [Serializable]
        public struct FleetStruct
        {
            public VEHICLE_TYPE Type;
            public int Count;
        }
        
        public List<BuildingSiteStruct> Conf =
            new List<BuildingSiteStruct>();

        public List<FleetStruct> Fleet = new List<FleetStruct>();
    }
}