using UnityEngine;

/// This script sets the Tutorial state. The tutorial is divided into steps which blocks the player's progress until completed.
public class TutorialManager : MonoBehaviour
{
    [Header("Step0")]
    public bool Step0;
    public GameObject Step0Block;

    [Header("Step1")]
    public bool Step1;
    public GameObject Step1Block;

    [Header("Step2")]
    public bool Step2;
    public GameObject Step2Block;

    [Header("Step3")]
    public bool Step3;
    public GameObject Step3Block;

    [Header("Step4")]
    public bool Step4;
    public GameObject Step4Block;

    [Header("Step5")]
    public bool Step5;
    public GameObject Step5Block;
    private int _shotsBlocked = 0;
    private int _shotsCountered = 0;
    [SerializeField] private Door _enemyDoor;
    [SerializeField] private GameObject _enemyPrefab;
    [HideInInspector] public GameObject Enemy;
    [SerializeField] private TutorialPanel _tutorialPanel;

    [Header("Step6")]
    public bool Step6;

    /// Called before the first frame.
    void Start()
    {
        Step1 = false;
        Step2 = false;
        Step3 = false;
        Step4 = false;
        Step5 = false;
        Step6 = false;
    }

    /// Changes step 0 to done.
    public void Step0Done()
    {
        Step0 = true;
        Step0Block.SetActive(false);
    }

    /// Changes step 1 to done.
    public void Step1Done()
    {
        Step1 = true;
        Step1Block.SetActive(false);
    }

    /// Changes step 2 to done.
    public void Step2Done()
    {
        Step2 = true;
        Step2Block.SetActive(false);
    }

    /// Changes step 3 to done.
    public void Step3Done()
    {
        Step3 = true;
        Step3Block.SetActive(false);
    }

    /// Changes step 4 to done.
    public void Step4Done()
    {
        Step4 = true;
        Step4Block.SetActive(false);
    }

    /// Changes step 5 to done.
    public void Step5Done()
    {
        Step5 = true;
        Step5Block.SetActive(false);
    }

    /// Changes step 6 to done.
    public void Step6Done()
    {
        Step6 = true;
        FindObjectOfType<CheckPointManager>().IsTutorialDone = true;
    }

    /// Called when the player counters a shot in the tutorial. Updates the light panel, increases the 'Shots countered' variable and checks if the step is complete.
    public void ShotCountered()
    {
        if (_shotsCountered >= 3) return;
        _shotsCountered++;
        _tutorialPanel.SetCounterLights(_shotsCountered);
        CheckShots();
    }

    /// Called when the player blocks a shot in the tutorial. Updates the light panel, increases the 'Shots blocked' variable and checks if the step is complete.
    public void ShotBlocked()
    {
        if (_shotsBlocked >= 3) return;
        _shotsBlocked++;
        _tutorialPanel.SetBlockedLights(_shotsBlocked);
        CheckShots();
    }

    /// Checks if 3 shots have been countered and blocked. If so, open the door to let the player progress.
    private void CheckShots()
    {
        if (_shotsBlocked >= 3 && _shotsCountered >= 3) _enemyDoor.Triggered = true;
    }

    /// Spawn an enemy at the position of the EnemySpawnPoint transform.
    public void SpawnEnemy()
    {
        Enemy = Instantiate(_enemyPrefab, GameObject.Find("EnemySpawnPoint").transform.position, Quaternion.identity);
    }
}
