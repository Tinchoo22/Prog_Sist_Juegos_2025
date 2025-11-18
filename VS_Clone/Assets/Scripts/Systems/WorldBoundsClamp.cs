using UnityEngine;

public class WorldBoundsClamp : MonoBehaviour
{
    public Vector2 min;
    public Vector2 max;

    private void LateUpdate()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, min.x, max.x);
        p.y = Mathf.Clamp(p.y, min.y, max.y);
        transform.position = p;
    }
}

