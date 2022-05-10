using UnityEngine;

/// This script manages the switch used to spawn the enemy in the tutorial.
public class EnemyReset : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;
    private SpriteRenderer _spriteRenderer;
    private TutorialManager _tutorialManager;

    /// Called before the first frame.
    void Start()
    {
        _tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// Called every frame.
    /** If the enemy is already spawned, show the switch as already in use. Otherwise, show it as inactive. */
    void Update()
    {
        if(_tutorialManager.Enemy == null)
        {
            _spriteRenderer.sprite = _inactiveSprite;
        }
        else
        {
            _spriteRenderer.sprite = _activeSprite;
        }
    }

    /// Called every frame a collider is inside the trigger on the gameobject.
    /** If no tutorial enemy exists, if the player is inside the trigger and presses E, the tutorial manager spawns a new enemy. */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_tutorialManager.Enemy != null) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _tutorialManager.SpawnEnemy();
            }
        }
    }
}
