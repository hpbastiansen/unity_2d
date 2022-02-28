using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetImprovedCursor : MonoBehaviour
{
    public Vector3 centerPt;
    public float radius;
    public GameObject player;
    public Movement mv;


    void Start()
    {
        player = GameObject.Find("Main_Character");
        mv = GameObject.Find("Main_Character").GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
