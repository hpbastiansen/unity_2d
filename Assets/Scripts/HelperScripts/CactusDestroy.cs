using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusDestroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Object.FindObjectOfType<TokenManager>().AddCactusTokenInt();
            Destroy(gameObject);
        }
    }
}
