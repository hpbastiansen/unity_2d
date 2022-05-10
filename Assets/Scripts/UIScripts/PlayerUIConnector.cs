using UnityEngine;
using UnityEngine.UI;

///The PlayerUIConnector script allows the UI to see and change it values based on the health scripts of the player.
public class PlayerUIConnector : MonoBehaviour
{
    public Slider HealthBar;
    public Text HPText;
    public PlayerHealth PlayerHealthScript;
    [SerializeField] private Slider _counterCooldown;

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. */
    /*! The start function locates the PlayerHealth script attached to the player. */
    void Start()
    {
        PlayerHealthScript = GameObject.Find("Main_Character").GetComponent<PlayerHealth>();
    }

    /// Update is called every frame.
    /** The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often. */
    /*! The Update function updates the UI Slider representation of the player health and counter cooldown. */
    void Update()
    {
        HealthBar.maxValue = PlayerHealthScript.MaxHP;
        HealthBar.value = PlayerHealthScript.CurrentHP;
        HPText.text = (PlayerHealthScript.CurrentHP.ToString("F0") + " / " + PlayerHealthScript.MaxHP.ToString("F0")).ToString();
        _counterCooldown.maxValue = PlayerHealthScript.BlockCooldownTime;
        _counterCooldown.value = (PlayerHealthScript.TempBlockTimer > 0 ? PlayerHealthScript.TempBlockTimer : 0);
    }
}
