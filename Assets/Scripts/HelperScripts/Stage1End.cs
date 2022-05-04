using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1End : MonoBehaviour
{
    private GrapplingHookController _grapplingHookController;
    void Start()
    {
        _grapplingHookController = Object.FindObjectOfType<GrapplingHookController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && _grapplingHookController.IsHooked == true)
        {
            WeaponController _weaponController = Object.FindObjectOfType<WeaponController>();
            _weaponController.SetDefaultGun();
            _grapplingHookController.ExternalUnhook();
            SceneManager.LoadScene("The_Hub");
        }
    }
}
