using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.eulerAngles = Vector3.zero;
    }
}
