using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedBlock : MonoBehaviour
{
    private BlockPuzzle _blockPuzzle;
    [SerializeField] private bool _correctBlock;

    void Start()
    {
        _blockPuzzle = GameObject.Find("Puzzle Controller").GetComponent<BlockPuzzle>();
    }

    public void OnHit()
    {
        if (_correctBlock) _blockPuzzle.IncreaseCorrectBlocks();
        else _blockPuzzle.PuzzleFailed = true;
        gameObject.SetActive(false);
    }
}
