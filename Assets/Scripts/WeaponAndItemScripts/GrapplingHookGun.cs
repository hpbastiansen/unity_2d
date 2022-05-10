using UnityEngine;
using UnityEngine.UI;

///The GrapplingHookGun script connects the grapplinghookgun to the UI and enables/disables the "hanginghook" animation.
public class GrapplingHookGun : MonoBehaviour
{
    public GameObject HookObject;
    public GameObject PlayerObject;
    public GrapplingHookController GrapplingHookControllerScript;
    public Sprite HookSymbol;
    private Image _imageUI;
    private Text _ammoText;
    private string _ammo = "∞";

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. */
    /*! In the Start function the Player object, the GrapplingHookController and the Text component in the UI is located and assigned. */
    void Start()
    {
        PlayerObject = GameObject.Find("Main_Character");
        GrapplingHookControllerScript = PlayerObject.GetComponent<GrapplingHookController>();
        _ammoText = GameObject.Find("AmmoTextUI").GetComponent<Text>();
    }

    /// Update is called every frame.
    /** The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often. */
    /*! The Update function enables/disables the "hook" object that is animated at the end of the gun (Only visual). 
    It also show the ammo text in the UI. */
    void Update()
    {
        if (GrapplingHookControllerScript.IsHooked)
        {
            HookObject.SetActive(false);
        }
        else
        {
            HookObject.SetActive(true);
        }
        _ammoText.text = _ammo.ToString();
    }

    /// This function is called when the object becomes enabled and active.
    /** In this function the UI image and ammo text of the gun is changed. Also the "IsUsingGrapplingHookGun" in the GrapplingHookControllerScript is changed to true. */
    private void OnEnable()
    {
        _imageUI = GameObject.Find("currentWepImg").GetComponent<Image>();
        GrapplingHookControllerScript.IsUsingGrapplingHookGun = true;
        _imageUI.sprite = HookSymbol;
        _ammoText = GameObject.Find("AmmoTextUI").GetComponent<Text>();

    }

    /// This function is called when the behaviour becomes disabled.
    /** The "IsUsingGrapplingHookGun" in the GrapplingHookControllerScript is changed to false. */
    private void OnDisable()
    {
        GrapplingHookControllerScript.IsUsingGrapplingHookGun = false;
    }
}
