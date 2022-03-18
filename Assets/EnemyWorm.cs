using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWorm : MonoBehaviour
{
    public float speed = 0.1f;
    public string direction = "left";
    float initialHeight;
    Transform enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.Find("Worm");
        initialHeight = enemy.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, direction == "right" ? transform.eulerAngles.z - speed : transform.eulerAngles.z + speed);
        
        if(transform.childCount < 1 || enemy.transform.position.y < initialHeight - 1)
        {
            Destroy(gameObject);
        }
        
    }
}
