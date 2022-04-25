using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionOverviewPanel : MonoBehaviour
{

    public GameObject ConstructionPrefab;
    
    private ScrollRect _scrollRect;

    private void Awake()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var go = Instantiate(ConstructionPrefab);
            go.transform.SetParent(_scrollRect.content.transform,false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
