using UnityEngine;

/// This script allows the boss' weakpoint to be damaged.
public class BossWeakpointStage4 : MonoBehaviour
{
    [SerializeField] private float _health;

    /// Update is called once per frame.
    /** Destroy the gameobject if health is below 0 and activate the stage 4 cutscene. */
    void Update()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
            GameObject.Find("CutsceneManager").GetComponent<CutsceneManagerStage4>().Activate();
        }
    }

    /// Other scripts can call this function to lower health.
    public void TakeDamage(float dmg)
    {
            _health = _health - dmg;
    }
}
