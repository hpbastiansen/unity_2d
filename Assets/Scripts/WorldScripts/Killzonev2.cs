using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzonev2 : MonoBehaviour
{
    public bool Kill;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //PlayerHealth _playerHP = Object.FindObjectOfType<PlayerHealth>();
            //_playerHP.TakeDamage(99999, 0);
            GameObject _player = GameObject.Find("Main_Character");
            StageCheckPointManager _checkpointManager = Object.FindObjectOfType<StageCheckPointManager>();
            if (Kill)
            {
                _player.GetComponent<PlayerHealth>().TakeDamage(9999, 0);
            }
            if (Kill == false)
            {
                _player.transform.position = _checkpointManager.CheckPoints[_checkpointManager.CheckPoints.Count - 1].transform.position;
            }
            //_checkpointManager.CheckPoints[_checkpointManager.CheckPoints.Count - 1].GetComponent<AddCheckPoint>().enabled = false;

        }
    }
}