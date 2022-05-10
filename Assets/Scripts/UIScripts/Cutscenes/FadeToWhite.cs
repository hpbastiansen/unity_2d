using UnityEngine;
using UnityEngine.UI;

/// This script creates an UI element which can fade in and out using the methods provided.
public class FadeToWhite : MonoBehaviour
{
    private RectTransform _whiteScreen;
    private Image _image;
    private float _targetOpacity;
    private float _changeOpacityAmount;
    private bool _isActive;

    /// Called at initialization, before all objects Start() methods.
    /** In the Awake method, we create the fading gameobject. It's color is set to white, and we scale and anchor it to fit the whole screen. */
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

    /// This method changes the target opacity of the UI element to 1, sets the amount it should change every second, then activates it.
    public void FadeIn(float _time)
    {
        _targetOpacity = 1f;
        _changeOpacityAmount = 1f / _time;
        _isActive = true;
    }

    /// This method changes the target opacity of the UI element to 0, sets the amount it should change every second, then activates it.
    public void FadeOut(float _time)
    {
        _targetOpacity = 0;
        _changeOpacityAmount = 1f / _time;
        _isActive = true;
    }

    /// Called every frame.
    /** If activated, the opacity of the UI element changes every frame, moving towards the target opacity.
        If the target opacity is hit, we stop the element. */
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
