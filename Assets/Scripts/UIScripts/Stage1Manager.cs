using UnityEngine;

/// Manager responsible for the first stage. Used for giving the player the grappling hook upon reaching it.
public class Stage1Manager : MonoBehaviour
{
    public bool GottenGrapplingHook;
    public GameObject EndScene;
    public GameObject HoleDiaTrigger;
    public GameObject GrapplingHook;
    public WeaponController WeaponControllerScript;

    /// Called before the first frame.
    /** Prepares the stage, disabling the end trigger until the player gets the grappling hook. */
    void Start()
    {
        EndScene.SetActive(false);
        HoleDiaTrigger.SetActive(false);
        WeaponControllerScript = FindObjectOfType<WeaponController>();
        GrapplingHook.SetActive(true);
    }

    /// Allows the player to use the grappling hook. Enables the ending trigger of the stage.
    public void GiveGrapplingHook()
    {
        GottenGrapplingHook = true;
        EndScene.SetActive(true);
        HoleDiaTrigger.SetActive(true);
        WeaponControllerScript.GottenGrapplingHook = true;
        GrapplingHook.SetActive(false);
    }
}
