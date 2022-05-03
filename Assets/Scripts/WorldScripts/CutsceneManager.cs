using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private Movement _playerMovement;
    private CinematicBars _blackBars;
    private FadeToWhite _fadeToWhite;
    private float _speedIncrease = 0f;
    private float _speedIncreaseMax = 20f;
    private float _screenShakePower = 0f;
    private float _screenShakePowerMax = 0.3f;
    private bool _worldRotating = false;
    private bool _activated = false;
    [SerializeField] GameObject[] _objectsToDisable;

    [Header("Cutscene components")]
    [SerializeField] private MovingBackground _background;
    [SerializeField] private MovingFloor _floor;
    [SerializeField] private Transform _playerPositionBefore;
    [SerializeField] private Transform _playerPositionAfter;
    [SerializeField] private Transform _bigWorm;

    [Header("Cutscene settings")]
    [SerializeField] private float _timeUntilFade;
    [SerializeField] private float _fadeInTime;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _fadeOutTime;
    [SerializeField] private float _timeUntilRotation;

    private void Start()
    {
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        _fadeToWhite = GameObject.Find("FadeToWhite").GetComponent<FadeToWhite>();
        _blackBars = GameObject.Find("BlackBars").GetComponent<CinematicBars>();
    }

    private void Update()
    {
        if (!_activated) return;

        if(_speedIncrease < _speedIncreaseMax) _speedIncrease += Time.deltaTime;
        if (_screenShakePower < _screenShakePowerMax) _screenShakePower += 0.01f * Time.deltaTime;

        _background.Speed += _speedIncrease * Time.deltaTime;
        _floor.Speed += _speedIncrease * Time.deltaTime;

        if(_worldRotating)
        {
            _playerMovement.transform.position = _playerPositionBefore.position;
            _playerMovement.transform.eulerAngles = new Vector3(_floor.transform.eulerAngles.x, _floor.transform.eulerAngles.y, _floor.transform.eulerAngles.z + Time.deltaTime);
            _floor.transform.eulerAngles = new Vector3(_floor.transform.eulerAngles.x, _floor.transform.eulerAngles.y, _floor.transform.eulerAngles.z + Time.deltaTime * 0.3f);
            _bigWorm.transform.eulerAngles = new Vector3(_bigWorm.transform.eulerAngles.x, _bigWorm.transform.eulerAngles.y, _bigWorm.transform.eulerAngles.z + Time.deltaTime);
        }
    }

    public void Activate()
    {
        foreach(GameObject _object in _objectsToDisable)
        {
            if (_object.activeSelf) _object.SetActive(false);
        }
        // TODO: Remove crosshair?
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;
        _playerPositionBefore.position = _playerMovement.transform.position;
        _blackBars.Show(300f, 0.5f);
        Invoke("RotateWorld", _timeUntilRotation);
        Invoke("StartFade", _timeUntilFade);
        InvokeRepeating("ScreenShake", 0f, 0.05f);
        _activated = true;
    }

    private void RotateWorld()
    {
        _worldRotating = true;
    }

    private void StartFade()
    {
        _fadeToWhite.FadeIn(_fadeInTime);
        Invoke("OnFullFade", _fadeInTime);
    }

    private void OnFullFade()
    {
        _activated = false;
        _worldRotating = false;
        _floor.Speed = 0;
        _background.Speed = 0;
        _background.GetComponent<Parallax>().enabled = true;
        _playerMovement.transform.position = _playerPositionAfter.position;
        _playerMovement.transform.eulerAngles = Vector3.zero;
        Invoke("EndFade", _fadeDuration);
    }

    private void EndFade()
    {
        CancelInvoke();
        _fadeToWhite.FadeOut(_fadeOutTime);
        Invoke("HideBlackBars", 3.5f);
        Invoke("Deactivate", 4f);
    }

    private void HideBlackBars()
    {
        _blackBars.Hide(0.5f);
    }

    private void Deactivate()
    {
        // TODO: Activate crosshair
        _playerMovement.NoControl = false;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = true;
    }

    private void ScreenShake()
    {
        ScreenShakeController.Instance.StartShake(.05f, _screenShakePower, true);
    }
}
