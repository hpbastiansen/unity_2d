using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPivot : MonoBehaviour
{
    public GameObject myPlayer;
    private Vector3 mouse_pos;
    public Movement myplayer;
    public Transform target;
    private Vector3 object_pos;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        myplayer = GameObject.Find("Main_Character").GetComponent<Movement>();

    }

    // Update is called once per frame
    void Update()
    {

        //http://answers.unity.com/answers/130142/view.html
        mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        object_pos = Camera.main.WorldToScreenPoint(target.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (!myplayer.facingRight)
        {
            transform.rotation = Quaternion.Euler(-180, 0, -angle);
        }
    }

}
