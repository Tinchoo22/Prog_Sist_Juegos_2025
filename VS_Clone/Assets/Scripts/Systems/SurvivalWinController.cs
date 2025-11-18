using UnityEngine;

public class SurvivalWinController : MonoBehaviour
{
    [SerializeField] private int targetSeconds = 300; // 5 minutos
    private bool fired;

    private void Update()
    {
        if (fired || GameSession.Instance == null) return;

        if (GameSession.Instance.RunTime >= targetSeconds)
        {
            fired = true;
            TriggerWin();
        }
    }

    private void TriggerWin()
    {
        GameFacade.I?.Victory();
    }
}

