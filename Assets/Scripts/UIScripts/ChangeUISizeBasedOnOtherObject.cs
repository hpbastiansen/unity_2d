using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUISizeBasedOnOtherObject : MonoBehaviour
{

    public RectTransform OtherObject;
    private RectTransform myRect;
    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {



    }
}
