using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MainManager : MonoBehaviour
{
    public UnityAction OnExperimentStart;

    public void OnExperimentStartMessage()
    {
        OnExperimentStart?.Invoke();
    }

}
