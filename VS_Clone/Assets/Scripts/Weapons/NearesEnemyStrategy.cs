using UnityEngine;
using System.Linq;

public class NearestEnemyStrategy : ITargetingStrategy
{
    public Transform SelectTarget(Vector3 origin)
    {
        var enemies = Object.FindObjectsOfType<EnemyController>();
        if (enemies.Length == 0) return null;
        return enemies.OrderBy(e => Vector3.SqrMagnitude(e.transform.position - origin))
                      .First().transform;
    }
}

