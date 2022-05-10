using UnityEngine;

/// This script opens a puzzle door on stage 3 when its method is called.
public class OpenDoorHelper : MonoBehaviour
{
    private Puzzle1 _puzzle1;

    /// Called before the first frame.
    void Start()
    {
        _puzzle1 = FindObjectOfType<Puzzle1>();
    }

    /// Open the puzzle door.
    public void OpenDoor()
    {
        _puzzle1.OpenDoor1();
    }
}
