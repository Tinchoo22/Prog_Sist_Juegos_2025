using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerHud : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText; 
    [SerializeField] private string prefix = "Survive ";
    [SerializeField] private int targetSeconds = 300; // 5 Minutos
    [SerializeField] private bool showCountdown = false;
    [SerializeField] private bool throttleToWholeSeconds = true;

    private int lastShownValue = -999;

    private void Start()
    {
        float rt = GameSession.Instance ? GameSession.Instance.RunTime : 0f;
        ForceUpdate(rt);
    }

    private void Update()
    {
        var gs = GameSession.Instance;
        if (!gs) return;

        float rt = gs.RunTime;
        int sec = Mathf.Clamp(Mathf.RoundToInt(rt), 0, 35999);

        if (throttleToWholeSeconds && sec == lastShownValue) return;

        lastShownValue = sec;
        Apply(sec);
    }

    private void ForceUpdate(float runtimeSeconds)
    {
        int sec = Mathf.Clamp(Mathf.RoundToInt(runtimeSeconds), 0, 35999);
        lastShownValue = -999; 
        Apply(sec);
    }

    private void Apply(int elapsedSeconds)
    {
        string timeText;
        if (showCountdown)
        {
            int remaining = Mathf.Max(0, targetSeconds - elapsedSeconds);
            timeText = FormatMMSS(remaining);
        }
        else
        {
            timeText = FormatMMSS(elapsedSeconds);
        }

        string finalText = string.IsNullOrEmpty(prefix) ? timeText : (prefix + timeText);

        if (tmpText) tmpText.text = finalText;
    }

    private string FormatMMSS(int totalSeconds)
    {
        int m = totalSeconds / 60;
        int s = totalSeconds % 60;
        return $"{m:00}:{s:00}";
    }
      
    public void SetPrefix(string newPrefix)
    {
        prefix = newPrefix ?? "";
        lastShownValue = -999; 
    }
}
