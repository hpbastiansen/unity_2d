using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// The TokenChange script is a script assigned to the UI indication of a token change. Only purpose is allow for text change and start animation.
public class TokenChange : MonoBehaviour
{
    public Text TokenText;
    private Animator _myAnimator;

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. */
    /*! Find the Text and Animator components */
    void Start()
    {
        TokenText = GetComponentInChildren<Text>();
        _myAnimator = GetComponent<Animator>();
    }

    /// IEnumerator to make sure the animation is restarted properly.
    public IEnumerator ActivateTokenAimation(string _text)
    {
        _myAnimator.Play("TokenChangeActiveAnimation_close");
        yield return new WaitForEndOfFrame();
        TokenText.text = _text;
        _myAnimator.Play("TokenChangeActiveAnimation");

    }

    /// Function to allow TokenManager to do the token change animation, and change the text.
    public void ActivateTokenAimations(string _text)
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(ActivateTokenAimation(_text));
        }
    }
}
