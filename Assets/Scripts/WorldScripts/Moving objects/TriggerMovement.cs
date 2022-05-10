using UnityEngine;

/// This script triggers the moving background and floor for the boss sequence in stage 4.
public class TriggerMovement : MonoBehaviour
{
    private bool _triggered;
    [SerializeField] private MovingBackground _background;
    [SerializeField] private MovingFloor _floor;

    /// Called on a collider entering the trigger on the gameobject.
    /** Once the player enters the trigger, trigger the background and floor movement. */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_triggered) return;

        if(collision.gameObject.CompareTag("Player"))
        {
            _triggered = true;
            _background.Triggered = true;
            _floor.Triggered = true;
        }
    }
}
