using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    public bool GottenGrapplingHook;
    public GameObject EndScene;
    public GameObject HoleDiaTrigger;
    public GameObject GrapplingHook;
    public WeaponController WeaponControllerScript;


    // Start is called before the first frame update
    void Start()
    {
        EndScene.SetActive(false);
        HoleDiaTrigger.SetActive(false);
        WeaponControllerScript = Object.FindObjectOfType<WeaponController>();
        GrapplingHook.SetActive(true);
    }
    public void GiveGrapplingHook()
    {
        GottenGrapplingHook = true;
        EndScene.SetActive(true);
        HoleDiaTrigger.SetActive(true);
        WeaponControllerScript.GottenGrapplingHook = true;
        GrapplingHook.SetActive(false);
    }
}
