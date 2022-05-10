// https://www.youtube.com/watch?v=sHhzWlrTgJo, https://www.youtube.com/watch?v=DTFgQIs5iMY

using UnityEngine;

/// The GrapplingHookController is a script that is assigned to the player object. The script's purpose is to control and manage all aspect of the grapplinghookgun that is related to physics.
public class GrapplingHookController : MonoBehaviour
{
    public Rigidbody2D PlayerRigidbody;
    public bool IsHooked;
    public GameObject HookedPoint;
    public float EndForce;
    public LineRenderer LineObject;
    DistanceJoint2D _joint;
    Vector3 _targetPosition;
    RaycastHit2D _hit;
    public float Distance = 10;
    public LayerMask WhatToHit;
    public float UpDownSpeed = 0.02f;
    public bool IsUsingGrapplingHookGun;
    public DialogueManager DialogueManagerScript;
    public Rigidbody2D TempJoint;
    public Movement MovementScript;
    public AudioSource HookAudioSource;
    public AudioSource JumpBoostAudioSource;
    public float MaxDistance = 5f;
    private LineOfSight _lineOfSight;
    private GameObject _lastHookedPoint = null;


    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. */
    /*! In the Start function the DistanceJoint2D component, Line of Sight cone, and the DialogueManager is located and assigned. The DistanceJoin2D, 
    the line and the boolean for using the grapplinghookgun is made sure to be disabled.*/
    void Start()
    {
        _joint = GetComponent<DistanceJoint2D>();
        _joint.enabled = false;
        LineObject.enabled = false;
        IsUsingGrapplingHookGun = false;
        DialogueManagerScript = GameObject.Find("Dialogue_Manager").GetComponent<DialogueManager>();
        MovementScript = FindObjectOfType<Movement>();
        _lineOfSight = transform.Find("Arm/WeaponHolder/Grapplinghook/LineOfSight").GetComponent<LineOfSight>();
    }

    /// Fixed Update is called based on a fixed frame rate.
    /** FixedUpdate has the frequency of the physics system; it is called every fixed frame-rate frame. Compute Physics system calculations after FixedUpdate. 0.02 seconds (50 calls per second) is the default time between calls. */
    /*! The FixedUpdate allowes the player to move up and down the grapplinghook rope. 
    It adds a force either to the left or to the right based on the position on the hook and the player when going up.
    This is to help prevent jagged movement when moving alongside a wall (or other collider) with friction. 
    Lastly it adds a small Impulse Force to the player when the position of the player is less than 0.05 from the hook position.
    This helps the player get up on the edge. If the player is above the hook, and moving upwards, their upwards velocity is cancelled to prevent weird movement. */
    private void FixedUpdate()
    {
        if (_joint.distance > .05f && IsHooked == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                _joint.distance -= UpDownSpeed;
                if (_targetPosition.x > PlayerRigidbody.transform.position.x)
                {
                    PlayerRigidbody.AddForce(Vector3.left * EndForce / 1000, ForceMode2D.Impulse);
                }
                else
                {
                    PlayerRigidbody.AddForce(Vector3.right * EndForce / 1000, ForceMode2D.Impulse);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (_joint.distance < MaxDistance) _joint.distance += UpDownSpeed;
            }
        }
        if (_joint.distance < .05f)
        {
            if (IsHooked == true)
            {
                PlayerRigidbody.AddForce(Vector3.up * EndForce, ForceMode2D.Impulse);
                HookedPoint = null;
                IsHooked = false;
                gameObject.GetComponent<Movement>().DashAnimation.SetActive(false);
                JumpBoostAudioSource.Play();
            }
            LineObject.enabled = false;
            _joint.enabled = false;
        }

        if(IsHooked)
        {
            if(PlayerRigidbody.position.y > HookedPoint.transform.position.y && PlayerRigidbody.velocity.y > 0)
            {
                PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, 0);
            }
        }
    }

    /// This method makes the player release their current grappling hook point.
    public void ReleaseGrapple()
    {
        IsHooked = false;
        _lastHookedPoint = HookedPoint;
        HookedPoint = null;
        _joint.enabled = false;
        LineObject.enabled = false;
        gameObject.GetComponent<Movement>().DashAnimation.SetActive(false);
        _joint.connectedBody = TempJoint;
    }

    /// Update is called every frame.
    /** The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
    This means that is a game run on higher frames per second the update function will also be called more often. */

    /*! This method works in conjunction to the LineOfSight script, getting the closest grappling point within line of sight.
        While hooked, if the player clicks or presses G, they release the current grappling point. If they click while another grappling point 
        is in range, they connect to that point instead. They also cannot use the grappling hook gun while in a dialogue.
        When the hook is initiated the line object will be set to enabled and drawn from the centre of the player to the hook position. 
        When the hook is deattached, the line will be removed. */
    void Update()
    {
        if (!_joint.connectedBody)
        {
            _joint.connectedBody = TempJoint;
        }

        if ((Input.GetMouseButtonDown(0) && IsHooked == true && IsUsingGrapplingHookGun == true) || (IsHooked && IsUsingGrapplingHookGun == false && Input.GetKeyDown(KeyCode.G)))
        {
            ReleaseGrapple();
        }

        if (Input.GetMouseButtonDown(0) && IsUsingGrapplingHookGun == true && DialogueManagerScript.InDialogue == false && _lineOfSight.ClosestGrapplingPoint && _lastHookedPoint != _lineOfSight.ClosestGrapplingPoint)
        {
            _targetPosition = _lineOfSight.ClosestGrapplingPoint.transform.position;

            _hit = Physics2D.Raycast(transform.position, _targetPosition - transform.position, Distance, WhatToHit);
            HookAudioSource.Play();
            IsHooked = true;
            HookedPoint = _lineOfSight.ClosestGrapplingPoint;
            MaxDistance = HookedPoint.GetComponent<GrapplingPoint>().MaxDistance;
            _joint.enabled = true;
            Vector2 connectPoint = _hit.point - new Vector2(_hit.collider.transform.position.x, _hit.collider.transform.position.y);
            connectPoint.x = connectPoint.x / _hit.collider.transform.localScale.x;
            connectPoint.y = connectPoint.y / _hit.collider.transform.localScale.y;
            _joint.connectedAnchor = connectPoint;

            _joint.connectedBody = _hit.collider.gameObject.GetComponent<Rigidbody2D>();
            _joint.distance = Vector2.Distance(transform.position, _hit.point);

            LineObject.enabled = true;
            LineObject.SetPosition(0, transform.position);
            LineObject.SetPosition(1, _hit.point);

            LineObject.GetComponent<RopeRatio>().PositionToHook = _hit.point;
        }

        LineObject.SetPosition(1, _joint.connectedBody.transform.TransformPoint(_joint.connectedAnchor));

        if (_joint.enabled == true && LineObject.enabled == true)
        {
            LineObject.SetPosition(0, transform.position);
        }

        _lastHookedPoint = null;
    }

    ///If the gameObject is disabled we make sure we set the connected distance joint to be a object we know always will exist in the scene.
    private void OnDisable()
    {
        _joint.connectedBody = TempJoint;
    }
}