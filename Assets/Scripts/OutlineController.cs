using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private MaterialManager _materialManager;
    public Material DefaultMaterial;
    public Material OutlineMaterial;
    public bool IsDefualtMaterial;
    public GameObject OutlineLight;

    public List<SpriteRenderer> SpriteRenderers;
    // Start is called before the first frame update
    void Start()
    {
        _materialManager = Object.FindObjectOfType<MaterialManager>();
        IsDefualtMaterial = true;
        foreach (var _sr in SpriteRenderers)
        {
            _sr.material = DefaultMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            IsDefualtMaterial = !IsDefualtMaterial;
        }

        if (IsDefualtMaterial)
        {
            foreach (var _sr in SpriteRenderers)
            {
                _sr.material = DefaultMaterial;
            }
            OutlineLight.SetActive(false);
        }
        else
        {
            foreach (var _sr in SpriteRenderers)
            {
                _sr.material = OutlineMaterial;
            }
            OutlineLight.SetActive(true);
        }
    }
}
