using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWorm : MonoBehaviour
{
    public float speed = 0.25f;
    float initialAngle;

    // Start is called before the first frame update
    void Start()
    {
        initialAngle = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + speed);
        
        if(transform.childCount < 1 || transform.eulerAngles.z - initialAngle > 180)
        {
            Destroy(gameObject);
        }
    }
}
