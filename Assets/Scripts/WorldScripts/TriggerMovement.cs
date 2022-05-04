using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour
{
    private bool _triggered;
    [SerializeField] private MovingBackground _background;
    [SerializeField] private MovingFloor _floor;
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
