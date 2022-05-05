using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private Sprite _lightOff;
    [SerializeField] private Sprite _lightOn;

    [SerializeField] private GameObject[] _topLights;
    [SerializeField] private GameObject[] _bottomLights;

    public void SetBlockedLights(int _amount)
    {
        for(int _i = 0; _i < _amount; _i++)
        {
            GameObject _light = _topLights[_i];
            _light.GetComponent<SpriteRenderer>().sprite = _lightOn;
            _light.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void SetCounterLights(int _amount)
    {
        for (int _i = 0; _i < _amount; _i++)
        {
            GameObject _light = _bottomLights[_i];
            _light.GetComponent<SpriteRenderer>().sprite = _lightOn;
            _light.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
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
