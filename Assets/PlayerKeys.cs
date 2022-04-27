using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeys : MonoBehaviour
{
    [HideInInspector] public List<string> Keys = new List<string>();
    // Start is called before the first frame update

    public void AddKey(string _name)
    {
        Keys.Add(_name);
    }

    public bool HasKey(string _name)
    {
        return Keys.Contains(_name);
    }
}
