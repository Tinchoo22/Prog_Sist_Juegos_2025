using UnityEngine;

public class PausePanelAnchor : MonoBehaviour
{
    private void Start()
    {
        if (GameFacade.I) GameFacade.I.RegisterPausePanel(gameObject);
    }
}
