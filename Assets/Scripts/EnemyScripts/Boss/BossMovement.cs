using UnityEngine;

/// This script moves the boss gameobject according to the speed value.
public class BossMovement : MonoBehaviour
{
    [HideInInspector] public float Speed;

    /// Called every frame.
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (Speed * Time.deltaTime), transform.position.y, transform.position.z);
    }
}
