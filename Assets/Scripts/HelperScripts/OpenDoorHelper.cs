using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorHelper : MonoBehaviour
{
    private Puzzle1 _puzzle1;
    // Start is called before the first frame update
    void Start()
    {
        _puzzle1 = Object.FindObjectOfType<Puzzle1>();
    }

    public void OpenDoor()
    {
        _puzzle1.OpenDoor1();
    }
}
