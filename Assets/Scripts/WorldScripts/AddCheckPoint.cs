using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCheckPoint : MonoBehaviour
{

    private CheckPointManager _checkpointManager;
    public string CheckPointToAdd;
    public bool W1;
    public bool W2;
    public bool W3;
    private void Start()
    {
        _checkpointManager = Object.FindObjectOfType<CheckPointManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (W1)
            {
                if (!_checkpointManager.W1Scenes.Contains(CheckPointToAdd))
                {
                    _checkpointManager.AddCheckPointW1(CheckPointToAdd);
                }
            }
            if (W2)
            {
                if (!_checkpointManager.W2Scenes.Contains(CheckPointToAdd))
                {
                    _checkpointManager.AddCheckPointW2(CheckPointToAdd);
                }
            }
            if (W3)
            {
                if (!_checkpointManager.W3Scenes.Contains(CheckPointToAdd))
                {
                    _checkpointManager.AddCheckPointW3(CheckPointToAdd);
                }
            }

        }
    }
}
