using UnityEngine;

public class EnemyConfig : ScriptableObject
{
    public string id;
    public float speed = 2f;
    public int maxHP = 3;
    public int damage = 1;
    public int xpDrop = 1;
    public Sprite sprite;
}
