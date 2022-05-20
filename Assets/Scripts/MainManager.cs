using UnityEngine;
using UnityEngine.Events;

public class MainManager : MonoBehaviour
{
    public UnityAction OnExperimentStart;
    
    public Config.Config Config => _config;

    [SerializeField]
    private Config.Config _config;

    private void Awake()
    {
        if (_config == null)
        {
            _config = Resources.Load<Config.Config>("Config/Config");
        }
    }

    public void OnExperimentStartMessage()
    {
        OnExperimentStart?.Invoke();
    }

}
