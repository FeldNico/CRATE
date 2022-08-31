using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LogManager : MonoBehaviour
{
    private static LogManager _instance = null;
    
    [DllImport("__Internal")]
    public static extern void OnFinish(string filename,string textContent);
    
    [DllImport("__Internal")]
    public static extern void OnStart(string isTest);
    
    [DllImport("__Internal")]
    public static extern void OnLog(string isTest);

    private MainManager _mainManager;
    private Dictionary<string, StreamWriter> _writers = new();
    private void Awake()
    {
        _mainManager = FindObjectOfType<MainManager>();

        System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        
        if (File.Exists(Path.Combine(Application.persistentDataPath, "test.csv")))
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "test.csv"));
        }
        if (File.Exists(Path.Combine(Application.persistentDataPath, "trial.csv")))
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "trial.csv"));
        }
        
        _mainManager.OnExperimentStart += () =>
        {
#if !UNITY_EDITOR
            OnStart(_mainManager.IsTest? "1" : "0");
#endif            
            LogMessage("START;"+(_mainManager.IsTest? "1" : "0"));
            
        };

        FindObjectOfType<TimeManager>().OnNewDay += day =>
        {
            LogMessage("NEW_DAY;" + day);
        };

        FindObjectOfType<ClickDetector>().OnClick += (pos, go) =>
        {
            LogMessage("CLICK;" + pos.ToString("F4")+";"+ (go != null ? go.GetFullName(): "NULL"));
        };

        AssignmentType.OnEventQuit += type =>
        {
            LogMessage("EVENT_QUIT;"+type);
        };
        AssignmentType.OnWaitPerfom += type =>
        {
            LogMessage("PERFORM_WAIT;"+type);
        };
        AssignmentType.OnStartPerfom += type =>
        {
            LogMessage("PERFORM_START;"+type);
        };
        AssignmentType.OnNewAssignmentGenerated += type =>
        {
            LogMessage("NEW_EVENT_GENERATED;"+type);
        };
        AssignmentType.OnNewEventAssignment += (type, deadline) =>
        {
            LogMessage("NEW_EVENT_ASSIGNED;"+type+";"+deadline);
        };
        AssignmentType.OnAssignmentDeadline += (type, wasAssigned) =>
        {
            LogMessage("DEADLINE;"+type+";"+wasAssigned);
        };
        AssignmentType.OnEventEnd += type =>
        {
            LogMessage("EVENT_FINISHED;"+type);
        };
        FleetPanel.OnVehicleTypeRequest += (assignmentType,vehicle) =>
        {
            LogMessage("VEHICLE_REQUEST;"+assignmentType+";"+(vehicle != null ? vehicle : "NULL"));
        };
        FleetPanel.OnVehicleTypeReturn += vehicle =>
        {
            LogMessage("VEHICLE_RETURN;"+vehicle);
        };
        SlidingPuzzle.OnNewPuzzle += type =>
        {
            LogMessage("PUZZLE_NEW;"+type);
        };
        SlidingPuzzle.OnPieceSwap += (p1,p2) =>
        {
            LogMessage("PUZZLE_SWAP;"+p1+";"+p2);
        };
        PointsPanel.OnPoints += points =>
        {
            LogMessage("POINTS;"+points);
        };
        VehicleAssignedEventPanel.MissClick += type =>
        {
            LogMessage("VEHICLE;MISS_CLICK;"+type);
        };
    }
    
    public void LogMessage(string message)
    {
        var filename = _mainManager.IsTest ? "test.csv" : "trial.csv";
        var path = Path.Combine(Application.persistentDataPath, filename);
        var folder = Path.GetDirectoryName(path);

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        if (!_writers.ContainsKey(path))
        {
            _writers[path] = new StreamWriter(path, true);
        }

        var now = DateTimeOffset.Now;
        var msg = now.ToString("dd/MM/yyyy HH:mm:ss.fff") + ";" + now.ToUnixTimeMilliseconds() + ";" + message;
        Debug.Log(msg);
        OnLog(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message)));
        _writers[path].WriteLine(msg);
    }
    
    public static void Log(string message)
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<LogManager>();
        }
        
        _instance.LogMessage(message);
    }

    public void DownloadFiles()
    {
        LogMessage("END");
        
        foreach (var writer in _writers.Values)
        {
            writer.Flush();
        }
        #if !UNITY_EDITOR && UNITY_WEBGL
        var filename = _mainManager.IsTest ? "test.csv" : "trial.csv";
        var filePath = Path.Combine(Application.persistentDataPath,_mainManager.IsTest ? "test.csv" : "trial.csv");
        OnFinish( filename,Convert.ToBase64String(File.ReadAllBytes(filePath)));
        #endif
    }
}