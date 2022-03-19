using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [Header("Spawn range")]
    public float maxSpawnRange = 15f;
    public float minSpawnRange = 4f;
    public int maxWorms = 2;

    [Header("Spawn time")]
    public float spawnTimeMin = 5f;
    public float spawnTimeMax = 10f;

    [Header("Objects")]
    public GameObject player;
    public GameObject worm;

    float timeSinceSpawn = 0f;
    float lastCheck = 0f;
    GameObject[] wormPoints;

    // Start is called before the first frame update
    void Start()
    {
        wormPoints = GameObject.FindGameObjectsWithTag("WormSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        lastCheck += Time.deltaTime;
        int wormAmount = GameObject.FindGameObjectsWithTag("Worm").Length;

        if(lastCheck > 1f && wormAmount < maxWorms)
        {
            lastCheck = 0f;
            CheckShouldSpawn();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            timeSinceSpawn = 10f;
        }
    }

    void CheckShouldSpawn()
    {
        if (timeSinceSpawn < spawnTimeMin) return;

        // Spawn chance is 0% at spawnTimeMin, 100% at spawnTimeMax.
        float spawnChance = (timeSinceSpawn - spawnTimeMin) / 10f;

        // Get a random number between 0 and 1. If number is lower than spawn chance,
        // check if a worm is able to be spawned and if it is - spawn it.
        if (Random.value < spawnChance || timeSinceSpawn > spawnTimeMax)
        {
            List<Vector2> selectedPoints = SelectPoints();
            SpawnWorm(selectedPoints);
        }
    }

    // Returns a list of two points for the worm to travel to and from, one on the left side of the player, one on the right side.
    List<Vector2> SelectPoints()
    {
        List<Vector2> chosenPoints = new List<Vector2>();
        List<Vector2> leftPoints = new List<Vector2>();
        List<Vector2> rightPoints = new List<Vector2>();

        foreach (GameObject point in wormPoints)
        {
            float distance = Vector3.Distance(point.transform.position, player.transform.position);
            if(distance < maxSpawnRange && distance > minSpawnRange)
            {
                if (point.transform.position.x < player.transform.position.x)
                {
                    leftPoints.Add(point.transform.position);
                }
                else
                {
                    rightPoints.Add(point.transform.position);
                }
            }
        }

        Vector2 leftPoint = leftPoints[(int)Mathf.Floor(Random.Range(0f, leftPoints.Count))];
        Vector2 rightPoint = rightPoints[(int)Mathf.Floor(Random.Range(0f, rightPoints.Count))];
        Vector2 playerPoint = new Vector2(player.transform.position.x, player.transform.position.y + 0.2f);

        if(Vector2.Distance(leftPoint, playerPoint) > Vector2.Distance(rightPoint, playerPoint))
        {
            chosenPoints.Add(leftPoint);
            chosenPoints.Add(playerPoint);
            chosenPoints.Add(rightPoint);
        } else
        {
            chosenPoints.Add(rightPoint);
            chosenPoints.Add(playerPoint);
            chosenPoints.Add(leftPoint);
        }

        return chosenPoints;
    }

    void SpawnWorm(List<Vector2> points)
    {
        timeSinceSpawn = 0f;

        // Instantiate worm and set path points.
        GameObject spawnedWorm = Instantiate(worm, points[0], Quaternion.Euler(0, 0, 0));
        WormPath path = spawnedWorm.GetComponent<WormPath>();
        path.startPoint = points[0];
        path.playerPoint = points[1];
        path.endPoint = points[2];
        path.Init();
    }

    // Finds the center of a circle given three points a, b, and c. Returns (0, 0, 0) if points are colinear.
/*    Vector3 FindCircleCenter(Vector3 a, Vector3 b, Vector3 c)
    {
        // Get perpendicular bisector of (x1, y1) and (x2, y2)
        float x1 = (b.x + a.x) / 2;
        float y1 = (b.y + a.y) / 2;
        float dy1 = b.x - a.x;
        float dx1 = -(b.y - a.y);

        // Get perpendicular bisector of (x2, y2) and (x3, y3)
        float x2 = (c.x + b.x) / 2;
        float y2 = (c.y + b.y) / 2;
        float dy2 = c.x - b.x;
        float dx2 = -(c.y - b.y);

        // Get line intersection to find center of circle.
        return FindIntersection(new Vector3(x1, y1, 0), new Vector3(x1+dx1, y1+dy1, 0), new Vector3(x2, y2, 0), new Vector3(x2+dx2, y2+dy2, 0));
    }

    // Finds the intersection given two lines. Returns (0, 0, 0) on no intersection.
    Vector3 FindIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float dx12 = p2.x - p1.x;
        float dy12 = p2.y - p1.y;
        float dx34 = p4.x - p3.x;
        float dy34 = p4.y - p3.y;

        float denominator = (dy12 * dx34 - dx12 * dy34);
        float t1 = ((p1.x - p3.x) * dy34 + (p3.y - p1.y) * dx34) / denominator;
        if (float.IsInfinity(t1)) return Vector3.zero;
        float t2 = ((p3.x - p1.x) * dy12 + (p1.y - p3.y) * dx12) / -denominator;

        return new Vector3(p1.x + dx12 * t1, p1.y + dy12 * t1, 0);
    }*/
}
