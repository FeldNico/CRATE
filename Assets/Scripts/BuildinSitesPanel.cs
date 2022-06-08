using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class BuildinSitesPanel : MonoBehaviour
{
    
    public UnityAction<Config.Config.BuildingSiteStruct> OnBuildingSiteDelete;

    public RectTransform Content => _content;
    public GameObject ConstructionPrefab => _constructionPrefab;
    
    [SerializeField]
    private GameObject _constructionPrefab;
    
    [SerializeField]
    private RectTransform _content;

}
