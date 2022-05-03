using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCutsceneManager : MonoBehaviour
{
    private BossController _bossController;
    private Movement _playerMovement;
    private CinematicBars _blackBars;
    private FadeToWhite _fadeToWhite;
    [SerializeField] private Transform _knockOffPoint;
    [SerializeField] private Transform _jumpUpPoint;
    [SerializeField] private Transform _teleportAfter;
    [SerializeField] private float _timeToMove;
    private float _t;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _explosionEffect;
    private Camera _camera;
    private bool _jumpingOff;

    private Vector3 _moveFrom;
    private Vector3 _moveTo;

    private bool _movePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _playerMovement = GameObject.Find("Main_Character").GetComponent<Movement>();
        _bossController = GameObject.Find("BossController").GetComponent<BossController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_movePlayer) return;

        _playerMovement.transform.position = Vector3.MoveTowards(_playerMovement.transform.position, _moveTo, Time.deltaTime * _speed);

        if(Vector3.Distance(_playerMovement.transform.position, _moveTo) < 0.1f)
        {
            _movePlayer = false;
            ReturnControl();
            if (_jumpingOff) SendSignalOff();
            else SendSignalOn();
        }

        //_t += Time.deltaTime / _timeToMove;
        //_playerMovement.transform.position = Vector3.Lerp(_moveFrom, _moveTo, _t);
    }

    public void KnockOffBoss()
    {
        _jumpingOff = true;
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;

        ScreenShakeController.Instance.StartShake(2f, 0.05f, false);
        _t = 0;
        _moveFrom = _playerMovement.transform.position;
        _moveTo = _knockOffPoint.position;
        _movePlayer = true;
    }

    public void JumpOnBoss()
    {
        _jumpingOff = false;
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;
        _playerMovement.DashAnimation.SetActive(true);

        ScreenShakeController.Instance.StartShake(2f, 0.05f, false);
        _t = 0;
        _moveFrom = _playerMovement.transform.position;
        _moveTo = _jumpUpPoint.position;
        _movePlayer = true;
    }

    public void BossDeath()
    {
        _blackBars = Resources.FindObjectsOfTypeAll(typeof(CinematicBars))[0] as CinematicBars;
        _fadeToWhite = Resources.FindObjectsOfTypeAll(typeof(FadeToWhite))[0] as FadeToWhite;
        _blackBars.gameObject.SetActive(true);
        _fadeToWhite.gameObject.SetActive(true);

        // Remove player control
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;

        _blackBars.Show(300f, 0.5f);

        InvokeRepeating("RandomExplosion", 0f, 1f);
        InvokeRepeating("RandomExplosion", 1.3f, 0.5f);
        InvokeRepeating("RandomExplosion", 1.5f, 0.2f);
        InvokeRepeating("RandomExplosion", 2f, 0.3f);
        Invoke("FadeIn", 5f);
    }

    void RandomExplosion()
    {
        // Explosion randomly on worm body
        // Select random y-coordinate between JumpOnPoint and KnockOffPoint.
        float randomY = Random.Range(_knockOffPoint.position.y, _jumpUpPoint.position.y);

        Vector3 _cameraLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        Vector3 _cameraRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        float randomX = Random.Range(_cameraLeft.x, _cameraRight.x);

        Instantiate(_explosionEffect, new Vector3(randomX, randomY, 0), Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
        ScreenShakeController.Instance.StartShake(.05f, .03f, true);
    }

    void FadeIn()
    {
        // Fade to white
        _fadeToWhite.FadeIn(3f);
        Invoke("FullyFaded", 3.5f);
    }

    void FullyFaded()
    {
        CancelInvoke();
        _playerMovement.transform.position = _teleportAfter.position;
        _bossController.OnKnockedOff();
        Destroy(GameObject.Find("Boss"));
        Invoke("FadeOut", 1.5f);
    }

    void FadeOut()
    {
        _fadeToWhite.FadeOut(1f);
        Invoke("HideBlackBars", 0.5f);
        Invoke("ReturnControl", 1f);
        Invoke("DisableUI", 1f);
    }

    private void DisableUI()
    {
        _blackBars.gameObject.SetActive(false);
        _fadeToWhite.gameObject.SetActive(false);
    }

    private void HideBlackBars()
    {
        _blackBars.Hide(0.5f);
    }

    void ReturnControl()
    {
        _playerMovement.DashAnimation.SetActive(false);
        _playerMovement.NoControl = false;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = true;
    }

    void SendSignalOff()
    {
        _bossController.OnKnockedOff();
    }

    void SendSignalOn()
    {
        _bossController.OnJumpedOn();
    }
}
