using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private CinematicBars _blackBars;
    private FadeToWhite _fadeToWhite;
    [SerializeField] private MovingBackground _background;
    [SerializeField] private MovingFloor _floor;
    [SerializeField] private Transform _bigWorm;
    private float _speedIncrease = 0f;
    private float _speedIncreaseMax = 10f;
    private float _screenShakePower = 0f;
    private float _screenShakePowerMax = 0.2f;
    private bool _cameraTurning = false;
    private bool _activated = false;
    private Movement _playerMovement;
    [SerializeField] private Transform _playerPosition;

    private void Start()
    {
        _fadeToWhite = GameObject.Find("FadeToWhite").GetComponent<FadeToWhite>();
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        _blackBars = GameObject.Find("BlackBars").GetComponent<CinematicBars>();
    }

    private void Update()
    {
        if (!_activated) return;

        if(_speedIncrease < _speedIncreaseMax) _speedIncrease += Time.deltaTime;
        if (_screenShakePower < _screenShakePowerMax) _screenShakePower += 0.01f * Time.deltaTime;

        _background.Speed += _speedIncrease * Time.deltaTime;
        _floor.Speed += _speedIncrease * Time.deltaTime;

        if (_cameraTurning)
        {
            _playerMovement.transform.position = _playerPosition.position;
            _bigWorm.transform.position = _playerPosition.position;
        }
    }

    public void Activate()
    {
        _blackBars.Show(300f, 0.5f);
        _fadeToWhite.FadeIn(3f);
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;
        _activated = true;
        Invoke("Deactivate", 5f);
        InvokeRepeating("ScreenShake", 0f, 0.05f);
    }

    private void Deactivate()
    {
        CancelInvoke();
        _blackBars.Hide(0.5f);
        _fadeToWhite.FadeOut(3f);
        _playerMovement.NoControl = false;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = true;
        _activated = false;
    }

    private void ScreenShake()
    {
        ScreenShakeController.Instance.StartShake(.05f, _screenShakePower);
    }
}
