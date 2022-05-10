using UnityEngine;

/// This script manages the cutscene after the boss encounter on stage 4.
public class CutsceneManagerStage4 : MonoBehaviour
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

    /// Called before the first frame.
    private void Start()
    {
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }

    /// Called every frame.
    /** The Update method moves the cutscene elements around. We increase the speed and screen shake power over time, up to a maximum.
        At the same time, if the world should be rotating, we rotate the player and boss. */
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
            _bigWorm.transform.eulerAngles = new Vector3(_bigWorm.transform.eulerAngles.x, _bigWorm.transform.eulerAngles.y, _bigWorm.transform.eulerAngles.z + (Time.deltaTime * 4f));
        }
    }

    /// Starts the cutscene.
    /** First, we activate the gameobjects for the cinematic bars and the fade to white. 
        Then, we disable the gameobjects in the _objectsToDisable array.
        Player control and physics is removed, cinematic bars are shown, and cutscene elements are invoked after the times specified have passed. */
    public void Activate()
    {
        _blackBars = Resources.FindObjectsOfTypeAll(typeof(CinematicBars))[0] as CinematicBars;
        _fadeToWhite = Resources.FindObjectsOfTypeAll(typeof(FadeToWhite))[0] as FadeToWhite;

        _blackBars.gameObject.SetActive(true);
        _fadeToWhite.gameObject.SetActive(true);

        foreach(GameObject _object in _objectsToDisable)
        {
            if (_object.activeSelf) _object.SetActive(false);
        }
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;
        _playerPositionBefore.position = _playerMovement.transform.position;
        _blackBars.Show(300f, 0.5f);
        Invoke("RotateWorld", _timeUntilRotation);
        Invoke("StartFade", _timeUntilFade);
        InvokeRepeating("ScreenShake", 0f, 0.05f);
        _activated = true;
    }

    /// Starts the world rotation, used in the Update method.
    private void RotateWorld()
    {
        _worldRotating = true;
    }

    /// Starts the Fade to white element, invoking OnFullFade after the screen has fully faded.
    private void StartFade()
    {
        _fadeToWhite.FadeIn(_fadeInTime);
        Invoke("OnFullFade", _fadeInTime);
    }

    /// Called when the screen is fully faded. Here we teleport the player away, remove rotation and disable the boss sequence elements. The EndFade method is invoked after the specified time has passed.
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

    /// This method starts the fade out process. The screen fades out in the specified time, and the methods for removing black bars and deactivating the cutscene are invoked.
    private void EndFade()
    {
        CancelInvoke();
        _fadeToWhite.FadeOut(_fadeOutTime);
        Invoke("HideBlackBars", 3.5f);
        Invoke("Deactivate", 4f);
    }

    /// This method hides the cinematic bars, using 0.5 seconds to go away completely.
    private void HideBlackBars()
    {
        _blackBars.Hide(0.5f);
    }

    /// This method reactivates player control and physics, and disables the cinematic bars and fade to white elements.
    private void Deactivate()
    {
        _playerMovement.NoControl = false;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = true;
        _blackBars.gameObject.SetActive(false);
        _fadeToWhite.gameObject.SetActive(false);
    }

    /// This method calls the ScreenShakeController to shake the screen with the specified power.
    private void ScreenShake()
    {
        ScreenShakeController.Instance.StartShake(.05f, _screenShakePower, true);
    }
}
