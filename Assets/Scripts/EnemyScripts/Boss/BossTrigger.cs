using UnityEngine;

/// This is used to trigger the start of the boss fight.
public class BossTrigger : MonoBehaviour
{
    private BossController _bossController;

    /// Called before the first frame update.
    void Start()
    {
        _bossController = GameObject.Find("BossController").GetComponent<BossController>();
    }

    /// Called on collider entering the trigger.
    /** Triggers the boss if the fight has not already started. */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_bossController.OnWorm)
        {
            _bossController.Trigger();
        }
    }
}
