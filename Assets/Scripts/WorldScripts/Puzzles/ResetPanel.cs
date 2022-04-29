using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPanel : MonoBehaviour
{
    [SerializeField] private bool _resetPosition;
    [SerializeField] private bool _resetRotation;
    [SerializeField] private GameObject[] _puzzleObjects;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                ResetObjects();
            }
        }
    }

    private void ResetObjects()
    {
        foreach (GameObject _object in _puzzleObjects)
        {
            PuzzleObject _puzzleObject = _object.GetComponent<PuzzleObject>();
            if (_resetPosition) _object.transform.position = _puzzleObject.InitialPosition;
            if (_resetRotation) _object.transform.rotation = _puzzleObject.InitialRotation;
        }
    }
}
