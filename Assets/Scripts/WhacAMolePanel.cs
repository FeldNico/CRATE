using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WhacAMolePanel : MonoBehaviour
{
    [SerializeField] private GameObject _molePrefab;
    [SerializeField,NonReorderable] private List<Image> _holes;

    private List<Mole> _moles = new List<Mole>();

    private void Start()
    {
        foreach (var hole in _holes)
        {
            _moles.Add(Instantiate(_molePrefab, hole.transform).GetComponent<Mole>());
        }

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            var mole = _moles[Random.Range(0, _moles.Count)];
            mole.Anim();
        }
    }
}
