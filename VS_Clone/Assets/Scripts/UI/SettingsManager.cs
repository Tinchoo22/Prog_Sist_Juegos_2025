using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Serializable]
    public class SettingsData
    {
        public int resolutionWidth = 1920;
        public int resolutionHeight = 1080;
        public bool fullscreen = true;
        public bool vSync = false;
        public int qualityIndex = 2;
        public float masterVolume = 1f;
    }

    private const string KEY = "GameSettings_v1";
    public SettingsData Data { get; private set; } = new SettingsData();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
        ApplyAll();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string json = PlayerPrefs.GetString(KEY);
            try { Data = JsonUtility.FromJson<SettingsData>(json); }
            catch { Data = new SettingsData(); }
        }
        else Data = new SettingsData();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public void ApplyAll()
    {
        ApplyResolution(Data.resolutionWidth, Data.resolutionHeight, Data.fullscreen);
        ApplyVSync(Data.vSync);
        ApplyQuality(Data.qualityIndex);
        ApplyMasterVolume(Data.masterVolume);
    }

    public void ApplyResolution(int w, int h, bool fullscreen)
    {
        Screen.SetResolution(w, h, fullscreen);
        Data.resolutionWidth = w;
        Data.resolutionHeight = h;
        Data.fullscreen = fullscreen;
    }

    public void ApplyVSync(bool enabled)
    {
        QualitySettings.vSyncCount = enabled ? 1 : 0;
        Data.vSync = enabled;
    }

    public void ApplyQuality(int index)
    {
        index = Mathf.Clamp(index, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(index, true);
        Data.qualityIndex = index;
    }

    public void ApplyMasterVolume(float v)
    {
        v = Mathf.Clamp01(v);
        AudioListener.volume = v;
        Data.masterVolume = v;
    }
}
