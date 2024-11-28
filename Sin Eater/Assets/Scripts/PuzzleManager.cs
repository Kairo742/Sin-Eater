using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<PuzzlePiece> _puzzlePieces = new List<PuzzlePiece>();
    private int _bools;
    [SerializeField] private GameObject _door;
    public static PuzzleManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public void UpdateList()
    {
        _bools++;

        CheckList();
    }

    private void CheckList()
    {
        if(_bools == _puzzlePieces.Count)
        {
            FinishedPuzzle();
        }
    }



    private void FinishedPuzzle()
    {
        Destroy(_door);
    }
}
