using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUntilDestroyed : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private GameObject _weakpoint;
    private bool _triggered = false;

    // Update is called once per frame
    void Update()
    {
        if (_triggered) return;

        if (_weakpoint == null)
        {
            _collider.enabled = false;
            _triggered = true;
        }
    }
}
