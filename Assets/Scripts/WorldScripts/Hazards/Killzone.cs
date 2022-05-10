using UnityEngine;

/// This script is placed on a trigger that should kill or reset a player to their latest checkpoint.
public class Killzone : MonoBehaviour
{
    public bool Kill;

    /// Called on a collider entering the trigger on the gameobject.
    /** If the collider belongs to the player, if set to kill, deal 9999 damage. Otherwise, teleport them to the latest checkpoint. */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObject _player = GameObject.Find("Main_Character");
            StageCheckPointManager _checkpointManager = FindObjectOfType<StageCheckPointManager>();
            if (Kill)
            {
                _player.GetComponent<PlayerHealth>().TakeDamage(9999, 0);
            }
            else
            {
                _player.transform.position = _checkpointManager.CheckPoints[_checkpointManager.CheckPoints.Count - 1].transform.position;
            }
        }
    }
}
