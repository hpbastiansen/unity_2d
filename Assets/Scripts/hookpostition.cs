using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hookpostition : MonoBehaviour
{
    public GameObject firepoint;
    public GameObject arm;
    public Movement mv;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()

    {
        if (mv.facingRight == true)
        {
            var armrot = arm.transform.localEulerAngles.z;
            transform.localRotation = Quaternion.Euler(0, 0, armrot * -1);
            transform.position = firepoint.transform.position;
        }
        else
        {
            var armrot = arm.transform.localEulerAngles.z;
            transform.localRotation = Quaternion.Euler(0, 0, armrot * -1);
            transform.position = firepoint.transform.position;

        }


    }
}
