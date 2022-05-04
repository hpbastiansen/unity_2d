using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject TokenUI;
    public GameObject TokenChange;
    public Transform TokenChangeInstantiatePosition;
    public GameObject DefaultToken;
    public GameObject BatToken;
    public GameObject ThornToken;
    public GameObject WormToken;
    public Text AmmoText;

    private void Awake()
    {
        AmmoText = GameObject.Find("AmmoTextUI").GetComponent<Text>();
    }
}
