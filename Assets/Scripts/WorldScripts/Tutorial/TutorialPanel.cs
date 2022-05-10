using UnityEngine;

/// This script manages the light panel showing blocked and countered shots in the tutorial.
public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private Sprite _lightOff;
    [SerializeField] private Sprite _lightOn;

    [SerializeField] private GameObject[] _topLights;
    [SerializeField] private GameObject[] _bottomLights;

    /// This method sets the three lights on the top of the light panel. Parameter chooses how many lights to turn on.
    public void SetBlockedLights(int _amount)
    {
        for(int _i = 0; _i < _amount; _i++)
        {
            GameObject _light = _topLights[_i];
            _light.GetComponent<SpriteRenderer>().sprite = _lightOn;
            _light.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    /// This method sets the three lights on the bottom of the light panel. Parameter chooses how many lights to turn on.
    public void SetCounterLights(int _amount)
    {
        for (int _i = 0; _i < _amount; _i++)
        {
            GameObject _light = _bottomLights[_i];
            _light.GetComponent<SpriteRenderer>().sprite = _lightOn;
            _light.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    /// Called before the first frame.
    /** In the start method we make sure the panel is ready for use by disabling all the lights. */
    void Start()
    {
        foreach(GameObject _light in _topLights)
        {
            _light.GetComponent<SpriteRenderer>().sprite = _lightOff;
            _light.transform.GetChild(0).gameObject.SetActive(false);
        }

        foreach (GameObject _light in _bottomLights)
        {
            _light.GetComponent<SpriteRenderer>().sprite = _lightOff;
            _light.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
