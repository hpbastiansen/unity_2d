using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Set a start position for the player e.g when scene is loaded.
public class SetPlayerPositionAndStageInfo : MonoBehaviour
{
    public StartInfo StartInfoScript;

    ///Function to be called whenever we want to set the player position equal to the scene's start position.
    public void SetInfo()
    {
        WeaponController _weaponController = Object.FindObjectOfType<WeaponController>();
        _weaponController.SetDefaultGun();
        Weapon _currentWeapon = Object.FindObjectOfType<WeaponController>().CurrentGun.GetComponent<Weapon>();
        PlayerHealth _playerHealth = Object.FindObjectOfType<PlayerHealth>();
        GameObject _player = GameObject.Find("Main_Character");
        GameObject _camera = GameObject.Find("Main Camera");
        _player.transform.position = new Vector3(StartInfoScript.StartPosition.x, StartInfoScript.StartPosition.y);
        _camera.transform.position = new Vector3(StartInfoScript.StartPosition.x, StartInfoScript.StartPosition.y);
        _currentWeapon.Ammo = StartInfoScript.Ammo;
        _playerHealth.MaxHP = StartInfoScript.Health;
        _playerHealth.CurrentHP = StartInfoScript.Health;
    }
    private void OnEnable()
    {
        SetInfo();
    }
}
