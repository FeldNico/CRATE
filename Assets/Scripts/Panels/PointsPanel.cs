using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PointsPanel : MonoBehaviour
{
    public static UnityAction<int> OnPoints;

    [SerializeField] private TMP_Text _pointsLabel;

    private int _points = 0;

    public int Points
    {
        set
        {
            OnPoints?.Invoke(value-_points);
            ShowHover(value-_points);
            _points = value;
            _pointsLabel.text = _points.ToString();
        }
        get => _points;
    }

    private void ShowHover(int points)
    {
        var canvas = FindObjectOfType<Canvas>();
        var go = new GameObject().AddComponent<TextMeshProUGUI>();
        go.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        var rect = go.transform as RectTransform;
        go.fontSize = 20;
        go.alignment = TextAlignmentOptions.Center;
        rect.sizeDelta = new Vector2(35, 20);
        go.transform.SetParent(canvas.transform,false);
        go.transform.position = Input.mousePosition + new Vector3(30+Random.Range(-3,3),Random.Range(-3,3),0);
        if (points > 0)
        {
            go.text = "+"+points;
            go.faceColor = Color.green;
        }
        else
        {
            go.text = points.ToString();
            go.faceColor = Color.red;
        }

        StartCoroutine(WaitDestroy());
        IEnumerator WaitDestroy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(go.gameObject);
        }
    }
}
