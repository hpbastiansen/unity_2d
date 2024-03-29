using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicBars : MonoBehaviour
{
    private RectTransform _topBar;
    private RectTransform _bottomBar;

    private float _targetSize;
    private float _changeSizeAmount;
    private bool _isActive;

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
    public void Show(float _size, float _time)
    {
        _targetSize = _size;
        _changeSizeAmount = (_targetSize - _topBar.sizeDelta.y) / _time;
        _isActive = true;
    }

    public void Hide(float _time)
    {
        _targetSize = 0f;
        _changeSizeAmount = (_targetSize - _topBar.sizeDelta.y) / _time;
        _isActive = true;
    }

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
