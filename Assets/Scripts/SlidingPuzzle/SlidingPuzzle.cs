using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

public class SlidingPuzzle : MonoBehaviour
{
    public static UnityAction<VehicleType> OnNewPuzzle;
    public static UnityAction<SlidingPiece, SlidingPiece> OnPieceSwap;
    
    public bool IsInteractable { private set; get; } = true;
    [field: SerializeField] public Vector2Int Size { private set; get; }
    [SerializeField] private GameObject _piecePrefab;
    [SerializeField] private GridLayoutGroup _content;
    
    private VehicleType _currentType;
    private MainManager _mainManager;
    private SlidingPiece[,] _pieces;
    private Random _random;

    private void Awake()
    {
        _mainManager = FindObjectOfType<MainManager>();
        _mainManager.OnExperimentStart += () =>
        {
            if (_random == null)
            {
                _random = _mainManager.IsTest ? new Random() : new Random(CrateConfig.Instance.Seed);
            }
            
            var sizeDelta = (_content.transform as RectTransform).sizeDelta;
            var min = Mathf.Min(sizeDelta.x, sizeDelta.y) - 5*Mathf.Max(Size.x,Size.y);
            _content.cellSize = new Vector2(min / Size.x,
                min / Size.y);
            _content.spacing = 5 * Vector2.one;

            _pieces = new SlidingPiece[Size.x, Size.y];
            
            Initalize(CrateConfig.Instance.VehicleTypes[_random.Next(0,CrateConfig.Instance.VehicleTypes.Count-1)]);
        };
    }

    public void Initalize(VehicleType type)
    {
        name = type.VehicleName+" Puzzle";
        OnNewPuzzle?.Invoke(type);
        _currentType = type;
        foreach (var child in _content.GetComponentsInChildren<SlidingPiece>())
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var piece = Instantiate(_piecePrefab,_content.transform).GetComponent<SlidingPiece>();
                piece.Initalize(type.VehicleImage.texture,j,i,i == Size.x - 1 && j == Size.y - 1);
                _pieces[j, i] = piece;
            }
        }

        var oldPieces = new SlidingPiece[Math.Max(Size.x,Size.y)*2];
        var currentIndex = 0;
        for (int i = 0; i < 30; i++)
        {
            var piece = _pieces[_random.Next(0,Size.x),_random.Next(0,Size.y)];
            while (GetEmptyNeighbour(piece) == null && !oldPieces.ToList().Contains(piece))
            {
                piece = _pieces[_random.Next(0,Size.x),_random.Next(0,Size.y)];
            }

            oldPieces[currentIndex++ % (Math.Max(Size.x,Size.y)*2)] = piece;
            SwapPiece(piece,false);
        }
    }

    public void SwapPiece(SlidingPiece piece,bool check)
    {
        var empty = GetEmptyNeighbour(piece);
        if (empty == null)
        {
            return;
        }

        if (check)
        {
            OnPieceSwap?.Invoke(piece,empty);
            _mainManager.PlaySound(_mainManager.PieceSwap);
        }
        
        IsInteractable = false;
        
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
        
        

        if (check)
        {
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
                _mainManager.PlaySound(_mainManager.PuzzleSolved);
                var newType = CrateConfig.Instance.VehicleTypes[_random.Next(0,CrateConfig.Instance.VehicleTypes.Count-1)];
                while (newType == _currentType)
                {
                    newType = CrateConfig.Instance.VehicleTypes[_random.Next(0,CrateConfig.Instance.VehicleTypes.Count-1)];
                }
                StartCoroutine(Wait());
                IEnumerator Wait()
                {
                    FindObjectOfType<FleetPanel>().ReturnVehicle(new Vehicle(_currentType,true));
                    yield return new WaitForSeconds(1);
                    Initalize(newType);
                    IsInteractable = true;
                }
            }
            else
            {
                IsInteractable = true;
            }
        }
        else
        {
            IsInteractable = true;
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