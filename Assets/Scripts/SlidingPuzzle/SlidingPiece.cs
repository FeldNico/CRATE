using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlidingPiece : MonoBehaviour, IPointerClickHandler
{
    

    public Vector2Int Position;
    private Vector2Int _initialPosition;

    [SerializeField] private RawImage _image;
    private SlidingPuzzle _puzzle;

    public bool Empty { private set; get; } = false;

    public bool IsCorrect => Position == _initialPosition;

    private void Awake()
    {
        _puzzle = FindObjectOfType<SlidingPuzzle>();
    }

    public void Initalize(Texture texture, int x, int y, bool empty)
    {
        name = x + ":" + y+" "+empty;
        Position = new Vector2Int(x, y);
        _initialPosition = new Vector2Int(x, y);
        Empty = empty;
        if (!empty)
        {
            _image.texture = texture;
            _image.uvRect = new Rect(x * 1f / _puzzle.Size.x, y  * 1f / _puzzle.Size.y, 1f / _puzzle.Size.x, 1f / _puzzle.Size.y);
        }
        else
        {
            _image.color = new Color(0.7f,0.7f,0.7f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Empty && _puzzle.IsInteractable)
        {
            _puzzle.SwapPiece(this,true);
        }
    }

    public override string ToString()
    {
        return String.Join(";", new[]
        {
            _initialPosition.x,
            _initialPosition.y,
            Position.x,
            Position.y
        });
    }
}
