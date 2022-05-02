using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToWhite : MonoBehaviour
{
    private RectTransform _whiteScreen;
    private Image _image;
    private float _targetOpacity;
    private float _changeOpacityAmount;
    private bool _isActive;

    private void Awake()
    {
        GameObject _gameObject = new GameObject("whiteScreen", typeof(Image));
        _gameObject.transform.SetParent(transform, false);
        _gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        _whiteScreen = _gameObject.GetComponent<RectTransform>();
        _whiteScreen.anchorMin = new Vector2(0, 0);
        _whiteScreen.anchorMax = new Vector2(1, 1);
        _whiteScreen.sizeDelta = new Vector2(1, 1);
        _image = _whiteScreen.GetComponent<Image>();
    }

    public void FadeIn(float _time)
    {
        _targetOpacity = 1f;
        _changeOpacityAmount = 1f / _time;
        _isActive = true;
    }

    public void FadeOut(float _time)
    {
        _targetOpacity = 0;
        _changeOpacityAmount = 1f / _time;
        _isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isActive)
        {
            Color _color = _image.color;
            float _opacity = _color.a;
            if(_targetOpacity == 1f)
            {
                _opacity += _changeOpacityAmount * Time.deltaTime;
                if(_opacity > _targetOpacity)
                {
                    _opacity = _targetOpacity;
                    _isActive = false;
                }
            }
            else
            {
                _opacity -= _changeOpacityAmount * Time.deltaTime;
                if(_opacity < 0)
                {
                    _opacity = 0;
                    _isActive = false;
                }
            }

            _color.a = _opacity;
            _image.color = _color;
        }
    }
}
