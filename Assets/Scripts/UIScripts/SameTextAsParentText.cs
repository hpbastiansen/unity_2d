using UnityEngine;
using UnityEngine.UI;

/// This script sets the text of the gameobject it's connected to to the parent's text.
public class SameTextAsParentText : MonoBehaviour
{
    public Text _parentText;
    public Text _myText;

    /// Called before the first frame.
    void Start()
    {
        _myText = gameObject.GetComponent<Text>();
        _parentText = transform.parent.GetComponent<Text>();
    }

    /// Called every frame.
    void Update()
    {
        _myText.text = _parentText.text;
    }
}
