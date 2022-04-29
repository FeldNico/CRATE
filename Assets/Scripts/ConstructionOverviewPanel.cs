using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        StartCoroutine(NewConstruction());
        IEnumerator NewConstruction()
        {
            while (true)
            {
                var go = Instantiate(ConstructionPrefab).GetComponent<ConstructionPanel>();
                go.transform.SetParent(_scrollRect.content.transform,false);
                go.Dirt = Random.Range(0.01f, 0.6f);
                go.Excavations = Random.Range(0.5f, 1f);
                yield return new WaitForSeconds(30f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
