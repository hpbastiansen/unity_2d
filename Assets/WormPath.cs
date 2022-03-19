using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPath : MonoBehaviour
{
    public Vector2 startPoint;
    public Vector2 playerPoint;
    public Vector2 endPoint;
    Vector2 bestMiddlePoint;

    bool pathSet = false;
    public int pointsAmt = 20;
    List<Vector2> points;
    int currentPoint = 0;

    public float speed = 200f;
    public float nextPointDistance = 1f;

    Rigidbody2D rb;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        bestMiddlePoint = CalculateBestMiddlePoint(startPoint, endPoint, playerPoint);
        points = CalculateRoute();

        Debug.Log("Start point: " + startPoint);
        Debug.Log("Middle point: " + bestMiddlePoint);
        Debug.Log("End point: " + endPoint);
        for (int i = 0; i < points.Count; i++)
        {
            Debug.Log("Point " + i + ": " + points[i].x + ", " + points[i].y);
        }
        pathSet = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(currentPoint);
        if (!pathSet) return;

        // Remove enemy if path is complete.
        if(currentPoint >= points.Count)
        {
            Destroy(gameObject);
        }

        // Find direction to next waypoint
        Vector3 direction = (points[currentPoint] - rb.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position = Vector2.MoveTowards(transform.position, points[currentPoint], speed * Time.deltaTime);
        
        //Vector2 force = direction * speed * Time.deltaTime;
        //rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, points[currentPoint]);
        if(distance < nextPointDistance)
        {
            currentPoint++;
        }
    }

    // http://answers.unity.com/answers/1700931/view.html
    Vector2 CalculateMiddlePoint(Vector2 aStart, Vector2 aEnd, float aTime, Vector2 aPoint)
    {
        float t = aTime;
        float rt = 1f - t;
        return 0.5f * (aPoint - rt * rt * aStart - t * t * aEnd) / (t * rt);
    }

    float CalculateMiddleTime(Vector2 aStart, Vector2 aEnd, Vector2 aPoint)
    {
        float a = Vector2.Distance(aPoint, aStart);
        float b = Vector2.Distance(aPoint, aEnd);
        return a / (a + b);
    }

    Vector2 CalculateBestMiddlePoint(Vector2 aStart, Vector2 aEnd, Vector2 aPoint)
    {
        return CalculateMiddlePoint(aStart, aEnd, CalculateMiddleTime(aStart, aEnd, aPoint), aPoint);
    }

    // Standard quadratic bezier curve formula
    Vector2 CalculateQuadraticBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector2 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    // Calculate an amount of points along a bezier curve.
    List<Vector2> CalculateRoute()
    {
        List<Vector2> route = new List<Vector2>();
        for(int i = 0; i < pointsAmt; i++)
        {
            float t = (float)i / pointsAmt;
            route.Add(CalculateQuadraticBezierPoint(t, startPoint, bestMiddlePoint, endPoint));
        }
        return route;
    }
}
