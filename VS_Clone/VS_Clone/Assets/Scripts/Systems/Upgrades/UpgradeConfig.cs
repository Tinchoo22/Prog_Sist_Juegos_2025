using UnityEngine;

public abstract class UpgradeConfig : ScriptableObject
{
    [Header("Identidad")]
    public string id;               
    public string title;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Nivel")]
    [Min(1)] public int maxLevel = 1;
     
    public abstract void Apply(GameSession session, int newLevel);
}



