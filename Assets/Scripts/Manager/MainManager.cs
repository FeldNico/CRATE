using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    public UnityAction OnExperimentStart;
    
    [field: SerializeField]
    public Config.Config Config { get; private set; }

    public string SubjectID { private set; get; }
    

    private void Awake()
    {
        if (Config == null)
        {
            Config = Resources.Load<Config.Config>("Config/Config");
        }
        Config.Fleet =Config.Fleet.OrderBy(s => s.Type).ToList();
    }

    private void Start()
    {
        #if UNITY_EDITOR
        OnExperimentStartMessage("11111");
        #endif
    }

    public void OnExperimentStartMessage(string sid)
    {
        OnExperimentStart?.Invoke();
    }

}
