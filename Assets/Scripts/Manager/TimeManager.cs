using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public UnityAction<int> OnNewDay;

    public float DayDuration = 10f;

    public float Day
    {
        get;
        private set;
    }

    [SerializeField]
    private TMP_Text _dayLabel;

    [SerializeField] private Slider _progress;

    private bool _doUpdate = false;
    
    private void Awake()
    {
        FindObjectOfType<MainManager>().OnExperimentStart += () =>
        {
            _doUpdate = true;
        };
    }

    private void Update()
    {
        if (!_doUpdate)
        {
            return;
        }
        
        Day = (int) (Time.time / DayDuration) + (Time.time % DayDuration) / DayDuration;
        _progress.value = (Time.time % DayDuration) / DayDuration;
        var dayString = ((int) Day).ToString();
        if (_dayLabel != null && _dayLabel.text != dayString)
        {
            OnNewDay?.Invoke((int) Day);
            _dayLabel.text = dayString;
        }
    }

    public float GetTimeStampInDays(int days)
    {
        var dayms = (int) (Time.time / DayDuration) + Time.time % DayDuration;
        return Time.time - (dayms - Day) + days * DayDuration;
    }

    public float GetTimeUntilNextDay()
    {
        return DayDuration - (Time.time % DayDuration);
    }
}
