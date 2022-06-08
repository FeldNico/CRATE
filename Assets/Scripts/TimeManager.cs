using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float DayDuration = 10f;
    
    public int Day
    {
        get => _day;
        private set
        {
            _day = value;
            if (_dayLabel != null)
            {
                _dayLabel.text = _day.ToString();
            }
        }
    }

    [SerializeField]
    private TMP_Text _dayLabel;
    private int _day;
    
    public void Start()
    {
        Day = 0;
        StartCoroutine(WaitForDay());
        IEnumerator WaitForDay()
        {
            while (true)
            {
                yield return new WaitForSeconds(DayDuration);
                Day++;
            }
        }
    }
}
