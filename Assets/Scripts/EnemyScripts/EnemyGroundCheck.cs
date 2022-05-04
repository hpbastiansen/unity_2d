using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    [SerializeField] private LayerMask _whatToHit;
    [SerializeField] private bool _checkGround;
    [SerializeField] private bool _checkRoof;
    [SerializeField] private bool _checkFront;
    [SerializeField] private bool _checkGap;

    private bool _isEmpty;

    // Start is called before the first frame update
    void Start()
    {
        _enemyMovement = transform.parent.GetComponent<EnemyMovement>();
    }

    private void FixedUpdate()
    {
        _isEmpty = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _isEmpty = false;
        if(_checkGap) _enemyMovement.GapInFront = false;

        if((_whatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if (_checkGround) _enemyMovement.IsTouchingGround = true;
            if (_checkRoof) _enemyMovement.IsCloseToRoof = true;
            if (_checkFront) _enemyMovement.BlockedInFront = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((_whatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if (_checkGround) _enemyMovement.IsTouchingGround = false;
            if (_checkRoof) _enemyMovement.IsCloseToRoof = false;
            if (_checkFront) _enemyMovement.BlockedInFront = false;
            if (_checkGap) _enemyMovement.GapInFront = true;
        }
    }
}
