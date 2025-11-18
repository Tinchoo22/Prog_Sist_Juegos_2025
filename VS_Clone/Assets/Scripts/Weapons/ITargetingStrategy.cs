using UnityEngine;

public interface ITargetingStrategy
{
    Transform SelectTarget(Vector3 origin);
}
