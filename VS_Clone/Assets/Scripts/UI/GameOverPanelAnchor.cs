using UnityEngine;
public class GameOverPanelAnchor : MonoBehaviour
{
    private void Start()
    {
        if (GameFacade.I) GameFacade.I.RegisterGameOverPanel(gameObject);
    }
}

