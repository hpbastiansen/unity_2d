using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class grapplinghookgun : MonoBehaviour
{
    public GameObject Hook;
    public GameObject player;
    public grapplinghook gph;
    public Sprite img;
    private Image imgUI;
    private Text ammotxt;
    private string Ammo = "∞";

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main_Character");
        gph = player.GetComponent<grapplinghook>();
        ammotxt = GameObject.Find("AmmoTextUI").GetComponent<Text>();
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
        ammotxt.text = Ammo.ToString();

    }
    private void OnEnable()
    {
        imgUI = GameObject.Find("currentWepImg").GetComponent<Image>();

        gph.usingthisgun = true;
        imgUI.sprite = img;
        ammotxt = GameObject.Find("AmmoTextUI").GetComponent<Text>();

    }
    private void OnDisable()
    {

        gph.usingthisgun = false;

    }
}
