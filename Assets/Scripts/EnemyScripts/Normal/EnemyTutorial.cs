using System.Collections;
using UnityEngine;

/// Special version of enemy without AI. Only aims and shoots at the target if within range.
public class EnemyTutorial : MonoBehaviour
{
    [SerializeField] private LayerMask _rayColliders;
    [SerializeField] private float _detectionDistance;
    private GameObject _player;
    private EnemyWeapon _enemyWeapon;
    private float _waitTime = 1f;
    private bool _hasWaited = false;

    /// Called before the first frame.
    /** Waits for _waitTime before trying to detect player. */
    void Start()
    {
        _enemyWeapon = transform.Find("Visuals/Arm/Weapon").GetComponent<EnemyWeapon>();
        _player = GameObject.Find("Main_Character");
        StartCoroutine(Activate());
    }

    /// Called every frame.
    /** Uses a raycast to check if the player is within detection distance and line of sight. If so, target the player and shoot. */
    void Update()
    {
        if (!_hasWaited) return;

        Vector3 _rayStart = transform.position;
        Vector3 _direction = (_player.transform.position - transform.position).normalized;

        RaycastHit2D _raycast = Physics2D.Raycast(_rayStart, _direction, _detectionDistance, _rayColliders);
        if(_raycast.collider == null || !(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            _enemyWeapon.Target = null;
        }
        else
        {
            _enemyWeapon.Target = _player;
        }
    }

    /// Coroutine activating detection after a certain amount of time.
    IEnumerator Activate()
    {
        yield return new WaitForSeconds(_waitTime);
        _hasWaited = true;
    }
}
