using UnityEngine;

/// This script manages the cutscenes on the boss fight in stage 5.
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
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _explosionEffect;
    private Camera _camera;
    private bool _jumpingOff;

    private Vector3 _moveTo;

    private bool _movePlayer = false;
    public bool IsDown;

    /// Called before the first frame.
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _playerMovement = GameObject.Find("Main_Character").GetComponent<Movement>();
        _bossController = GameObject.Find("BossController").GetComponent<BossController>();
        IsDown = false;
    }

    /// Called every frame.
    /** The Update method moves the player towards the point set, if the _movePlayer boolean is set to true.
        When the player is close to the point, return control and signal the boss controller. */
    void Update()
    {
        if (!_movePlayer) return;

        _playerMovement.transform.position = Vector3.MoveTowards(_playerMovement.transform.position, _moveTo, Time.deltaTime * _speed);

        if (Vector3.Distance(_playerMovement.transform.position, _moveTo) < 0.1f)
        {
            _movePlayer = false;
            ReturnControl();
            if (_jumpingOff) SendSignalOff();
            else SendSignalOn();
        }
    }

    /// This method is called to start the 'Knocked off Boss' cutscene. It removes player control, shakes the screen and sets the point the player should go towards.
    public void KnockOffBoss()
    {
        _jumpingOff = true;
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;

        ScreenShakeController.Instance.StartShake(2f, 0.05f, false);
        _moveTo = _knockOffPoint.position;
        _movePlayer = true;
        IsDown = true;
        FindObjectOfType<Stage5Manager>().DebuffPlayer();
    }

    /// This method is called to start the 'Jump on Boss' cutscene. It removes player control, shakes the screen and sets the point the player should go towards.
    public void JumpOnBoss()
    {
        _jumpingOff = false;
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;
        _playerMovement.DashAnimation.SetActive(true);

        ScreenShakeController.Instance.StartShake(2f, 0.05f, false);
        _moveTo = _jumpUpPoint.position;
        _movePlayer = true;
        IsDown = false;
    }

    /// This method is called to start the 'Boss Death' cutscene. It shows cinematic bars, instantiates explosions on the boss and fades to white.
    public void BossDeath()
    {
        // Find and enable the cinematic bars and fade to white UI elements.
        _blackBars = Resources.FindObjectsOfTypeAll(typeof(CinematicBars))[0] as CinematicBars;
        _fadeToWhite = Resources.FindObjectsOfTypeAll(typeof(FadeToWhite))[0] as FadeToWhite;
        _blackBars.gameObject.SetActive(true);
        _fadeToWhite.gameObject.SetActive(true);

        // Remove player control
        _playerMovement.NoControl = true;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = false;

        _blackBars.Show(300f, 0.5f);

        // Spawn random explosion effects at irregular intervals.
        InvokeRepeating("RandomExplosion", 0f, 1f);
        InvokeRepeating("RandomExplosion", 1.3f, 0.5f);
        InvokeRepeating("RandomExplosion", 1.5f, 0.2f);
        InvokeRepeating("RandomExplosion", 2f, 0.3f);

        // Fade in after 5 seconds have passed.
        Invoke("FadeIn", 5f);
    }

    /// This method spawns a random explosion effect somewhere on the boss.
    void RandomExplosion()
    {
        // Select a random Y coordinate between the top and bottom of the boss.
        float randomY = Random.Range(_knockOffPoint.position.y, _jumpUpPoint.position.y);

        // Select a random X coordinate visible on the screen.
        Vector3 _cameraLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        Vector3 _cameraRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        float randomX = Random.Range(_cameraLeft.x, _cameraRight.x);

        // Spawn the explosion effect on the selected coordinates and shake the screen.
        Instantiate(_explosionEffect, new Vector3(randomX, randomY, 0), Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
        ScreenShakeController.Instance.StartShake(.05f, .03f, true);
    }

    /// This method is used to fade in the screen, using 3 seconds to fully fade. The FullyFaded method is called after 3.5 seconds.
    void FadeIn()
    {
        // Fade to white
        _fadeToWhite.FadeIn(3f);
        Invoke("FullyFaded", 3.5f);
    }

    /// This method is called when the fade to white element has fully faded the screen. It teleports the player to the specified location and removes the boss.
    void FullyFaded()
    {
        CancelInvoke();
        _playerMovement.transform.position = _teleportAfter.position;
        _bossController.OnKnockedOff();
        Destroy(GameObject.Find("Boss"));
        Invoke("FadeOut", 1.5f);
    }

    /// This method starts fading out the UI element, and invokes methods to return player control and removing the cutscene UI.
    void FadeOut()
    {
        _fadeToWhite.FadeOut(1f);
        Invoke("HideBlackBars", 0.5f);
        Invoke("ReturnControl", 1f);
        Invoke("DisableUI", 1f);
    }

    /// This method disables the cinematic bars and fade to white UI elements.
    private void DisableUI()
    {
        _blackBars.gameObject.SetActive(false);
        _fadeToWhite.gameObject.SetActive(false);
    }

    /// This method hides the cinematic bars, using 0.5 seconds.
    private void HideBlackBars()
    {
        _blackBars.Hide(0.5f);
    }

    /// This method returns control to the player and enables their physics.
    void ReturnControl()
    {
        _playerMovement.DashAnimation.SetActive(false);
        _playerMovement.NoControl = false;
        _playerMovement.transform.GetComponent<Rigidbody2D>().simulated = true;
    }

    /// This method signals the boss controller that the cutscene has finished and the player is on the ground.
    void SendSignalOff()
    {
        _bossController.OnKnockedOff();
    }

    /// This method signals the boss controller that the cutscene has finished and the player is on top of the boss.
    void SendSignalOn()
    {
        _bossController.OnJumpedOn();
    }
}
