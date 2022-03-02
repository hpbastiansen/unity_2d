using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TokenManager : MonoBehaviour
{
    public GameObject tokenUI;
    public GameObject tokenchange;
    public Transform tokenchangepos;

    public GameObject DefaultToken;
    public bool DefaultTokenActive;
    public GameObject BatToken;
    public bool BatTokenActive;
    public GameObject ThornToken;
    public bool ThornTokenActive;
    public GameObject WormToken;
    public bool WormTokenActive;
    private bool tokenUIactive;
    public List<GameObject> tokenowned;
    public List<bool> tokensactive;


    [Header("Changeable values")]
    public float Speed;
    private Movement movement;
    public float gunLifeSteal;
    private Weapon weapon;
    public float DashSpeed;
    public float DashDuration;
    public int CounterLifeSteal;
    private PlayerHealth playerHP;
    public float ShieldLifeSteal;
    private ShieldHP shieldHP;

    [Header("IGNORE")]
    public int index;
    public bool usingTokenMenu;
    private void Awake()
    {
        movement = Object.FindObjectOfType<Movement>();
        playerHP = Object.FindObjectOfType<PlayerHealth>();
        shieldHP = Object.FindObjectOfType<ShieldHP>();

    }

    // Start is called before the first frame update
    void Start()
    {


        tokenUI.SetActive(false);
        tokenUIactive = false;

        tokenowned.Add(DefaultToken);

        tokenowned.Add(BatToken);

        foreach (GameObject tokens in tokenowned)
        {
            tokens.SetActive(false);
        }
        tokenowned[index].SetActive(true);

        ActivateDefaultToken();
    }

    // Update is called once per frame
    void Update()
    {

        if (tokenUIactive)
        {
            tokenUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            tokenUI.SetActive(false);
            Time.timeScale = 1f;
            usingTokenMenu = false;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            tokenUIactive = !tokenUIactive;
            usingTokenMenu = true;
        }
    }



    public void nextToken()
    {
        foreach (GameObject tokens in tokenowned)
        {
            tokens.SetActive(false);
        }

        if (index + 1 < tokenowned.Count)
        {
            index += 1;
        }
        else if (index + 1 == tokenowned.Count)
        {
            index = 0;
        }
        tokenowned[index].SetActive(true);
    }

    public void previousToken()
    {
        foreach (GameObject tokens in tokenowned)
        {
            tokens.SetActive(false);
        }
        if (index > 0)
        {
            index -= 1;
        }
        else if (index == 0)
        {
            index = tokenowned.Count - 1;
        }
        tokenowned[index].SetActive(true);
    }




    ///////////////////FOR ENABLE/DISABLE OF THE TOKENS///////////////////////////
    public void ActivateDefaultToken()
    {
        DefaultTokenActive = true;
        BatTokenActive = false;
        ThornTokenActive = false;
        WormTokenActive = false;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        Speed = 10;
        movement.speed = Speed;
        movement.activeMoveSpeed = Speed;

        gunLifeSteal = 0;

        DashSpeed = 20;
        DashDuration = 0.2f;
        movement.dashLength = DashDuration;
        movement.dashSpeed = DashSpeed;

        CounterLifeSteal = 99999999;
        playerHP.blockLifeSteal = CounterLifeSteal;

        ShieldLifeSteal = 9999999;
        shieldHP.lifeSteal = ShieldLifeSteal;

        GameObject tokenchangeobj = Instantiate(tokenchange, tokenchangepos.position, tokenchangepos.rotation) as GameObject;
        tokenchangeobj.transform.SetParent(tokenchangepos, true);
        tokenchangeobj.GetComponent<tokenchange>().txt.text = "Default Token Active";

    }

    public void ActivateBatToken()
    {
        weapon = Object.FindObjectOfType<WeaponController>().currentGun.GetComponent<Weapon>();

        DefaultTokenActive = false;
        BatTokenActive = true;
        ThornTokenActive = false;
        WormTokenActive = false;

        //VARIABLES TO MAKE IT SPECIAL. IMPORTANT! WILL ONLY START TAKING EFFECT WHENEVER PLAYER ACTIVATES THE TOKEN! IS NOT CONNECTED TO AN UPDATE FUNCTOIN.
        Speed = 10;
        movement.speed = Speed;
        movement.activeMoveSpeed = Speed;

        gunLifeSteal = 0.2f;

        DashSpeed = 14;
        DashDuration = 0.6f;
        movement.dashLength = DashDuration;
        movement.dashSpeed = DashSpeed;

        CounterLifeSteal = 10;
        playerHP.blockLifeSteal = CounterLifeSteal;

        ShieldLifeSteal = 20;
        shieldHP.lifeSteal = ShieldLifeSteal;

        GameObject tokenchangeobj = Instantiate(tokenchange, tokenchangepos.position, tokenchangepos.rotation);
        tokenchangeobj.transform.SetParent(tokenchangepos, true);
        tokenchangeobj.GetComponent<tokenchange>().txt.text = "Bat Token Active";
    }

    public void ActivateThornToken()
    {
        DefaultTokenActive = false;
        BatTokenActive = false;
        ThornTokenActive = true;
        WormTokenActive = false;
    }

    public void ActivateWormToken()
    {
        DefaultTokenActive = false;
        BatTokenActive = false;
        ThornTokenActive = false;
        WormTokenActive = true;
    }
}
