using UnityEngine;

public class EventPump : MonoBehaviour
{
    private void Update() => EventBus.Pump();
}
