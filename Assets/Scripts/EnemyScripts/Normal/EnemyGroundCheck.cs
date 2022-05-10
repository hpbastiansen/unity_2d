using UnityEngine;

/// This script checks if the enemy is on the ground, close to the roof, or if they have an obstacle or gap in front of them.
public class EnemyGroundCheck : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    [SerializeField] private LayerMask _whatToHit;
    [SerializeField] private bool _checkGround;
    [SerializeField] private bool _checkRoof;
    [SerializeField] private bool _checkFront;
    [SerializeField] private bool _checkGap;

    /// Called before the first frame.
    void Start()
    {
        _enemyMovement = transform.parent.GetComponent<EnemyMovement>();
    }

    /// Called every frame a collider is inside the trigger on the gameobject.
    /** If the selected trigger is blocked, set the corresponding bool in the EnemyMovement script. */
    private void OnTriggerStay2D(Collider2D other)
    {
        if(_checkGap) _enemyMovement.GapInFront = false;

        if((_whatToHit & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            if (_checkGround) _enemyMovement.IsTouchingGround = true;
            if (_checkRoof) _enemyMovement.IsCloseToRoof = true;
            if (_checkFront) _enemyMovement.BlockedInFront = true;
        }
    }

    /// Called on a collider exiting the trigger on the gameobject.
    /** When the selected trigger is no longer blocked, set the corresponding bool in the EnemyMovement script. */
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
