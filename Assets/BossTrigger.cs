using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private BossController _bossController;
    // Start is called before the first frame update
    void Start()
    {
        _bossController = GameObject.Find("BossController").GetComponent<BossController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_bossController.OnWorm)
        {
            _bossController.Trigger();
        }
    }
}
