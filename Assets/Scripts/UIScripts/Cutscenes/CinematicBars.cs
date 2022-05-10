using UnityEngine;
using UnityEngine.UI;

/// This script creates cinematic bars that can be shown and hidden with two public methods.
public class CinematicBars : MonoBehaviour
{
    private RectTransform _topBar;
    private RectTransform _bottomBar;

    private float _targetSize;
    private float _changeSizeAmount;
    private bool _isActive;

    /// Called at initialization, before all objects Start() methods.
    /** In the Awake method, we create both of the cinematic bar objects and set their color, anchor positions and size deltas. */
    private void Awake()
    {
        GameObject _gameObject = new GameObject("topBar", typeof(Image));
        _gameObject.transform.SetParent(transform, false);
        _gameObject.GetComponent<Image>().color = Color.black;
        _topBar = _gameObject.GetComponent<RectTransform>();
        _topBar.anchorMin = new Vector2(0, 1);
        _topBar.anchorMax = new Vector2(1, 1);
        _topBar.sizeDelta = new Vector2(0, 0);

        _gameObject = new GameObject("bottomBar", typeof(Image));
        _gameObject.transform.SetParent(transform, false);
        _gameObject.GetComponent<Image>().color = Color.black;
        _bottomBar = _gameObject.GetComponent<RectTransform>();
        _bottomBar.anchorMin = new Vector2(0, 0);
        _bottomBar.anchorMax = new Vector2(1, 0);
        _bottomBar.sizeDelta = new Vector2(0, 0);
    }

    /// This method makes the cinematic bars grow to the specified size in the time specified.
    public void Show(float _size, float _time)
    {
        _targetSize = _size;
        _changeSizeAmount = (_targetSize - _topBar.sizeDelta.y) / _time;
        _isActive = true;
    }

    /// This method makes the cinematic bars go away in the time specified.
    public void Hide(float _time)
    {
        _targetSize = 0f;
        _changeSizeAmount = (_targetSize - _topBar.sizeDelta.y) / _time;
        _isActive = true;
    }

    /// Called every frame.
    /** In the Update method, if the cinematic bars are activated, we either increase or decrease their size depending on the targetSize set. */
    void Update()
    {
        if(_isActive)
        {
            Vector2 _sizeDelta = _topBar.sizeDelta;
            _sizeDelta.y += _changeSizeAmount * Time.deltaTime;
            if (_changeSizeAmount > 0)
            {
                if (_sizeDelta.y >= _targetSize)
                {
                    _sizeDelta.y = _targetSize;
                    _isActive = false;
                }
            } else
            {
                if (_sizeDelta.y <= _targetSize)
                {
                    _sizeDelta.y = _targetSize;
                    _isActive = false;
                }
            }

            _topBar.sizeDelta = _sizeDelta;
            _bottomBar.sizeDelta = _sizeDelta;
        }
    }
}
