using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class BuildinSitesPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject ConstructionPrefab;
    
    [SerializeField]
    private RectTransform _content;
    public RectTransform Content => _content;

    public UnityAction<Config.Config.BuildingSiteStruct> OnBuildingSiteDelete;

    private void Start()
    {
        var conf = FindObjectOfType<MainManager>().Config.Conf;
        for (int i = 0; i < 3; i++)
        {
            var buildingSiteStruct = conf[Random.Range(0, conf.Count)];
            var vehiclePanel = Instantiate(ConstructionPrefab).GetComponent<BuildingSite>();
            vehiclePanel.transform.SetParent(_content, false);
            vehiclePanel.Instantiate(buildingSiteStruct);
        }

        OnBuildingSiteDelete += AddBuildingSite;
    }

    private async void AddBuildingSite(Config.Config.BuildingSiteStruct buildingSiteStruct)
    {
        await Task.Delay(5000);
        var conf = FindObjectOfType<MainManager>().Config.Conf;
        var vehiclePanel = Instantiate(ConstructionPrefab).GetComponent<BuildingSite>();
        vehiclePanel.transform.SetParent(_content, false);
        vehiclePanel.Instantiate( conf[Random.Range(0, conf.Count)]);
    }
    
    
}
