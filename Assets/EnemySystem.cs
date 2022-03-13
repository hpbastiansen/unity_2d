using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [Header("Spawn range")]
    public float maxSpawnRange = 10f;
    public float minSpawnRange = 5f;
    public int maxWorms = 2;

    [Header("Spawn time")]
    public float spawnTimeMin = 10f;
    public float spawnTimeMax = 20f;

    [Header("Objects")]
    public GameObject player;
    public GameObject worm;

    float timeSinceSpawn = 0f;
    float lastCheck = 0f;
    List<GameObject> wormPoints = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        wormPoints.AddRange(GameObject.FindGameObjectsWithTag("WormSpawn"));
        Debug.Log(wormPoints.Count);
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
    }

    void CheckShouldSpawn()
    {
        if (timeSinceSpawn < spawnTimeMin) return;

        float spawnChance = (timeSinceSpawn - spawnTimeMin) / 10f;

        if (Random.value < spawnChance || timeSinceSpawn > spawnTimeMax)
        {
            List<GameObject> availablePoints = FindPointsInRange();
            if (availablePoints.Count >= 2)
            {
                List<GameObject> chosenPoints = SelectPoints(availablePoints);
                SpawnWorm(chosenPoints);
            }
            else
            {
                Debug.Log("Could not find two worm points.");
            }
        }
    }

    void SpawnWorm(List<GameObject> points)
    {
        Vector3 center = FindCircleCenter(points[0].transform.position, points[1].transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z));
        timeSinceSpawn = 0f;
        GameObject spawnedWorm = Instantiate(worm, center, Quaternion.FromToRotation(Vector3.zero, points[0].transform.position));
        spawnedWorm.transform.Find("Worm").transform.position = points[0].transform.position;
    }

    List<GameObject> FindPointsInRange()
    {
        List<GameObject> points = new List<GameObject>();

        foreach (GameObject wormPoint in wormPoints)
        {
            float distance = Vector3.Distance(wormPoint.transform.position, player.transform.position);
            if(distance < maxSpawnRange && distance > minSpawnRange)
            {
                points.Add(wormPoint);
            }
        }

        return points;
    }

    List<GameObject> SelectPoints(List<GameObject> points)
    {
        List<GameObject> chosenPoints = new List<GameObject>();
        while(chosenPoints.Count < 2)
        {
            int index = (int)Mathf.Floor(Random.Range(0f, points.Count));
            if(!chosenPoints.Contains(points[index]))
            {
                chosenPoints.Add(points[index]);
            }
        }
        return chosenPoints;
    }

    Vector3 FindCircleCenter(Vector3 a, Vector3 b, Vector3 c)
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

        // Get line intersection
        return FindIntersection(new Vector3(x1, y1, 0), new Vector3(x1+dx1, y1+dy1, 0), new Vector3(x2, y2, 0), new Vector3(x2+dx2, y2+dy2, 0));
    }

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
    }
}
