using System.Collections;
using UnityEngine;

public class AssignmentsPanel : MonoBehaviour
{
    [SerializeField] private GameObject _assignementPanelPrefab;
    [SerializeField] private RectTransform _content1;
    [SerializeField] private RectTransform _content2;
    
    private TimeManager _timeManager;
    private MainManager _mainManager;
    private int _assignmentTypeIndex = 0;

    private void Awake()
    {
        _timeManager = FindObjectOfType<TimeManager>();
        _mainManager = FindObjectOfType<MainManager>();
        _mainManager.OnExperimentStart += () =>
        {
            _assignmentTypeIndex = 0;
            for (int i = 0; i < 3; i++)
            {
                var assignmentPanel = Instantiate(_assignementPanelPrefab).GetComponent<AssignmentPanel>();
                assignmentPanel.Initialize(AssignmentType.GenerateRandom());
                assignmentPanel.transform.SetParent(_content1,false);
            }
            for (int i = 0; i < 3; i++)
            {
                var assignmentPanel = Instantiate(_assignementPanelPrefab).GetComponent<AssignmentPanel>();
                assignmentPanel.Initialize(AssignmentType.GenerateRandom());
                assignmentPanel.transform.SetParent(_content2,false);
            }
        };

        AssignmentType.OnEventEnd += _ =>
        {
            StartCoroutine(Wait());
        };
        AssignmentType.OnAssignmentDeadline += (_, _) =>
        {
            StartCoroutine(Wait());
        };
        AssignmentType.OnEventQuit += _ =>
        {
            StartCoroutine(Wait());
        };
    }
    
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_timeManager.GetTimeUntilNextDay());
        var assignmentPanel = Instantiate(_assignementPanelPrefab).GetComponent<AssignmentPanel>();
        assignmentPanel.Initialize(AssignmentType.GenerateRandom());
        if (_content1.childCount < _content2.childCount)
        {
            assignmentPanel.transform.SetParent(_content1,false);
        }
        else
        {
            assignmentPanel.transform.SetParent(_content2,false);
        }
    }
}