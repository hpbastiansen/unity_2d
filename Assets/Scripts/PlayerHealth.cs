using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Movement movement;
    public int HP = 100;
    public bool isBlocked;
    // Start is called before the first frame update
    void Start()
    {
        movement = GameObject.Find("Main_Character").GetComponent<Movement>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isBlocked = true;
            StartCoroutine(blockCoolDown());
        }
    }


    public void TakeDmg(int x)
    {
        if (isBlocked == true)
        {
            HP += x;
        }
        else
        {
            HP -= x;
        }
        if (HP <= 0)
        {
            Debug.Log("Player died");
            SceneManager.LoadScene("BOXSCNENE");
        }

    }
    IEnumerator blockCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isBlocked = false;
    }
}
