using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    [SerializeField] private LayerMask _whatToHit;
    [SerializeField] private bool _checkForRoof;
    // Start is called before the first frame update
    void Start()
    {
        _enemyMovement = transform.parent.GetComponent<EnemyMovement>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if((_whatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if(!_checkForRoof)
            {
                _enemyMovement.IsTouchingGround = true;
            } 
            else
            {
                _enemyMovement.IsCloseToRoof = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((_whatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if(!_checkForRoof)
            {
                _enemyMovement.IsTouchingGround = false;
            }
            else
            {
                _enemyMovement.IsCloseToRoof = false;
            }
        }
    }
}
