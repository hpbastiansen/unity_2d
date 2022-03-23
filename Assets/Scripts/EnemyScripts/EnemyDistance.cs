///////////////////////////////////////////////////////
/*http://answers.unity.com/answers/1425815/view.html*/
///////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class EnemyDistance : MonoBehaviour
{
    #region FIELDS_&_PROPERTIES

    static List<EnemyDistance> _instances = new List<EnemyDistance>(100);
    public static List<EnemyDistance> instances { get { return _instances; } }

    /// <summary> Same as transform, but little less costly to access </summary>
    Transform _transform;

    /// <summary> return _transform.position </summary>
    public Vector3 position { get { return _transform.position; } }

    #endregion
    #region MONOBEHAVIOUR_METHODS

    void Awake()
    {
        //cache reference to Transform component:
        _transform = transform;
    }

    void OnEnable()
    {
        //register instance:
        _instances.Add(this);
    }

    void OnDisable()
    {
        //unregister instance:
        _instances.Remove(this);
    }

    #endregion
    #region PUBLIC_METHODS

    public static EnemyDistance FindNearest(Vector3 point)
    {
        EnemyDistance nearest = null;

#if ENABLE_PROFILER
        Profiler.BeginSample("FindNearest()");
#endif
        {
            Vector3 nearestPosition = Vector3.zero;
            float nearestDistance = float.PositiveInfinity;
            {
                int instancesCount = _instances.Count;
                int i = 0;
                if (instancesCount > 0)
                {
                    nearest = _instances[0];
                    nearestPosition = nearest._transform.position;
                    nearestDistance = Vector3.Distance(point, nearestPosition);
                    i = 1;
                }
                for (; i < instancesCount; i++)
                {
                    EnemyDistance next = _instances[i];
                    Vector3 nextPosition = next._transform.position;
                    float dist = Vector3.Distance(point, nextPosition);
                    if (dist < nearestDistance)
                    {
                        nearest = next;
                        nearestPosition = next._transform.position;
                        nearestDistance = dist;
                    }
                }
            }
        }
#if ENABLE_PROFILER
        Profiler.EndSample();
#endif

        return nearest;
    }

    #endregion
}