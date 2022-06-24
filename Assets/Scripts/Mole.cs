using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mole : MonoBehaviour, IPointerClickHandler
{
    private Slider _slider;
    private Coroutine _coroutine;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        gameObject.SetActive(false);
    }

    public void Anim()
    {
        gameObject.SetActive(true);
        _coroutine = StartCoroutine(Wait());
        IEnumerator Wait()
        {
            var startTime = Time.time;
            _slider.value = 0;
            while (Time.time < startTime + 0.3f)
            {
                yield return null;
                _slider.value = (Time.time - startTime) / 0.3f;
            }
            _slider.value = 1;
            yield return new WaitForSeconds(1f);
            startTime = Time.time;
            while (Time.time < startTime + 0.3f)
            {
                yield return null;
                _slider.value = 1 - ((Time.time - startTime)/0.3f);
            }
            _slider.value = 0;
            gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StopCoroutine(_coroutine);
        _slider.value = 0;
        gameObject.SetActive(false);
        FindObjectOfType<PointsPanel>().Points += 1;
    }
}
