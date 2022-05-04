using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTutorial : MonoBehaviour
{
    [SerializeField] private LayerMask _rayColliders;
    [SerializeField] private float _detectionDistance;
    private GameObject _player;
    private Weapon_Enemy _enemyWeapon;

    // Start is called before the first frame update
    void Start()
    {
        _enemyWeapon = transform.Find("Visuals/Arm/Weapon").GetComponent<Weapon_Enemy>();
        _player = GameObject.Find("Main_Character");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _rayStart = transform.position;
        Vector3 _direction = (_player.transform.position - transform.position).normalized;

        RaycastHit2D _raycast = Physics2D.Raycast(_rayStart, _direction, _detectionDistance, _rayColliders);
        if(_raycast.collider == null)
        {
            _enemyWeapon.Target = null;
        }
        else if(_raycast.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _enemyWeapon.Target = _player;
        }
        else
        {
            _enemyWeapon.Target = null;
        }
    }
}
