using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///Scriptable Object for each scene to be set to a start position.
[CreateAssetMenu(fileName = "StartInfo", menuName = "Scene Start Info")]
public class StartInfo : ScriptableObject
{
    public Vector2 StartPosition;
    public int Ammo;
    public float Health;
}
