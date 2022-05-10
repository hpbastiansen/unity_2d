using UnityEngine;

/// This script holds the logic for the block puzzle on stage 4.
public class BlockPuzzle : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private GameObject[] _blocks;
    public int _correctBlocks;
    [SerializeField] private int _totalCorrectBlocks;
    public bool PuzzleFailed = false;
    public bool PuzzleSolved = false;

    /// This method resets the puzzle completely, reactivating all puzzle blocks.
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

    /// If the block hit was correct, this method is called. If all correct blocks are hit, open the door.
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
