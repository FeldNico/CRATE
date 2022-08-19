using UnityEngine;
using UnityEngine.Events;

public class AssignedEventsPanel : MonoBehaviour
{
    [field: SerializeField] public GameObject EventPrefab { get; private set; }
    
    [field: SerializeField] public RectTransform Content { get; private set; }

    public void AddEvent(AssignmentType type,int deadline)
    {
        var eventPanel = Instantiate(EventPrefab).GetComponent<EventPanel>();
        eventPanel.Initialize(type,deadline);
        eventPanel.transform.SetParent(Content,false);
    }
}
