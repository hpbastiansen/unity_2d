/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*https://www.youtube.com/watch?v=sHhzWlrTgJo&list=PUuXkaW-PS6zmJ5zO4FbiiXQ&index=2&ab_channel=DonHaulGameDev-Wabble-UnityTutorials*/
/*https://www.youtube.com/watch?v=DTFgQIs5iMY&list=PUuXkaW-PS6zmJ5zO4FbiiXQ&index=11&ab_channel=DonHaulGameDev-Wabble-UnityTutorials*/
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class grapplinghook : MonoBehaviour
{
    public Movement mv;
    public Rigidbody2D rb;
    public bool isHooked;
    public float forces;
    public LineRenderer line;
    DistanceJoint2D joint;
    Vector3 targetPos;
    RaycastHit2D hit;
    public float distance = 10f;
    public LayerMask mask;
    public float step = 0.02f;
    public bool usingthisgun;

    // Use this for initialization
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        line.enabled = false;
        mv = gameObject.GetComponent<Movement>();
        usingthisgun = false;
    }

    // Update is called once per frame
    void Update()
    {


        if (joint.distance > .05f && isHooked == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                joint.distance -= step;
                if (targetPos.x > rb.transform.position.x)
                {
                    rb.AddForce(Vector3.left * forces / 1000, ForceMode2D.Impulse);

                }
                else
                {
                    rb.AddForce(Vector3.right * forces / 1000, ForceMode2D.Impulse);

                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                joint.distance += step;

            }
            //mv.enabled = false;
        }

        else
        {
            if (isHooked == true)
            {
                rb.AddForce(Vector3.up * forces, ForceMode2D.Impulse);
                isHooked = false;
                gameObject.GetComponent<Movement>().DashAnimation.SetActive(false);
            }
            line.enabled = false;
            joint.enabled = false;
            //mv.enabled = true;
        }


        if ((Input.GetMouseButtonDown(0) && isHooked == true && usingthisgun == true) || (isHooked && usingthisgun == false && Input.GetKeyDown(KeyCode.G)))
        {
            isHooked = false;
            joint.enabled = false;
            line.enabled = false;
            gameObject.GetComponent<Movement>().DashAnimation.SetActive(false);

        }
        if (Input.GetMouseButtonDown(0) && usingthisgun == true)
        {

            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;

            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)

            {
                isHooked = true;
                joint.enabled = true;
                Vector2 connectPoint = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
                connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
                joint.connectedAnchor = connectPoint;

                joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                joint.distance = Vector2.Distance(transform.position, hit.point);

                line.enabled = true;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);

                line.GetComponent<roperatio>().grabPos = hit.point;


            }
        }
        line.SetPosition(1, joint.connectedBody.transform.TransformPoint(joint.connectedAnchor));

        if (joint.enabled == true && line.enabled == true)
        {
            line.SetPosition(0, transform.position);
        }


    }
}