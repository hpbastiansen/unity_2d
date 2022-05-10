using UnityEngine;
using UnityEngine.SceneManagement;

/// This script makes sure the grappling hook resets after stage 1 is completed.
public class Stage1End : MonoBehaviour
{
    private GrapplingHookController _grapplingHookController;

    /// Called before the first frame.
    void Start()
    {
        _grapplingHookController = FindObjectOfType<GrapplingHookController>();
    }

    /// Called on a collider entering the trigger on the gameobject.
    /** Set gun to default, release grapple and go back to The Hub. */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && _grapplingHookController.IsHooked == true)
        {
            WeaponController _weaponController = FindObjectOfType<WeaponController>();
            _weaponController.SetDefaultGun();
            _grapplingHookController.ReleaseGrapple();
            SceneManager.LoadScene("The_Hub");
        }
    }
}
