using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        var go = Instantiate(ConstructionPrefab).GetComponent<ConstructionPanel>();
        go.transform.SetParent(_scrollRect.content.transform,false);
        go.Dirt = Random.Range(0.01f, 0.6f);
        go.Excavations = Random.Range(0.5f, 1f);
        go = Instantiate(ConstructionPrefab).GetComponent<ConstructionPanel>();
        go.transform.SetParent(_scrollRect.content.transform,false);
        go.Dirt = Random.Range(0.01f, 0.6f);
        go.Excavations = Random.Range(0.5f, 1f);
        go = Instantiate(ConstructionPrefab).GetComponent<ConstructionPanel>();
        go.transform.SetParent(_scrollRect.content.transform,false);
        go.Dirt = Random.Range(0.01f, 0.6f);
        go.Excavations = Random.Range(0.5f, 1f);

        ConstructionPanel.OnConstructionFinished += _ =>
        {
            StartCoroutine(Wait());
            IEnumerator Wait()
            {
                yield return new WaitForSeconds(Random.Range(5f,10f));
                var cp = Instantiate(ConstructionPrefab).GetComponent<ConstructionPanel>();
                cp.transform.SetParent(_scrollRect.content.transform, false);
                cp.Dirt = Random.Range(0.01f, 0.6f);
                cp.Excavations = Random.Range(0.5f, 1f);
            }
        };
    }
}
