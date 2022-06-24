using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Config", menuName = "CRATE/Config", order = 1)]
    public class Config :ScriptableObject
    {
        public enum VEHICLE_TYPE{
            Carousel,
            FoodStand,
            FerrisWheel,
            BouncyCastle,
            LooseBooth,
            BumperCar
        }

        public enum EVENT_CATEGORY
        {
            OpenDoorDay,
            Funfair,
            Party,
            ChristmasMarket
        }
        
        [Serializable]
        public struct EventStruct
        {
            public EVENT_CATEGORY Category;
            public int DurationInDays;
            [NonReorderable]
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
        
        [NonReorderable]
        public List<EventStruct> Conf = new();

        [NonReorderable]
        public List<FleetStruct> Fleet = new();
    }
}