using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SlidingPuzzle : MonoBehaviour
{

    [field: SerializeField] public Vector2Int Size { private set; get; }
    [SerializeField] private GameObject _piecePrefab;
    [SerializeField] private GridLayoutGroup _content;
    private List<Texture> _textures;
    private Texture _currentTexture;

    private SlidingPiece[,] _pieces;
    private void Start()
    {
        var sizeDelta = (_content.transform as RectTransform).sizeDelta;
        var min = Mathf.Min(sizeDelta.x, sizeDelta.y) - 5*Mathf.Max(Size.x,Size.y);
        _content.cellSize = new Vector2(min / Size.x,
            min / Size.y);
        _content.spacing = 5 * Vector2.one;

        _pieces = new SlidingPiece[Size.x, Size.y];

        _textures = Resources.LoadAll<Texture>("Images").ToList();
        
        Initalize(_textures[Random.Range(0,_textures.Count)]);
    }

    public void Initalize(Texture texture)
    {
        _currentTexture = texture;
        foreach (var child in _content.GetComponentsInChildren<SlidingPiece>())
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var piece = Instantiate(_piecePrefab,_content.transform).GetComponent<SlidingPiece>();
                piece.Initalize(texture,j,i,i == Size.x - 1 && j == Size.y - 1);
                _pieces[j, i] = piece;
            }
        }

        var oldPieces = new SlidingPiece[Math.Max(Size.x,Size.y)*2];
        var currentIndex = 0;
        for (int i = 0; i < 50; i++)
        {
            var piece = _pieces[Random.Range(0, Size.x), Random.Range(0, Size.y)];
            while (GetEmptyNeighbour(piece) == null && !oldPieces.ToList().Contains(piece))
            {
                piece = _pieces[Random.Range(0, Size.x), Random.Range(0, Size.y)];
            }

            oldPieces[currentIndex++ % (Math.Max(Size.x,Size.y)*2)] = piece;
            SwapPiece(piece);
        }
    }

    public void SwapPiece(SlidingPiece piece)
    {
        var empty = GetEmptyNeighbour(piece);
        if (empty == null)
        {
            return;
        }

        var indexA = piece.transform.GetSiblingIndex();
        var indexB = empty.transform.GetSiblingIndex();
        var posA = piece.Position;
        var posB = empty.Position;
        
        piece.transform.SetSiblingIndex(indexB);
        _pieces[posB.x,posB.y] = piece;
        piece.Position = posB;
        
        empty.transform.SetSiblingIndex(indexA);
        _pieces[posA.x,posA.y] = empty;
        empty.Position = posA;

        var finished = true;
        foreach (var slidingPiece in _pieces)
        {
            if (!slidingPiece.IsCorrect)
            {
                finished = false;
            }
        }

        if (finished)
        {
            FindObjectOfType<PointsPanel>().Points += 10;

            var newTexture = _textures[Random.Range(0, _textures.Count)];
            while (newTexture == _currentTexture)
            {
                newTexture = _textures[Random.Range(0, _textures.Count)];
            }
            
            Initalize(newTexture);
        }
    }

    private SlidingPiece GetEmptyNeighbour(SlidingPiece piece)
    {
        if (piece.Position.x > 0)
        {
            var n = _pieces[piece.Position.x - 1, piece.Position.y];
            if (n.Empty)
            {
                return n;
            }
        }
        
        if (piece.Position.x < Size.x-1)
        {
            var n = _pieces[piece.Position.x + 1, piece.Position.y];
            if (n.Empty)
            {
                return n;
            }
        }
        
        if (piece.Position.y > 0)
        {
            var n = _pieces[piece.Position.x, piece.Position.y-1];
            if (n.Empty)
            {
                return n;
            }
        }
        
        if (piece.Position.y < Size.y-1)
        {
            var n = _pieces[piece.Position.x, piece.Position.y+1];
            if (n.Empty)
            {
                return n;
            }
        }

        return null;
    }
}