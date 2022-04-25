using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour
{
    public GameObject VehiclePrefab;
    
    private ScrollRect _scrollRect;

    private void Awake()
    {
        _scrollRect = GetComponentInChildren<ScrollRect>();
    }

    private void Start()
    {
        
        for (int i = 0; i < 10; i++)
        {
            var go = Instantiate(VehiclePrefab);
            go.transform.SetParent(_scrollRect.content.transform,false);
        }
        
    }
}
