using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

///The TokenManger script handles most actions and variables surrounding the tokens.
public class TokenManager : MonoBehaviour
{
    public GameObject TokenUI;
    public GameObject TokenChange;
    public Transform TokenChangeInstantiatePosition;

    public GameObject DefaultToken;
    public bool DefaultTokenActive;
    public GameObject BatToken;
    public bool BatTokenActive;
    public GameObject ThornToken;
    public bool ThornTokenActive;
    public GameObject WormToken;
    public bool WormTokenActive;
    private bool _tokenUIactive;
    public List<GameObject> TokensOwned;
    public List<bool> TokensActive;


    [Header("Changeable values")]
    public float CustomPlayerMoveSpeed;
    private Movement PlayerMovement;
    public float GunLifeStealAmount;
    private Weapon _currentWeapon;
    public float CustomDashSpeed;
    public float CustomDashDuration;
    public int CustomBlockLifeSteal;
    private PlayerHealth _playerHealth;
    public float ShieldLifeSteal;
    private ShieldHP _shieldHP;

    [Header("IGNORE")]
    public int TokenIndex;
    public bool UsingTokenMenu;

    ///Awake is called when the script instance is being loaded.
    /**Awake is called either when an active GameObject that contains the script is initialized when a Scene loads, 
    or when a previously inactive GameObject is set to active, or after a GameObject created with Object.
    Instantiate is initialized. Use Awake to initialize variables or states before the application starts.*/
    /*! In this Awake we find and assign the player Movement script, PlayerHealth script and ShieldHP script.*/
    private void Awake()
    {
        PlayerMovement = Object.FindObjectOfType<Movement>();
        _playerHealth = Object.FindObjectOfType<PlayerHealth>();
        _shieldHP = Object.FindObjectOfType<ShieldHP>();
    }

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we make sure the UI elements are disables.
    We then add all the tokens that the player owns into a list. And then deactivates all of them and activates the default one.*/
    void Start()
    {
        TokenUI.SetActive(false);
        _tokenUIactive = false;

        TokensOwned.Add(DefaultToken);

        TokensOwned.Add(BatToken);

        foreach (GameObject tokens in TokensOwned)
        {
            tokens.SetActive(false);
        }
        TokensOwned[TokenIndex].SetActive(true);

        ActivateDefaultToken();
    }


    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! In the Update function if statements set the TokenUI on/off based on _tokenUIactive variable. This also allows the player to true/false the variable.
    If the TokenUI is enables the ingame timescale is set to 0, meaning everything is frozen in time. While if the TokenUI is disables everything goes back to normal time scale.*/
    void Update()
    {
        if (_tokenUIactive)
        {
            TokenUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            TokenUI.SetActive(false);
            Time.timeScale = 1f;
            UsingTokenMenu = false;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            _tokenUIactive = !_tokenUIactive;
            UsingTokenMenu = true;
        }
    }


    ///The NextToken allows the player to show the next token the player owns.
    public void NextToken()
    {
        foreach (GameObject tokens in TokensOwned)
        {
            tokens.SetActive(false);
        }

        if (TokenIndex + 1 < TokensOwned.Count)
        {
            TokenIndex += 1;
        }
        else if (TokenIndex + 1 == TokensOwned.Count)
        {
            TokenIndex = 0;
        }
        TokensOwned[TokenIndex].SetActive(true);
    }

    ///The PreviousToken allows the player to see the previous token the player owns.
    public void PreviousToken()
    {
        foreach (GameObject tokens in TokensOwned)
        {
            tokens.SetActive(false);
        }
        if (TokenIndex > 0)
        {
            TokenIndex -= 1;
        }
        else if (TokenIndex == 0)
        {
            TokenIndex = TokensOwned.Count - 1;
        }
        TokensOwned[TokenIndex].SetActive(true);
    }




    /*_________________FOR ENABLE/DISABLE OF THE TOKENS_________________*/

    ///The ActivateDefaultToken function activates the defualt token, and deacitvates every other token. Then different variables to match it's unique trait.
    public void ActivateDefaultToken()
    {
        DefaultTokenActive = true;
        BatTokenActive = false;
        ThornTokenActive = false;
        WormTokenActive = false;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        CustomPlayerMoveSpeed = 10;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        GunLifeStealAmount = 0;

        CustomDashSpeed = 20;
        CustomDashDuration = 0.2f;
        PlayerMovement.DashLength = CustomDashDuration;
        PlayerMovement.DashSpeed = CustomDashSpeed;

        CustomBlockLifeSteal = 99999999;
        _playerHealth.BlockLifeSteal = CustomBlockLifeSteal;

        ShieldLifeSteal = 9999999;
        _shieldHP.LifeStealAmount = ShieldLifeSteal;

        GameObject tokenchangeobj = Instantiate(TokenChange, TokenChangeInstantiatePosition.position, TokenChangeInstantiatePosition.rotation) as GameObject;
        tokenchangeobj.transform.SetParent(TokenChangeInstantiatePosition, true);
        tokenchangeobj.GetComponent<TokenChange>().txt.text = "Default Token Active";

    }
    ///The ActivateBatToken function activates the defualt token, and deacitvates every other token. Then different variables to match it's unique trait.
    public void ActivateBatToken()
    {
        _currentWeapon = Object.FindObjectOfType<WeaponController>().CurrentGun.GetComponent<Weapon>();

        DefaultTokenActive = false;
        BatTokenActive = true;
        ThornTokenActive = false;
        WormTokenActive = false;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        CustomPlayerMoveSpeed = 10;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        GunLifeStealAmount = 0.2f;

        CustomDashSpeed = 14;
        CustomDashDuration = 0.6f;
        PlayerMovement.DashLength = CustomDashDuration;
        PlayerMovement.DashSpeed = CustomDashSpeed;

        CustomBlockLifeSteal = 10;
        _playerHealth.BlockLifeSteal = CustomBlockLifeSteal;

        ShieldLifeSteal = 20;
        _shieldHP.LifeStealAmount = ShieldLifeSteal;

        GameObject tokenchangeobj = Instantiate(TokenChange, TokenChangeInstantiatePosition.position, TokenChangeInstantiatePosition.rotation);
        tokenchangeobj.transform.SetParent(TokenChangeInstantiatePosition, true);
        tokenchangeobj.GetComponent<TokenChange>().txt.text = "Bat Token Active";
    }

    ///The ActivateThornToken function activates the defualt token, and deacitvates every other token. Then different variables to match it's unique trait. NOT YET IMPLEMENTED
    public void ActivateThornToken()
    {
        DefaultTokenActive = false;
        BatTokenActive = false;
        ThornTokenActive = true;
        WormTokenActive = false;
    }
    ///The ActivateWormToken function activates the defualt token, and deacitvates every other token. Then different variables to match it's unique trait. NOT YET IMPLEMENTED
    public void ActivateWormToken()
    {
        DefaultTokenActive = false;
        BatTokenActive = false;
        ThornTokenActive = false;
        WormTokenActive = true;
    }
}
