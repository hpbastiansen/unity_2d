using UnityEngine;

/// This script is responsible for informing the block puzzle controller the block it's placed on is destroyed.
public class CrackedBlock : MonoBehaviour
{
    private BlockPuzzle _blockPuzzle;
    [SerializeField] private bool _correctBlock;

    /// Called before the first frame.
    private void Start()
    {
        _blockPuzzle = GameObject.Find("Puzzle Controller").GetComponent<BlockPuzzle>();
    }

    /// Called when a bullet hits this block. Deactivates the object.
    /** If the block is marked as correct, increase the correct blocks on the puzzle controller. 
        Otherwise, set the puzzle as failed. */
    public void OnHit()
    {
        if (_correctBlock) _blockPuzzle.IncreaseCorrectBlocks();
        else _blockPuzzle.PuzzleFailed = true;
        gameObject.SetActive(false);
    }
}
