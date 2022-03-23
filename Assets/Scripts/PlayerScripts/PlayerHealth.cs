using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


///The PlayerHealth script controls the health of the player and allwos for TakeDamage function and a block ablity.
public class PlayerHealth : MonoBehaviour
{
    public Movement PlayerMovement;
    public float MaxHP = 100;
    public float CurrentHP;
    public bool IsBlocking;
    [Tooltip("How long the block is active for.")]
    public float BlockActiveTime;
    [Tooltip("This is the value which the damage is divided by. E.g. a bullet that takes 10 dmg and this value being 5 will give the player a life steal of 2.")]
    public int BlockLifeSteal;
    public float BlockCooldownTime;
    private float _tempBlockTimer;
    public bool UsingCactusToken;
    private TokenManager _tokenManager;


    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function the player Movement script is located and assigned to a variable*/
    void Start()
    {
        PlayerMovement = GameObject.Find("Main_Character").GetComponent<Movement>();
        CurrentHP = MaxHP;
        _tokenManager = Object.FindObjectOfType<TokenManager>();
    }

    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! In the Update function we check for if the player is using the block functionality. If that is the case we make a countdown for it. If player is not blocking and press the block key we 
    set the IsBlocking to true and starts the coroutine "BlockCoolDown()".*/
    void Update()
    {
        UsingCactusToken = _tokenManager.CactusTokenActive;
        if (_tempBlockTimer > 0)
        {
            _tempBlockTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1) && _tempBlockTimer <= 0)
        {
            IsBlocking = true;
            _tempBlockTimer = BlockCooldownTime;
            StartCoroutine(BlockCoolDown());

        }
        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }

    ///The TakeDamage function allows any object in the game to take damage on the player and decrease the health.
    /**If the player is blocking while this function is called, the player does not lose any health, but rather gains health depending on the lifesteal value.
    If the player is not blocking a decrease in health will happen, the amount depending on the provided parameter.
    If the player has zero or less health we log it and restarts the current scene (level).*/
    public void TakeDamage(float x)
    {
        if (IsBlocking == true)
        {
            CurrentHP += BlockLifeSteal;
            _tempBlockTimer = BlockCooldownTime;
            if (UsingCactusToken == true)
            {
                StartCoroutine(_tokenManager.CactusTokenCounter());
            }
        }
        else
        {
            CurrentHP -= x;
            _tempBlockTimer = BlockCooldownTime;
        }
        if (Mathf.Round(CurrentHP) * 1 <= 0)
        {
            CurrentHP = MaxHP;
            SetPlayerPositionAndStageInfo _startInfo = GameObject.Find("StartPosition").GetComponent<SetPlayerPositionAndStageInfo>();
            SceneManager.LoadScene("The_Hub");
            _startInfo.SetInfo();
        }
    }
    ///Starts a timer for the block cool down.
    /**When ever the player starts a block attack there will be a timer that starts based upon the BlockActiveTime time variable. When the timer hits 0, IsBlocking is set to false and the player can chose to block again if wanted.*/
    IEnumerator BlockCoolDown()
    {
        yield return new WaitForSeconds(BlockActiveTime);
        IsBlocking = false;
    }
}
