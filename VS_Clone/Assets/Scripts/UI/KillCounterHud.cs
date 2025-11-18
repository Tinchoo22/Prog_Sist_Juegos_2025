using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillCounterHud : MonoBehaviour
{    
    [SerializeField] private TMP_Text counterTMP;       
    private void OnEnable() { EventBus.OnEnemyKilled += UpdateText; }
    private void OnDisable() { EventBus.OnEnemyKilled -= UpdateText; }
    private void Start()
    {
     
        if (counterTMP == null) counterTMP = GetComponentInChildren<TMP_Text>(true);

        int start = GameSession.Instance ? GameSession.Instance.KillCount : 0;
        UpdateText(start);
    }

    private void UpdateText(int total)
    {
        string msg = $"Kills: {total}";
        if (counterTMP != null) counterTMP.text = msg;
    }
}

