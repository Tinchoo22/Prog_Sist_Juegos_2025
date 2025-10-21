
using System.Collections.Generic;
using UnityEngine;

public interface IFirePattern
{
    IEnumerable<Vector2> GetDirections(Vector2 ownerPos, Transform target, Vector2 baseDir);
}

