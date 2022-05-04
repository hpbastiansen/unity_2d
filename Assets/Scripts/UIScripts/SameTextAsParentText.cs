using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SameTextAsParentText : MonoBehaviour
{
    public Text _parentText;
    public Text _myText;
    void Start()
    {
        _myText = gameObject.GetComponent<Text>();
        _parentText = transform.parent.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _myText.text = _parentText.text;
    }
}
