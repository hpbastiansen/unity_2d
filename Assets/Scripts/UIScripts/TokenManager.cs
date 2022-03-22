using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

///The TokenManger script handles most actions and variables surrounding the tokens.
public class TokenManager : MonoBehaviour
{
    public GameObject TokenUI;
    public GameObject DefaultToken;
    public bool DefaultTokenActive;
    public GameObject BatToken;
    public bool BatTokenActive;
    public bool TokenUIactive;
    public List<GameObject> TokensOwned;
    public List<bool> TokensActive;
    private UIManager _myUIManager;
    public string ShortInfo;
    public string DashInfo;
    public string ShieldInfo;
    public string BulletInfo;
    public string CounterInfo;
    public string MovementInfo;

    public Text ShortInfoText;
    public Text DashInfoText;
    public Text ShieldInfoText;
    public Text BulletInfoText;
    public Text CounterInfoText;
    public Text MovementInfoText;


    [Header("Changeable values")]
    public float CustomPlayerMoveSpeed;
    public float CustomPlayerJumpHeight;
    private Movement PlayerMovement;
    public float GunLifeStealAmount;
    public Weapon CurrentWeapon;
    public float CustomDashSpeed;
    public float CustomDashDuration;
    public int CustomBlockLifeSteal;
    public int CustomBlockLifeStealCooldown;
    public int CustomBlockLifeStealActiveTime;
    private PlayerHealth _playerHealth;
    public float ShieldLifeSteal;
    private ShieldHP _shieldHP;

    private string _weaponAccuracy;

    [Header("IGNORE")]
    public int TokenIndex;
    public bool UsingTokenMenu;
    private string _spacechar = " __ ";
    private UITest _checkUI;

    ///Awake is called when the script instance is being loaded.
    /**Awake is called either when an active GameObject that contains the script is initialized when a Scene loads, 
    or when a previously inactive GameObject is set to active, or after a GameObject created with Object.
    Instantiate is initialized. Use Awake to initialize variables or states before the application starts.*/
    /*! In this Awake we find and assign the necessary sctipt in the scene.*/
    private void Awake()
    {
        PlayerMovement = Object.FindObjectOfType<Movement>();
        _playerHealth = Object.FindObjectOfType<PlayerHealth>();
        _shieldHP = Object.FindObjectOfType<ShieldHP>();
        _myUIManager = GameObject.FindObjectOfType<UIManager>();

    }

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we make sure the UI elements are disables.
    We then add all the tokens that the player owns into a list. And then deactivates all of them and activates the default one.*/
    void Start()
    {

        _checkUI = Object.FindObjectOfType<UITest>();

        TokenUI = GameObject.Find("TokenUI");
        //TokenUI.SetActive(false);
        TokenUIactive = false;

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
        if (TokenUIactive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_checkUI.IsPointerOverUIElement() == false)
                {
                    TokenUIactive = !TokenUIactive;
                }
            }
        }
        else
        {
            UsingTokenMenu = false;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TokenUIactive = !TokenUIactive;
            UsingTokenMenu = true;
        }

        DashInfo = "Speed: " + CustomDashSpeed + _spacechar + "Duration: " + CustomDashDuration + "s";
        DashInfoText.text = DashInfo.ToString();

        ShieldInfo = "Lifesteal: " + ShieldLifeSteal + _spacechar + "Health: " + _shieldHP.MaxHP + _spacechar + "Cooldown: " + _shieldHP.RechargeTimer + "s";
        ShieldInfoText.text = ShieldInfo.ToString();

        BulletInfo = "                    Damage: " + CurrentWeapon.Damage + _spacechar + "Lifesteal: " + CurrentWeapon.LifeSteal + _spacechar + "Speed: " +
        CurrentWeapon.BulletSpeed + _spacechar + "Accuracy: " + _weaponAccuracy + _spacechar + "Firerate: " + CurrentWeapon.Firerate + _spacechar + "FullAuto: " + CurrentWeapon.FullAuto + _spacechar
        + "Knockback: " + CurrentWeapon.BulletWeight + _spacechar + "Type: " + CurrentWeapon.WeaponType;
        BulletInfoText.text = BulletInfo.ToString();

        CounterInfo = "Lifesteal: " + CustomBlockLifeSteal + _spacechar + "Cooldown: " + CustomBlockLifeStealCooldown + "s" + _spacechar + "Activetime: " + CustomBlockLifeStealActiveTime + "s";
        CounterInfoText.text = CounterInfo.ToString();

        MovementInfo = "Runspeed: " + CustomPlayerMoveSpeed + _spacechar + "Jump height: " + CustomPlayerJumpHeight / 1000;
        MovementInfoText.text = MovementInfo.ToString();

        ShortInfoText.text = ShortInfo.ToString();
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

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        ShortInfo = "Default token is obtained at the start of the game for free. The values in the token is the default values for all the variables. Other tokens can however change these.";

        CustomPlayerMoveSpeed = 10;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        CustomPlayerJumpHeight = 10000;
        PlayerMovement.JumpForce = CustomPlayerJumpHeight;

        GunLifeStealAmount = 0;

        CustomDashSpeed = 40;
        CustomDashDuration = 0.2f;
        PlayerMovement.DashLength = CustomDashDuration;
        PlayerMovement.DashSpeed = CustomDashSpeed;

        CustomBlockLifeSteal = 0;
        _playerHealth.BlockLifeSteal = CustomBlockLifeSteal;
        CustomBlockLifeStealCooldown = 3;
        _playerHealth.BlockCooldownTime = CustomBlockLifeStealCooldown;
        CustomBlockLifeStealActiveTime = 1;
        _playerHealth.BlockActiveTime = CustomBlockLifeStealActiveTime;

        ShieldLifeSteal = 0;
        _shieldHP.LifeStealAmount = ShieldLifeSteal;

        _weaponAccuracy = "decent";
    }
    ///The ActivateBatToken function activates the defualt token, and deacitvates every other token. Then different variables to match it's unique trait.
    public void ActivateBatToken()
    {
        DefaultTokenActive = false;
        BatTokenActive = true;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        ShortInfo = "This is another token!";

        CustomPlayerMoveSpeed = 10;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        GunLifeStealAmount = 0.2f;

        CustomDashSpeed = 20;
        CustomDashDuration = 0.6f;
        PlayerMovement.DashLength = CustomDashDuration;
        PlayerMovement.DashSpeed = CustomDashSpeed;

        CustomBlockLifeSteal = 3;
        _playerHealth.BlockLifeSteal = CustomBlockLifeSteal;
        CustomBlockLifeStealCooldown = 2;
        _playerHealth.BlockCooldownTime = CustomBlockLifeStealCooldown;
        CustomBlockLifeStealActiveTime = 1;
        _playerHealth.BlockActiveTime = CustomBlockLifeStealActiveTime;


        ShieldLifeSteal = 1;
        _shieldHP.LifeStealAmount = ShieldLifeSteal;
        _weaponAccuracy = "decent";

    }

}
