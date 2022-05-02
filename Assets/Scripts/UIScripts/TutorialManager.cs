using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Step0")]
    public bool Step0;
    public GameObject Step0Block;
    [Header("Step1")]
    public bool Step1;
    public GameObject Step1Block;

    [Header("Step2")]
    public bool Step2;
    public GameObject Step2Block;

    [Header("Step3")]
    public bool Step3;
    public GameObject Step3Block;

    [Header("Step4")]
    public bool Step4;
    public GameObject Step4Block;

    [Header("Step5")]
    public bool Step5;
    public GameObject Step5Block;


    [Header("Step6")]
    public bool Step6;
    // Start is called before the first frame update
    void Start()
    {
        Step1 = false;
        Step2 = false;
        Step3 = false;
        Step4 = false;
        Step5 = false;
        Step6 = false;
    }
    public void Step0Done()
    {
        Step0 = true;
        Step0Block.SetActive(false);
    }
    public void Step1Done()
    {
        Step1 = true;
        Step1Block.SetActive(false);
    }
    public void Step2Done()
    {
        Step2 = true;
        Step2Block.SetActive(false);
    }
    public void Step3Done()
    {
        Step3 = true;
        Step3Block.SetActive(false);
    }
    public void Step4Done()
    {
        Step4 = true;
        Step4Block.SetActive(false);
    }
    public void Step5Done()
    {
        Step5 = true;
        Step5Block.SetActive(false);
    }
    public void Step6Done()
    {
        Step6 = true;
        Object.FindObjectOfType<CheckPointManager>().IsTutorialDone = true;
    }
}
