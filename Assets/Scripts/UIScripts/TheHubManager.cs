using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHubManager : MonoBehaviour
{
    public bool IsTutorialDone;
    public Animator WorldDoorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        IsTutorialDone = false;
        IsTutorialDone = Object.FindObjectOfType<CheckPointManager>().IsTutorialDone;
    }
    private void Update()
    {
        if (IsTutorialDone)
        {
            WorldDoorAnimator.SetBool("IsOpen", true);
        }
        else
        {
            WorldDoorAnimator.SetBool("IsOpen", false);
        }
    }
}
