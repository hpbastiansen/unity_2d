using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


///The WeaponController keeps track of available weapons and which weapons player is currently using.
public class WeaponController : MonoBehaviour
{
    int _totalWeapons = 1;
    public int CurrentWeaponIndex;

    public GameObject[] Weapons;
    public GameObject WeaponHolderObject;
    public GameObject CurrentGun;
    public bool UsingShield;
    public GameObject ShieldObject;
    public float ShieldRechargeTimer;
    private TokenManager _tokenManager;
    private Movement _playerMovement;

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we force the player not to start with the shield. 
    Next we are indexing all the weapons on the player. These can be found as child objects under the "WeaponHolderObject" gameObject. 
    Then we set the first gun as the current gun, and activates it, and deactivates all the others.*/
    void Start()
    {
        _playerMovement = GetComponent<Movement>();
        UsingShield = false;
        _totalWeapons = WeaponHolderObject.transform.childCount;
        Weapons = new GameObject[_totalWeapons];

        for (int i = 0; i < _totalWeapons; i++)
        {
            Weapons[i] = WeaponHolderObject.transform.GetChild(i).gameObject;
            Weapons[i].SetActive(false);
        }

        Weapons[0].SetActive(true);
        CurrentGun = Weapons[0];
        CurrentWeaponIndex = 0;

        _tokenManager = Object.FindObjectOfType<TokenManager>();
        _tokenManager.CurrentWeapon = CurrentGun.GetComponent<Weapon>();
    }

    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! In the Update function we are allowing the player to cycle between the weapons we just indexed. The player can increase the current weapon index with the key "3",
    and decrease with the key "2".
    Lastly we allow the player to switch between using the weapons and the shield.*/
    void Update()
    {
        if (_playerMovement.NoControl) return;
        ////////////////////////////////////////////////////////////////
        // https://www.youtube.com/watch?v=-YISSX16NwE&ab_channel=TheGameGuy
        ////////////////////////////////////////////////////////////////

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //next Weapon
            if (CurrentWeaponIndex == 0)
            {
                Weapons[CurrentWeaponIndex].SetActive(false);
                CurrentWeaponIndex += 1;
                Weapons[CurrentWeaponIndex].SetActive(true);
                CurrentGun = Weapons[CurrentWeaponIndex];
            }
            else if (CurrentWeaponIndex == 1)
            {
                Weapons[CurrentWeaponIndex].SetActive(false);
                CurrentWeaponIndex = 0;
                Weapons[CurrentWeaponIndex].SetActive(true);
                CurrentGun = Weapons[CurrentWeaponIndex];
            }
        }
        //allow player to use shield instead of weapons.
        if (Input.GetMouseButtonDown(2) && ShieldRechargeTimer <= 0)
        {
            UsingShield = !UsingShield;
        }
        if (UsingShield == true)
        {
            ShieldObject.SetActive(true);
            WeaponHolderObject.SetActive(false);
        }
        else if (UsingShield == false)
        {
            ShieldObject.SetActive(false);
            WeaponHolderObject.SetActive(true);
        }
        if (ShieldRechargeTimer > 0)
        {
            ShieldRechargeTimer -= Time.deltaTime;
            ShieldObject.GetComponent<ShieldHP>().HP = ShieldObject.GetComponent<ShieldHP>().MaxHP;

        }
    }

    ///Function to disable all guns and enable the first one.
    public void SetDefaultGun()
    {
        for (int i = 0; i < _totalWeapons; i++)
        {
            Weapons[i] = WeaponHolderObject.transform.GetChild(i).gameObject;
            Weapons[i].SetActive(false);
        }

        Weapons[0].SetActive(true);
        CurrentGun = Weapons[0];
        CurrentWeaponIndex = 0;
    }
}
