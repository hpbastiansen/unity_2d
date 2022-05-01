using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnewayPlatform : MonoBehaviour
{
    private PlatformEffector2D _effector;
    // Start is called before the first frame update
    void Start()
    {
        _effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_effector.rotationalOffset);
        if(Input.GetKeyDown(KeyCode.S))
        {
            _effector.rotationalOffset = 180;
        } 
        else
        {
            _effector.rotationalOffset = 0;
        }
    }
}
