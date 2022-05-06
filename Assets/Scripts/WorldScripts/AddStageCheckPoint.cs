using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStageCheckPoint : MonoBehaviour
{
    public bool CanAddCheckPoint;
    // Start is called before the first frame update
    void Start()
    {
        CanAddCheckPoint = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StageCheckPointManager _checkpointManager = Object.FindObjectOfType<StageCheckPointManager>();
            _checkpointManager.AddCheckpoint(this.gameObject);
            gameObject.SetActive(false);
        }
    }
}
