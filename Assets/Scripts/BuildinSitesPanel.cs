using UnityEngine;

public class BuildinSitesPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject ConstructionPrefab;
    
    [SerializeField]
    private RectTransform _content;
    public RectTransform Content => _content;

    private void Start()
    {
        var vehiclePanel = Instantiate(ConstructionPrefab).GetComponent<BuildingSite>();
        vehiclePanel.transform.SetParent(_content, false);
        vehiclePanel = Instantiate(ConstructionPrefab).GetComponent<BuildingSite>();
        vehiclePanel.transform.SetParent(_content, false);
    }
}
