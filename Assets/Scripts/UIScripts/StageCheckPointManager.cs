using System.Collections.Generic;
using UnityEngine;

/// This script manages the checkpoints for a stage. On touching a new checkpoint, the previous ones gets disabled.
public class StageCheckPointManager : MonoBehaviour
{
    public List<GameObject> CheckPoints;

    /// Called before the first frame.
    /** On load, sets the player's position to the current checkpoint. */
    void Start()
    {
        ES3AutoSaveMgr.Current.Load();
        GameObject _player = GameObject.Find("Main_Character");
        _player.transform.position = CheckPoints[CheckPoints.Count - 1].transform.position;
        CheckPoints[CheckPoints.Count - 1].SetActive(false);
    }

    /// Called every frame.
    /** Listens for KeyDown events for O and X, clearing all checkpoints and resetting to the previous one respectively. */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ClearCheckPoints();
            ES3AutoSaveMgr.Current.Save();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject _player = GameObject.Find("Main_Character");
            _player.transform.position = CheckPoints[CheckPoints.Count - 1].transform.position;
        }
    }

    /// Adds a new checkpoint to the checkpoint manager.
    public void AddCheckpoint(GameObject _newCheckPoint)
    {
        CheckPoints.Add(_newCheckPoint);
        ES3AutoSaveMgr.Current.Save();
        foreach (GameObject _checkPoint in CheckPoints)
        {
            _checkPoint.SetActive(false);
        }
    }

    /// Clears all checkpoints except the first one.
    public void ClearCheckPoints()
    {
        foreach (var checkpoint in CheckPoints)
        {
            checkpoint.SetActive(true);
        }
        if (CheckPoints.Count > 1)
            CheckPoints.RemoveRange(1, CheckPoints.Count - 1);
    }
}
