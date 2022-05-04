using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
