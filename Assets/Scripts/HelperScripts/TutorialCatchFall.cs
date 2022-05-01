using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCatchFall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.transform.parent.gameObject.transform.position = new Vector2(-36.245f, 8.96125f);
        }
    }
}
