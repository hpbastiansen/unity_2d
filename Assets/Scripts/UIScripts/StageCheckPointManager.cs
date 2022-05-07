using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCheckPointManager : MonoBehaviour
{
    public List<GameObject> CheckPoints;

    // Start is called before the first frame update
    void Start()
    {
        ES3AutoSaveMgr.Current.Load();
        GameObject _player = GameObject.Find("Main_Character");
        _player.transform.position = CheckPoints[CheckPoints.Count - 1].transform.position;
        CheckPoints[CheckPoints.Count - 1].SetActive(false);
    }

    // Update is called once per frame
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
    public void AddCheckpoint(GameObject gobject)
    {
        CheckPoints.Add(gobject);
        ES3AutoSaveMgr.Current.Save();
        foreach (var checkpoint in CheckPoints)
        {
            checkpoint.SetActive(false);
        }
    }

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
