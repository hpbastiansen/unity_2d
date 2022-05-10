using UnityEngine;

///The EnemyTouchDamage script makes the player take damage on contact with the gameobject it is connected to.
public class EnemyTouchDamage : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] float _damage = 20f;
    [SerializeField] float _knockback = 10f;
    private float _angle;

    ///Start is called once before the first frame update
    /**In the start function we grab a reference to the player.*/
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    ///This is called every frame a collider is inside the trigger on this gameobject.
    /**If the collider is the player, we damage the player and apply a knockback in the opposite direction.*/
    private void OnTriggerStay2D(Collider2D _collision)
    {
        if(_collision.gameObject == _player)
        {
            float _deltaX = transform.position.x - _player.transform.position.x;
            _angle = _deltaX > 0 ? 135f : 45f;

            _player.GetComponent<PlayerHealth>().TakeDamage(_damage, _knockback, _angle);
        }
    }
}
