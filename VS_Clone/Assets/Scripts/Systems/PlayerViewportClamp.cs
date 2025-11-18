using UnityEngine;

[DisallowMultipleComponent]
public class PlayerViewportClamp : MonoBehaviour
{
  
    public Camera cam;
        
    public float margin = 0.2f;

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null || !cam.orthographic) return;
               
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        Vector3 pos = transform.position;
        Vector3 cpos = cam.transform.position;

        float minX = cpos.x - horzExtent + margin;
        float maxX = cpos.x + horzExtent - margin;
        float minY = cpos.y - vertExtent + margin;
        float maxY = cpos.y + vertExtent - margin;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
