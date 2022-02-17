using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Text PlayerHPtxt;
    public PlayerHealth HP;
    // Start is called before the first frame update
    void Start()
    {
        PlayerHPtxt = GameObject.Find("PlayerHealth").GetComponent<Text>();
        HP = GameObject.Find("Main_Character").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHPtxt.text = HP.HP.ToString("F0");
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Scene currentscene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentscene.name);
        }
    }
}
