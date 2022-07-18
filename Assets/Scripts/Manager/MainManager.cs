using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    public UnityAction OnExperimentStart;
    
    [field: SerializeField]
    public Config.Config Config { get; private set; }
    

    private void Awake()
    {
        if (Config == null)
        {
            Config = Resources.Load<Config.Config>("Config/Config");
        }
        Config.Fleet =Config.Fleet.OrderBy(s => s.Type).ToList();
    }

    public void OnExperimentStartMessage()
    {
        OnExperimentStart?.Invoke();
    }

}
