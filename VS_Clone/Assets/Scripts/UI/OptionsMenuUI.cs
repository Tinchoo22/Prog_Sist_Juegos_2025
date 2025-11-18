using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionTMP;
    [SerializeField] private TMP_Dropdown qualityTMP;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private Slider masterVolSlider;

    private Resolution[] resolutionsDistinct;

    private void OnEnable()
    {
        Populate();
        LoadFromSettings();
    }

    private void Populate()
    {
        resolutionsDistinct = Screen.resolutions
            .Select(r => new Resolution { width = r.width, height = r.height, refreshRateRatio = r.refreshRateRatio })
            .GroupBy(r => (r.width, r.height))
            .Select(g => g.First())
            .OrderBy(r => r.width * r.height)
            .ToArray();

        if (resolutionTMP)
        {
            resolutionTMP.ClearOptions();
            var options = resolutionsDistinct.Select(r => $"{r.width} x {r.height}").Distinct().ToList();
            resolutionTMP.AddOptions(options);
        }

        if (qualityTMP)
        {
            qualityTMP.ClearOptions();
            qualityTMP.AddOptions(QualitySettings.names.ToList());
        }
    }

    private void LoadFromSettings()
    {
        var s = SettingsManager.Instance.Data;

        int idx = 0;
        if (resolutionsDistinct != null && resolutionsDistinct.Length > 0)
        {
            for (int i = 0; i < resolutionsDistinct.Length; i++)
            {
                if (resolutionsDistinct[i].width == s.resolutionWidth &&
                    resolutionsDistinct[i].height == s.resolutionHeight)
                { idx = i; break; }
            }
        }

        if (resolutionTMP) resolutionTMP.value = idx;
        if (fullscreenToggle) fullscreenToggle.isOn = s.fullscreen;
        if (vSyncToggle) vSyncToggle.isOn = s.vSync;
        if (qualityTMP) qualityTMP.value = Mathf.Clamp(s.qualityIndex, 0, QualitySettings.names.Length - 1);
        if (masterVolSlider) masterVolSlider.value = s.masterVolume;
    }

    public void OnApply()
    {
        var s = SettingsManager.Instance;

        if (resolutionsDistinct != null && resolutionTMP)
        {
            int idx = Mathf.Clamp(resolutionTMP.value, 0, resolutionsDistinct.Length - 1);
            var r = resolutionsDistinct[idx];
            s.ApplyResolution(r.width, r.height, fullscreenToggle ? fullscreenToggle.isOn : s.Data.fullscreen);
        }

        if (vSyncToggle) s.ApplyVSync(vSyncToggle.isOn);
        if (qualityTMP) s.ApplyQuality(qualityTMP.value);
        if (masterVolSlider) s.ApplyMasterVolume(masterVolSlider.value);

        s.Save();
    }

    public void OnBack()
    {
        OnApply();
        gameObject.SetActive(false);
    }
}
