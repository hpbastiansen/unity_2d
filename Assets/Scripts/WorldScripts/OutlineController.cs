using System.Collections.Generic;
using UnityEngine;

/// This script controls the outline material applied to enemies after being debuffed by the player.
public class OutlineController : MonoBehaviour
{
    public Material DefaultMaterial;
    public Material OutlineMaterial;
    public bool IsDefaultMaterial;
    public GameObject OutlineLight;
    public List<SpriteRenderer> SpriteRenderers;

    /// Called before the first frame.
    /** Set all sprite renderers to use the default material. */
    void Start()
    {
        IsDefaultMaterial = true;
        foreach (var _sr in SpriteRenderers)
        {
            _sr.material = DefaultMaterial;
        }
    }

    /// Called every frame.
    /** If the IsDefaultMaterial boolean is flipped, set the spriterenderers materials to be the Outline material. */
    void Update()
    {
        if (IsDefaultMaterial)
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
