using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplinghookgun : MonoBehaviour
{
    public GameObject Hook;
    public GameObject player;
    public grapplinghook gph;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main_Character");
        gph = player.GetComponent<grapplinghook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gph.isHooked)
        {
            Hook.SetActive(false);
        }
        else
        {
            Hook.SetActive(true);
        }

    }
    private void OnEnable()
    {

        gph.usingthisgun = true;
    }
    private void OnDisable()
    {

        gph.usingthisgun = false;

    }
}
