using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    public bool GottenGrapplingHook;
    public GameObject HoleKillzone;
    public GameObject HoleDiaTrigger;
    public WeaponController WeaponControllerScript;


    // Start is called before the first frame update
    void Start()
    {
        GottenGrapplingHook = false;
        HoleKillzone.SetActive(true);
        HoleDiaTrigger.SetActive(false);
        WeaponControllerScript = Object.FindObjectOfType<WeaponController>();
    }
    public void GiveGrapplingHook()
    {
        GottenGrapplingHook = true;
        HoleKillzone.SetActive(true);
        HoleDiaTrigger.SetActive(true);
        WeaponControllerScript.GottenGrapplingHook = true;
    }
}
