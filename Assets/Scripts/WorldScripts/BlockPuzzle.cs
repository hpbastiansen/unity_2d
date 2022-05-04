using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPuzzle : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private GameObject[] _blocks;
    public int _correctBlocks;
    [SerializeField] private int _totalCorrectBlocks;
    public bool PuzzleFailed = false;
    public bool PuzzleSolved = false;

    public void ResetPuzzle()
    {
        foreach(GameObject _block in _blocks)
        {
            if(!_block.activeSelf)
            {
                _block.SetActive(true);
            }
        }

        PuzzleFailed = false;
        _correctBlocks = 0;
    }

    public void IncreaseCorrectBlocks()
    {
        if (PuzzleFailed) return;

        _correctBlocks++;
        if(_correctBlocks >= _totalCorrectBlocks)
        {
            PuzzleSolved = true;
            _door.Triggered = true;
        }
    }
}
