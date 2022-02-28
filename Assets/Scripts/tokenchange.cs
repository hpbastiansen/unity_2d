using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class tokenchange : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt;
    void Start()
    {
        txt = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2);
    }
}
