using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerSetpiece : MonoBehaviour
{
    private bool _hasBeenTriggered = false;
    private BossController _controller;

    private void Start()
    {
        _controller = GameObject.Find("BossController").GetComponent<BossController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasBeenTriggered) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Section done, ready for next.");
            _controller.SectionsDone++;
            _controller.ReadyForNextSection = true;
            _hasBeenTriggered = true;
        }
    }
}
