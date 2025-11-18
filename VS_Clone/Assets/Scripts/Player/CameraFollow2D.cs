using UnityEngine;

[DisallowMultipleComponent]
public class CameraFollow2D : MonoBehaviour
{
    public Transform target;

    public float smoothTime = 0.15f; 
    private Vector3 vel;

  
    public float deadZoneWidth = 2.5f;
    public float deadZoneHeight = 1.5f;

   
    public Vector3 offset;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam) cam = Camera.main;
    }

    void LateUpdate()
    {
        if (!target) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position + offset;
        targetPos.z = camPos.z;

        
        float halfW = deadZoneWidth * 0.5f;
        float halfH = deadZoneHeight * 0.5f;

       
        Vector3 desired = camPos;

        if (targetPos.x > camPos.x + halfW) desired.x = targetPos.x - halfW;
        else if (targetPos.x < camPos.x - halfW) desired.x = targetPos.x + halfW;

        if (targetPos.y > camPos.y + halfH) desired.y = targetPos.y - halfH;
        else if (targetPos.y < camPos.y - halfH) desired.y = targetPos.y + halfH;

        
        Vector3 newPos = Vector3.SmoothDamp(camPos, desired, ref vel, smoothTime);
        transform.position = newPos;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.yellow;
        Vector3 center = Application.isPlaying ? (Vector3)(transform.position) : transform.position;
        Gizmos.DrawWireCube(center, new Vector3(deadZoneWidth, deadZoneHeight, 0.01f));
    }
#endif
}

