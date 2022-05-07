using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReset : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _inactiveSprite;
    private SpriteRenderer _spriteRenderer;
    private TutorialManager _tutorialManager;

    // Start is called before the first frame update
    void Start()
    {
        _tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
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
