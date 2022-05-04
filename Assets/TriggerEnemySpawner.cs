using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemySpawner : MonoBehaviour
{
    [SerializeField] WaveController _waveController;
    private bool _hasBeenTriggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenTriggered) return;

        if(collision.gameObject.CompareTag("Player"))
        {
            _hasBeenTriggered = true;
            _waveController.Trigger();
        }
    }
}
