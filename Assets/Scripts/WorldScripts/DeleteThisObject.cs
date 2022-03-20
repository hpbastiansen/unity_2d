using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteThisObject : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
