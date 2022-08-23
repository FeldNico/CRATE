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

public class LogManager : MonoBehaviour
{
    private static LogManager _instance = null;
    
    [DllImport("__Internal")]
    public static extern void OnFinish(string textContent);
    
    private Dictionary<string, StreamWriter> _writers = new();
    
    private string _subjectID = null;
    private void Awake()
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

        FindObjectOfType<MainManager>().OnExperimentStart += () =>
        {
            LogMessage("trial.csv","START;"+FindObjectOfType<MainManager>().SubjectID);
        };

        FindObjectOfType<TimeManager>().OnNewDay += day =>
        {
            LogMessage("trial.csv", "NEW_DAY;" + day);
        };

        AssignmentType.OnEventQuit += type =>
        {
            LogMessage("trial.csv","EVENT_QUIT;"+type);
        };
        AssignmentType.OnWaitPerfom += type =>
        {
            LogMessage("trial.csv","PERFORM_WAIT;"+type);
        };
        AssignmentType.OnStartPerfom += type =>
        {
            LogMessage("trial.csv","PERFORM_START;"+type);
        };
        AssignmentType.OnEventEnd += type =>
        {
            LogMessage("trial.csv","EVENT_END;"+type);
        };
        AssignmentType.OnNewAssignmentGenerated += type =>
        {
            LogMessage("trial.csv","NEW_EVENT_GENERATED;"+type);
        };
        AssignmentType.OnNewEventAssignment += (type, deadline) =>
        {
            LogMessage("trial.csv","NEW_EVENT_ASSIGNED;"+type+";"+deadline);
        };
        AssignmentType.OnAssignmentDeadline += (type, wasAssigned) =>
        {
            LogMessage("trial.csv","DEADLINE;"+type+";"+wasAssigned);
        };
        AssignmentType.OnEventEnd += type =>
        {
            LogMessage("trial.csv","EVENT_FINISHED;"+type);
        };
        FleetPanel.OnVehicleTypeRequest += vehicle =>
        {
            LogMessage("trial.csv",vehicle.ToString());
        };
        FleetPanel.OnVehicleTypeReturn += vehicle =>
        {
            LogMessage("trial.csv","VEHICLE_RETURN;"+vehicle);
        };
        SlidingPuzzle.OnNewPuzzle += type =>
        {
            LogMessage("trial.csv","PUZZLE_NEW;"+type);
        };
        SlidingPuzzle.OnPieceSwap += (p1,p2) =>
        {
            LogMessage("trial.csv","PUZZLE_SWAP;"+p1+";"+p2);
        };
        PointsPanel.OnPoints += points =>
        {
            LogMessage("trial.csv","POINTS;"+points);
        };
        VehicleAssignedEventPanel.MissClick += type =>
        {
            LogMessage("trial.csv","VEHICLE;MISS_CLICK;"+type);
        };
    }
    
    public void LogMessage(string filename, string message)
    {
        if (_subjectID == null)
        {
            _subjectID = FindObjectOfType<MainManager>().SubjectID;
        }

        var path = Path.Combine(Application.persistentDataPath, _subjectID, filename.EndsWith(".csv") ? filename : filename+".csv");
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
        _writers[path].WriteLine(now.ToString("dd/MM/yyyy HH:mm:ss.fff") + ";" + now.ToUnixTimeMilliseconds() + ";" + message);
    }
    
    public static void Log(string filename, string message)
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<LogManager>();
        }
        
        _instance.LogMessage(filename, message);
    }

    public void DownloadFiles()
    {
        foreach (var writer in _writers.Values)
        {
            writer.Flush();
            writer.Close();
        }
        var zipPath = Path.Combine(Application.persistentDataPath, _subjectID + ".zip");
        var filePath = Path.Combine(Application.persistentDataPath, _subjectID);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        ZipFile.CreateFromDirectory(Path.Combine(Application.persistentDataPath, _subjectID),zipPath);
        OnFinish( Convert.ToBase64String(File.ReadAllBytes(zipPath)));
    }
}