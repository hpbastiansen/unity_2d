using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrubDestroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Object.FindObjectOfType<TokenManager>().AddShrubsTokenInt();
            Destroy(gameObject);
        }
    }
}