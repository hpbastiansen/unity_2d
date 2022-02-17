using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Movement movement;
    public float HP = 100;
    public bool isBlocked;
    [Tooltip("How long the block is active for.")]
    public float blockActiveTime;
    [Tooltip("This is the value which the damage is divided by. E.g. a bullet that takes 10 dmg and this value being 5 will give the player a life steal of 2.")]
    public int blockLifeSteal;
    public float blockTimer;
    private float tempBlockTimer;
    // Start is called before the first frame update
    void Start()
    {
        movement = GameObject.Find("Main_Character").GetComponent<Movement>();
    }
    // Update is called once per frame
    void Update()
    {
        if (tempBlockTimer > 0)
        {
            tempBlockTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1) && tempBlockTimer <= 0)
        {
            isBlocked = true;
            tempBlockTimer = blockTimer;
            StartCoroutine(blockCoolDown());
        }


    }


    public void TakeDmg(float x)
    {
        if (isBlocked == true)
        {
            HP += (x / blockLifeSteal) * 1;
            tempBlockTimer = blockTimer;
        }
        else
        {
            HP -= x;
            tempBlockTimer = blockTimer;
        }
        if (HP <= 0)
        {
            Debug.Log("Player died");
            SceneManager.LoadScene("BOXSCNENE");
        }
    }
    IEnumerator blockCoolDown()
    {
        yield return new WaitForSeconds(blockActiveTime);
        isBlocked = false;
    }
}
