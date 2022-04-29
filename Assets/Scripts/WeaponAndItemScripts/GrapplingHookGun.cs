using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


///The GrapplingHookGun script connects the grapplinghookgun to the UI and enable/disable the "hanginghook" animation.
public class GrapplingHookGun : MonoBehaviour
{
    public GameObject HookObject;
    public GameObject PlayerObject;
    public GrapplingHookController GrapplingHookControllerScript;
    public Sprite HookSymbol;
    private PolygonCollider2D _losCollider;
    private Image _imageUI;
    private Text _ammoText;
    private string _ammo = "∞";
    [SerializeField] private float _losConeWidth = 1f;
    private List<GameObject> _grapplingPoints = new List<GameObject>();
    private GameObject _closestGrapplingPoint = null;

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function the Player object, the GrapplingHookController and the Text component in the UI is located and assigned.*/
    void Start()
    {
        PlayerObject = GameObject.Find("Main_Character");
        GrapplingHookControllerScript = PlayerObject.GetComponent<GrapplingHookController>();
        _ammoText = GameObject.Find("AmmoTextUI").GetComponent<Text>();
        _losCollider = GetComponent<PolygonCollider2D>();
        SetLOSCollider();
    }

    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! The Update function enables/disables the "hook" object that is animated at the end of the gun (Only visual). 
    It also show the ammo text in the UI.*/
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
    ///This function is called when the object becomes enabled and active.

    /** In this function the UI image and ammo text of the gun is changed. Also the "IsUsingGrapplingHookGun" in the GrapplingHookControllerScript is changed to true.*/
    private void OnEnable()
    {
        _imageUI = GameObject.Find("currentWepImg").GetComponent<Image>();

        GrapplingHookControllerScript.IsUsingGrapplingHookGun = true;
        _imageUI.sprite = HookSymbol;
        _ammoText = GameObject.Find("AmmoTextUI").GetComponent<Text>();

    }

    ///This function is called when the behaviour becomes disabled.
    /**The "IsUsingGrapplingHookGun" in the GrapplingHookControllerScript is changed to false.*/
    private void OnDisable()
    {
        GrapplingHookControllerScript.IsUsingGrapplingHookGun = false;
    }

    private void SetLOSCollider()
    {
        Vector2 _pointA = PlayerObject.transform.position;
        Vector2 _pointB = new Vector2(PlayerObject.transform.position.x + GrapplingHookControllerScript.MaxDistance, PlayerObject.transform.position.y + _losConeWidth);
        Vector2 _pointC = new Vector2(PlayerObject.transform.position.x + GrapplingHookControllerScript.MaxDistance, PlayerObject.transform.position.y - _losConeWidth);

        _losCollider.SetPath(0, new Vector2[] { _pointA, _pointB, _pointC });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GrapplingHookControllerScript.IsHooked) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("GRAPPABLEOBJECTS"))
        {
            if(!_grapplingPoints.Contains(collision.gameObject))
            {
                _grapplingPoints.Add(collision.gameObject);

                if (!_closestGrapplingPoint)
                {
                    _closestGrapplingPoint = collision.gameObject;
                    _closestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    if(Vector3.Distance(PlayerObject.transform.position, _closestGrapplingPoint.transform.position) > Vector3.Distance(PlayerObject.transform.position, collision.transform.position))
                    {
                        _closestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = false;
                        _closestGrapplingPoint = collision.gameObject;
                        _closestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
            Debug.Log("Entered range");
            // Draw target on object
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GrapplingHookControllerScript.IsHooked) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("GRAPPABLEOBJECTS"))
        {
            if(_grapplingPoints.Contains(collision.gameObject))
            {
                _grapplingPoints.Remove(collision.gameObject);
            }

            if(collision.gameObject == _closestGrapplingPoint)
            {
                _closestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = false;
                if(_grapplingPoints.Count > 0)
                {
                    GameObject _closest = null;
                    foreach (GameObject _grapplingPoint in _grapplingPoints)
                    {
                        if (!_closest) _closest = _grapplingPoint;
                        if (Vector3.Distance(PlayerObject.transform.position, _closest.transform.position) > Vector3.Distance(PlayerObject.transform.position, _grapplingPoint.transform.position))
                        {
                            _closest = _grapplingPoint;
                        }
                    }
                    _closestGrapplingPoint = _closest;
                    _closestGrapplingPoint.transform.Find("Target").GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    _closestGrapplingPoint = null;
                }
            }
            Debug.Log("Exited range");
            // Remove target on object
        }
    }
}
