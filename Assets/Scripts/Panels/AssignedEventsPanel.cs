using UnityEngine;
using UnityEngine.Events;

public class AssignedEventsPanel : MonoBehaviour
{
    [field: SerializeField] public GameObject EventPrefab { get; private set; }
    
    [field: SerializeField] public RectTransform Content { get; private set; }
}
