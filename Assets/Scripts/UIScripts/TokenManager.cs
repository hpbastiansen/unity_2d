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
    public GameObject CactusToken;
    public bool CactusTokenActive;
    public GameObject RevloverToken;
    public bool RevloverTokenActive;
    public GameObject WormToken;
    public bool WormTokenActive;
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
    private UIManager _uiManager;


    [Header("Changeable values")]
    public float CustomPlayerMoveSpeed;
    public float CustomPlayerJumpHeight;
    private Movement PlayerMovement;
    public float GunLifeStealAmount;
    public Weapon CurrentWeapon;
    public float CustomDashSpeed;
    public float CustomDashDuration;
    public float CustomDashCooldown;
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


    [Header("CactusToken")]
    public GameObject CactusSplinter;
    public WeaponController WeaponControllerScript;
    public bool ReadyToGiveToken;
    public int CactiDestoyed;

    [Header("WormToken")]
    public int ShrubsDestoyed;



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
        WeaponControllerScript = Object.FindObjectOfType<WeaponController>();
        _uiManager = Object.FindObjectOfType<UIManager>();
        ReadyToGiveToken = true;
    }

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we make sure the UI elements are disables.
    We then add all the tokens that the player owns into a list. And then deactivates all of them and activates the default one.*/
    void Start()
    {
        _checkUI = Object.FindObjectOfType<UITest>();
        TokenUI = GameObject.Find("TokenUI");
        TokenUIactive = false;

        foreach (GameObject tokens in TokensOwned)
        {
            tokens.SetActive(false);
        }
        ActivateDefaultToken();
        TokensOwned[TokenIndex].SetActive(true);
        UsingTokenMenu = true;
        UsingTokenMenu = false;

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
        if (Input.GetKeyDown(KeyCode.T) && _uiManager.UsingMainMenu == false)
        {
            TokenUIactive = !TokenUIactive;
            UsingTokenMenu = true;
        }

        DashInfo = "Speed: " + CustomDashSpeed + _spacechar + "Duration: " + CustomDashDuration + "s";
        DashInfoText.text = DashInfo.ToString();

        ShieldInfo = "Lifesteal: " + ShieldLifeSteal + _spacechar + "Health: " + _shieldHP.MaxHP + _spacechar + "Cooldown: " + _shieldHP.RechargeTimer + "s";
        ShieldInfoText.text = ShieldInfo.ToString();

        BulletInfo = "            Damage: " + CurrentWeapon.Damage + _spacechar + "Lifesteal: " + CurrentWeapon.LifeSteal + _spacechar + "Speed: " +
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

    public IEnumerator CactusTokenDash()
    {
        for (int i = 0; i < 5; i++)
        {
            if (PlayerMovement.IsDashing == true)
            {
                yield return new WaitForSeconds(0.1f);
                float _randomSpread = Random.Range(-1, 1);
                GameObject thebullet = Instantiate(CactusSplinter, CurrentWeapon.FirePoint.position, PlayerMovement.ArmPivotGameObject.transform.rotation);
                thebullet.transform.Rotate(0, 0, _randomSpread);
                thebullet.GetComponent<CactusSplinter>().CameraShakeStrength = 0;
                thebullet.GetComponent<CactusSplinter>().TimeToLive = 1f;
                thebullet.GetComponent<CactusSplinter>().WhatToHit = CurrentWeapon.WhatToHit;
                thebullet.GetComponent<CactusSplinter>().BulletSpeed = CustomDashSpeed * 1.5f;
                thebullet.GetComponent<CactusSplinter>().Damage = 5;
                thebullet.GetComponent<CactusSplinter>().IsHoming = false;
            }
        }
    }
    public IEnumerator CactusTokenCounter()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_playerHealth.IsBlocking == true)
            {
                yield return new WaitForSeconds(0.1f);
                float _randomSpread = Random.Range(-1, 1);
                GameObject thebullet = Instantiate(CactusSplinter, CurrentWeapon.FirePoint.position, PlayerMovement.ArmPivotGameObject.transform.rotation);
                thebullet.transform.Rotate(0, 0, _randomSpread);
                thebullet.GetComponent<CactusSplinter>().CameraShakeStrength = 0;
                thebullet.GetComponent<CactusSplinter>().TimeToLive = 10f;
                thebullet.GetComponent<CactusSplinter>().WhatToHit = CurrentWeapon.WhatToHit;
                thebullet.GetComponent<CactusSplinter>().BulletSpeed = 4f;
                thebullet.GetComponent<CactusSplinter>().Damage = 5;
                thebullet.GetComponent<CactusSplinter>().IsHoming = true;
            }
        }
    }
    public void AddCactusTokenInt()
    {
        CactiDestoyed += 1;
        if (CactiDestoyed == 50)
        {
            AddTokens(CactusToken);
            OnEnableDialogueManager _dialogue = Object.FindObjectOfType<OnEnableDialogueManager>();
            _dialogue.Dialogues.SentencesToSpeak.Clear();
            _dialogue.Dialogues.SentencesToSpeak.Add("You earned the Cactus Token! Check it out in the Token Menu!");
            _dialogue.ActivateDialogue();

        }
    }
    public void AddShrubsTokenInt()
    {
        ShrubsDestoyed += 1;
        if (ShrubsDestoyed == 50)
        {
            AddTokens(WormToken);
            OnEnableDialogueManager _dialogue = Object.FindObjectOfType<OnEnableDialogueManager>();
            _dialogue.Dialogues.SentencesToSpeak.Clear();
            _dialogue.Dialogues.SentencesToSpeak.Add("You earned the Worm Token! Check it out in the Token Menu!");
            _dialogue.ActivateDialogue();
        }
    }
    public void RevolverTokenDash()
    {
        CurrentWeapon.InstantReload();
        StopCoroutine(CurrentWeapon.ReloadClipTimer());
    }
    public void RevolverTokenCounter()
    {
        CurrentWeapon.AddAmmo(1);
    }
    public void WormTokenDash()
    {
        Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PlayerMovement.transform.position = new Vector2(_mousePos.x, _mousePos.y);
    }
    public void WormTokenCounter()
    {
        EnemyHealth[] hitCollidersS = FindObjectsOfType(typeof(EnemyHealth)) as EnemyHealth[];
        //Collider2D[] hitCollidersS = Physics2D.OverlapCircleAll(_playerHealth.transform.position, 10f, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (var hitCollider in hitCollidersS)
        {
            hitCollider.GetComponent<EnemyHealth>().Debuff();
        }
    }

    public void AddTokens(GameObject token)
    {
        if (TokensOwned.Contains(token) == false)
        {
            TokensOwned.Add(token);
        }
    }

    /*_________________FOR ENABLE/DISABLE OF THE TOKENS_________________*/

    ///The ActivateDefaultToken function activates the defualt token, and deacitvates every other token. Then different variables to match it's unique trait.
    public void ActivateDefaultToken()
    {
        CurrentWeapon = Object.FindObjectOfType<WeaponController>().CurrentGun.GetComponent<Weapon>();
        DefaultTokenActive = true;
        CactusTokenActive = false;
        WormTokenActive = false;
        RevloverTokenActive = false;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        ShortInfo = "Default token is obtained at the start of the game for free. The values in the token is the default values for all the variables. Other tokens can however change these.";

        CustomPlayerMoveSpeed = 5f;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        CustomPlayerJumpHeight = 10000;
        PlayerMovement.JumpForce = CustomPlayerJumpHeight;

        GunLifeStealAmount = .5f;
        CurrentWeapon.BulletTimeToLive = 1f;
        CurrentWeapon.Firerate = 10;
        CurrentWeapon.MinVerticalSpread = -1;
        CurrentWeapon.MaxVerticalSpread = 5;
        CurrentWeapon.UseClipSize = false;
        CurrentWeapon.Damage = 8;
        CurrentWeapon.BulletSpeed = 30;
        CurrentWeapon.IsHoming = false;

        CustomDashSpeed = 15;
        CustomDashDuration = 0.3f;
        CustomDashCooldown = 1;
        PlayerMovement.DashLength = CustomDashDuration;
        PlayerMovement.DashSpeed = CustomDashSpeed;
        PlayerMovement.DashCooldown = CustomDashCooldown;

        CustomBlockLifeSteal = 0;
        _playerHealth.BlockLifeSteal = CustomBlockLifeSteal;
        CustomBlockLifeStealCooldown = 3;
        _playerHealth.BlockCooldownTime = CustomBlockLifeStealCooldown;
        CustomBlockLifeStealActiveTime = 1;
        _playerHealth.BlockActiveTime = CustomBlockLifeStealActiveTime;

        ShieldLifeSteal = 0;
        _shieldHP.LifeStealAmount = ShieldLifeSteal;
        _shieldHP.MaxHP = 12;
        _shieldHP.HP = 12;

        _weaponAccuracy = "decent";
    }
    ///The ActivateCactusToken function activates the Cactus token, and deacitvates every other token. Then different variables to match it's unique trait.
    public void ActivateCactusToken()
    {
        DefaultTokenActive = false;
        CactusTokenActive = true;
        WormTokenActive = false;
        RevloverTokenActive = false;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        ShortInfo = "When dashing projectiles shoot out in the direction of the momentum. Bullets split into 8 after traveling the maximum distance. When blocking succesfully it shoots out 3 thorns, which homes to the nearest enemy. " +
        "Shield gives the player touch damage.";

        CustomPlayerMoveSpeed = 5f;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        CustomPlayerJumpHeight = 10000;
        PlayerMovement.JumpForce = CustomPlayerJumpHeight;

        GunLifeStealAmount = 0;
        CurrentWeapon.BulletTimeToLive = .08f;
        CurrentWeapon.Firerate = 3;
        CurrentWeapon.MinVerticalSpread = 0;
        CurrentWeapon.MaxVerticalSpread = 0;
        CurrentWeapon.UseClipSize = false;
        CurrentWeapon.Damage = 6;
        CurrentWeapon.BulletSpeed = 60;
        CurrentWeapon.IsHoming = false;


        CustomDashSpeed = 10;
        CustomDashDuration = 0.5f;
        CustomDashCooldown = 2;
        PlayerMovement.DashLength = CustomDashDuration;
        PlayerMovement.DashSpeed = CustomDashSpeed;
        PlayerMovement.DashCooldown = CustomDashCooldown;

        CustomBlockLifeSteal = 1;
        _playerHealth.BlockLifeSteal = CustomBlockLifeSteal;
        CustomBlockLifeStealCooldown = 3;
        _playerHealth.BlockCooldownTime = CustomBlockLifeStealCooldown;
        CustomBlockLifeStealActiveTime = 1;
        _playerHealth.BlockActiveTime = CustomBlockLifeStealActiveTime;

        ShieldLifeSteal = 2;
        _shieldHP.LifeStealAmount = ShieldLifeSteal;
        _shieldHP.MaxHP = 18;
        _shieldHP.HP = 18;

        _weaponAccuracy = "Perfect";

    }
    ///The ActivateRevolverToken function activates the Revolver token, and deacitvates every other token. Then different variables to match it's unique trait.

    public void ActivateRevolverToken()
    {
        DefaultTokenActive = false;
        CactusTokenActive = false;
        WormTokenActive = false;
        RevloverTokenActive = true;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        ShortInfo = "The Revlover tokens allows the gun can to shoot 6 times in rapid succession. When dashing the your gun is reloaded. If you successfully counter/block another bullet it adds one more ammo to the current clip."
        + " Shield has however lower health.";

        CustomPlayerMoveSpeed = 5f;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        CustomPlayerJumpHeight = 11000;
        PlayerMovement.JumpForce = CustomPlayerJumpHeight;

        GunLifeStealAmount = 0;
        CurrentWeapon.BulletTimeToLive = 1f;
        CurrentWeapon.Firerate = 20;
        CurrentWeapon.MinVerticalSpread = -7;
        CurrentWeapon.MaxVerticalSpread = 7;
        CurrentWeapon.UseClipSize = true;
        CurrentWeapon.Damage = 8;
        CurrentWeapon.BulletSpeed = 60;
        CurrentWeapon.IsHoming = false;
        CurrentWeapon.MaxClipSize = 6;
        CurrentWeapon.ReloadTime = 1.5f;

        CustomDashSpeed = 20;
        CustomDashDuration = 0.3f;
        CustomDashCooldown = 2;
        PlayerMovement.DashLength = CustomDashDuration;
        PlayerMovement.DashSpeed = CustomDashSpeed;
        PlayerMovement.DashCooldown = CustomDashCooldown;

        CustomBlockLifeSteal = 0;
        _playerHealth.BlockLifeSteal = CustomBlockLifeSteal;
        CustomBlockLifeStealCooldown = 3;
        _playerHealth.BlockCooldownTime = CustomBlockLifeStealCooldown;
        CustomBlockLifeStealActiveTime = 1;
        _playerHealth.BlockActiveTime = CustomBlockLifeStealActiveTime;

        ShieldLifeSteal = 0;
        _shieldHP.LifeStealAmount = ShieldLifeSteal;
        _shieldHP.MaxHP = 6;
        _shieldHP.HP = 6;

        _weaponAccuracy = "bad";
    }

    ///The ActivateWormToken function activates the Worm token, and deacitvates every other token. Then different variables to match it's unique trait.

    public void ActivateWormToken()
    {
        DefaultTokenActive = false;
        CactusTokenActive = false;
        RevloverTokenActive = false;
        WormTokenActive = true;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        ShortInfo = "Super fast dash! But can only dash if the cursor is not colliding with other objects! Bullets are now slow, homing bullets. The bullet will always seek the nearest enemy. So make sure they are not behind a wall! When countering or blocking with the shield enemies will be highlighted!";

        CustomPlayerMoveSpeed = 6f;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        CustomPlayerJumpHeight = 10000;
        PlayerMovement.JumpForce = CustomPlayerJumpHeight;

        GunLifeStealAmount = .6f;
        CurrentWeapon.BulletTimeToLive = 1f;
        CurrentWeapon.Firerate = 10;
        CurrentWeapon.MinVerticalSpread = -3;
        CurrentWeapon.MaxVerticalSpread = 3;
        CurrentWeapon.UseClipSize = false;
        CurrentWeapon.Damage = 7;
        CurrentWeapon.BulletSpeed = 7;
        CurrentWeapon.IsHoming = true;

        CustomDashSpeed = 100;
        CustomDashDuration = 0.07f;
        CustomDashCooldown = 2;
        PlayerMovement.DashLength = CustomDashDuration;
        PlayerMovement.DashSpeed = CustomDashSpeed;
        PlayerMovement.DashCooldown = CustomDashCooldown;

        CustomBlockLifeSteal = 0;
        _playerHealth.BlockLifeSteal = CustomBlockLifeSteal;
        CustomBlockLifeStealCooldown = 3;
        _playerHealth.BlockCooldownTime = CustomBlockLifeStealCooldown;
        CustomBlockLifeStealActiveTime = 1;
        _playerHealth.BlockActiveTime = CustomBlockLifeStealActiveTime;

        ShieldLifeSteal = 1;
        _shieldHP.LifeStealAmount = ShieldLifeSteal;
        _shieldHP.MaxHP = 21;
        _shieldHP.HP = 21;

        _weaponAccuracy = "OK";
    }
}
