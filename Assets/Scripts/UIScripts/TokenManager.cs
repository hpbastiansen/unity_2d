using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// The TokenManger script handles most actions and variables surrounding the tokens.
public class TokenManager : MonoBehaviour
{
    public GameObject TokenUI;
    public GameObject DefaultToken;
    public bool DefaultTokenActive;
    public GameObject CactusToken;
    public bool CactusTokenActive;
    public GameObject RevloverToken;
    public bool RevolverTokenActive;
    public GameObject WormToken;
    public bool WormTokenActive;
    public bool TokenUIactive;
    public List<GameObject> TokensOwned;

    public List<bool> TokensActive;
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
        PlayerMovement = FindObjectOfType<Movement>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _shieldHP = FindObjectOfType<ShieldHP>();
        WeaponControllerScript = FindObjectOfType<WeaponController>();
        _uiManager = FindObjectOfType<UIManager>();
        ReadyToGiveToken = true;
    }

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we make sure the UI elements are disabled.
    We add all the tokens that the player owns into a list, then deactivate all of them, and reactivate the default one.*/
    void Start()
    {
        _checkUI = FindObjectOfType<UITest>();
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

    /// Update is called every frame.
    /** The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often. */
    /*! In the Update function if statements set the TokenUI on/off based on _tokenUIactive variable. This also allows the player to true/false the variable. */
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

    /// The NextToken allows the player to show the next token the player owns.
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

    /// The PreviousToken allows the player to see the previous token the player owns.
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

    /// Spawns 5 cactus splinters after dashing with the Cactus Token active.
    public IEnumerator CactusTokenDash()
    {
        for (int _i = 0; _i < 5; _i++)
        {
            if (PlayerMovement.IsDashing == true)
            {
                yield return new WaitForSeconds(0.1f);
                float _randomSpread = Random.Range(-1, 1);
                GameObject _bullet = Instantiate(CactusSplinter, CurrentWeapon.FirePoint.position, PlayerMovement.ArmPivotGameObject.transform.rotation);
                _bullet.transform.Rotate(0, 0, _randomSpread);
                _bullet.GetComponent<CactusSplinter>().CameraShakeStrength = 0;
                _bullet.GetComponent<CactusSplinter>().TimeToLive = 1f;
                _bullet.GetComponent<CactusSplinter>().WhatToHit = CurrentWeapon.WhatToHit;
                _bullet.GetComponent<CactusSplinter>().BulletSpeed = CustomDashSpeed * 1.5f;
                _bullet.GetComponent<CactusSplinter>().Damage = 5;
                _bullet.GetComponent<CactusSplinter>().IsHoming = false;
            }
        }
    }

    /// Spawns 3 homing cactus splinters after countering with the Cactus Token active.
    public IEnumerator CactusTokenCounter()
    {
        for (int _i = 0; _i < 3; _i++)
        {
            if (_playerHealth.IsBlocking == true)
            {
                yield return new WaitForSeconds(0.1f);
                float _randomSpread = Random.Range(-1, 1);
                GameObject _bullet = Instantiate(CactusSplinter, CurrentWeapon.FirePoint.position, PlayerMovement.ArmPivotGameObject.transform.rotation);
                _bullet.transform.Rotate(0, 0, _randomSpread);
                _bullet.GetComponent<CactusSplinter>().CameraShakeStrength = 0;
                _bullet.GetComponent<CactusSplinter>().TimeToLive = 10f;
                _bullet.GetComponent<CactusSplinter>().WhatToHit = CurrentWeapon.WhatToHit;
                _bullet.GetComponent<CactusSplinter>().BulletSpeed = 4f;
                _bullet.GetComponent<CactusSplinter>().Damage = 5;
                _bullet.GetComponent<CactusSplinter>().IsHoming = true;
            }
        }
    }

    /// Called on cactus destruction. If the player has destroyed 50 cacti, award the cactus token and show dialogue.
    public void AddCactusTokenInt()
    {
        CactiDestoyed += 1;
        if (CactiDestoyed == 50)
        {
            AddTokens(CactusToken);
            OnEnableDialogueManager _dialogue = FindObjectOfType<OnEnableDialogueManager>();
            _dialogue.Dialogues.SentencesToSpeak.Clear();
            _dialogue.Dialogues.SentencesToSpeak.Add("You earned the Cactus Token! Check it out in the Token Menu!");
            _dialogue.ActivateDialogue();

        }
    }

    /// Called on shrub destruction. If the player has destroyed 50 shrubs, award the worm token and show dialogue.
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

    /// Instantly reloads the player's weapon after dashing with the revolver token active.
    public void RevolverTokenDash()
    {
        CurrentWeapon.InstantReload();
        StopCoroutine(CurrentWeapon.ReloadClipTimer());
    }

    /// Increase player's ammo count after countering with the revolver token active.
    public void RevolverTokenCounter()
    {
        CurrentWeapon.AddAmmo(1);
    }

    /// Teleport the player to the cursor if dashing with the Worm Token active.
    public void WormTokenDash()
    {
        Vector3 _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        PlayerMovement.transform.position = new Vector2(_mousePos.x, _mousePos.y);
    }

    /// Debuffs all enemies after countering with the Worm Token active.
    public void WormTokenCounter()
    {
        EnemyHealth[] _hitColliders = FindObjectsOfType(typeof(EnemyHealth)) as EnemyHealth[];
        foreach (EnemyHealth _hitCollider in _hitColliders)
        {
            _hitCollider.Debuff();
        }
    }

    /// Add the specified token to the player's owned tokens if not already owned.
    public void AddTokens(GameObject _token)
    {
        if (TokensOwned.Contains(_token) == false)
        {
            TokensOwned.Add(_token);
        }
    }

    /*_________________FOR ENABLE/DISABLE OF THE TOKENS_________________*/

    /// The ActivateDefaultToken function activates the defualt token, and deactivates every other token. Then sets different variables to match its unique traits.
    public void ActivateDefaultToken()
    {
        CurrentWeapon = FindObjectOfType<WeaponController>().CurrentGun.GetComponent<Weapon>();
        DefaultTokenActive = true;
        CactusTokenActive = false;
        WormTokenActive = false;
        RevolverTokenActive = false;

        // VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTION.
        ShortInfo = "Default token is obtained at the start of the game for free. The values in the token is the default values for all the variables. Other tokens can however change these.";

        CustomPlayerMoveSpeed = 5f;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        CustomPlayerJumpHeight = 10000;
        PlayerMovement.JumpForce = CustomPlayerJumpHeight;

        GunLifeStealAmount = .5f;
        CurrentWeapon.BulletTimeToLive = 1f;
        CurrentWeapon.Firerate = 8;
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
        _shieldHP.MaxHP = 13;
        _shieldHP.HP = 13;

        _weaponAccuracy = "decent";
    }

    /// The ActivateCactusToken function activates the Cactus token, and deactivates every other token. Then sets different variables to match its unique traits.
    public void ActivateCactusToken()
    {
        DefaultTokenActive = false;
        CactusTokenActive = true;
        WormTokenActive = false;
        RevolverTokenActive = false;

        // VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTION.
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

    /// The ActivateRevolverToken function activates the Revolver token, and deactivates every other token. Then sets different variables to match its unique traits.
    public void ActivateRevolverToken()
    {
        DefaultTokenActive = false;
        CactusTokenActive = false;
        WormTokenActive = false;
        RevolverTokenActive = true;

        // VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTION.
        ShortInfo = "The Revlover tokens allows the gun can to shoot 6 times in rapid succession. When dashing the your gun is reloaded. If you successfully counter/block another bullet it adds one more ammo to the current clip."
        + " Shield has however lower health.";

        CustomPlayerMoveSpeed = 5f;
        PlayerMovement.MoveSpeed = CustomPlayerMoveSpeed;
        PlayerMovement.ActiveMoveSpeed = CustomPlayerMoveSpeed;

        CustomPlayerJumpHeight = 11000;
        PlayerMovement.JumpForce = CustomPlayerJumpHeight;

        GunLifeStealAmount = 0;
        CurrentWeapon.BulletTimeToLive = 1f;
        CurrentWeapon.Firerate = 15;
        CurrentWeapon.MinVerticalSpread = -7;
        CurrentWeapon.MaxVerticalSpread = 7;
        CurrentWeapon.UseClipSize = true;
        CurrentWeapon.Damage = 12;
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

    /// The ActivateWormToken function activates the Worm token, and deactivates every other token. Then sets different variables to match its unique traits.
    public void ActivateWormToken()
    {
        DefaultTokenActive = false;
        CactusTokenActive = false;
        RevolverTokenActive = false;
        WormTokenActive = true;

        // VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTION.
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
        CurrentWeapon.Damage = 6;
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
