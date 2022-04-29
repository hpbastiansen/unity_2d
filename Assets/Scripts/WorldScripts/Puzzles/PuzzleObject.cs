using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    [HideInInspector] public Vector3 InitialPosition;
    [HideInInspector] public Quaternion InitialRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = gameObject.transform.position;
        InitialRotation = gameObject.transform.rotation;
    }
}
