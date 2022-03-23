// https://www.youtube.com/watch?v=9hTnlp9_wX8 Creating the worm body
// https://www.youtube.com/watch?v=BfP0KyOxVWs Collision for line renderer

using System.Collections.Generic;
using UnityEngine;

///The WormBody script handles the drawing and collision of the worm enemy's body.

public class WormBody : MonoBehaviour
{
    private WormCollision _wormCollision;
    private LineRenderer _lineRend;

    [Header("Body Length")]
    [SerializeField] private int _minLength = 4;
    [SerializeField] private int _maxLength = 8;
    private int _length;
    
    [Header("Body Segments")]
    [SerializeField] private Transform _targetDir;
    [SerializeField] private float _targetDist;
    [SerializeField] private float _smoothSpeed;
    [SerializeField] private float _trailSpeed;

    [Header("Wiggle")]
    [SerializeField] private Transform _wiggleDir;
    [SerializeField] private float _wiggleSpeed;
    [SerializeField] private float _wiggleMagnitude;

    private Vector3[] _segmentPoses;
    private Vector3[] _segmentV;

    /// Start methods run once when enabled.
    /**Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.*/
    /*! In the Start function we find the necessary components, choose the worm's length and prepare the Line Renderer.*/
    private void Start()
    {
        _wormCollision = transform.root.GetComponent<WormCollision>();
        _lineRend = GetComponent<LineRenderer>();
        _length = (int)Mathf.Floor(Random.Range(_minLength, _maxLength));
        _lineRend.positionCount = _length;
        _segmentPoses = new Vector3[_length];
        _segmentV = new Vector3[_length];
        ResetPos();
    }

    ///Update is called every frame.
    /**The Update function is FPS dependent, meaning it will update as often as it possibly can based on a change of frames. 
     * This means that is a game run on higher frames per second the update function will also be called more often.*/
    /*! Note: This update fuction is fairly long, and more documentation will be written "inline" in the code itself.*/
    /*! The update function moves the body segments sequentially, creating a natural flowing movement. We also draw the Polygon Collider around the segments. */
    private void Update()
    {
        // Changes the wiggle rotation based on the current time passed into a sine function.
        _wiggleDir.localRotation = Quaternion.Euler(0, 0, (Mathf.Sin(Time.time * _wiggleSpeed) * _wiggleMagnitude) + 270);

        // Sets the position of the first body segment manually.
        _segmentPoses[0] = _targetDir.position;

        // Moves the other body segments smoothly behind the first one.
        for(int _i = 1; _i < _segmentPoses.Length; _i++)
        {
            _segmentPoses[_i] = Vector3.SmoothDamp(_segmentPoses[_i], _segmentPoses[_i - 1] + _targetDir.right * _targetDist, ref _segmentV[_i], _smoothSpeed + _i / _trailSpeed);
        }
        _lineRend.SetPositions(_segmentPoses);
        _wormCollision.SetCollision(_segmentPoses, _length, _lineRend.startWidth);
    }

    ///Resets the positions of the Line Renderer so they do not start at (0, 0, 0) in world space. Used only at instantiation.
    private void ResetPos()
    {
        _segmentPoses[0] = _targetDir.position;
        for(int _i = 1; _i < _length; _i++)
        {
            _segmentPoses[_i] = _segmentPoses[_i - 1] + _targetDir.right * _targetDist;
        }
        _lineRend.SetPositions(_segmentPoses);
    }
}
