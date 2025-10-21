using UnityEngine;

public class EnemyArchetypeConfig : ScriptableObject
{
    [Header("ID y Prefab")]
    public string id;                    
    public GameObject prefab;         

    [Header("Stats base")]
    public int baseHP = 5;
    public float moveSpeed = 2f;
    public int contactDamage = 1;
    public int xpDrop = 1;

    [Header("Opcional")]
    public Sprite overrideSprite;     

    
    public AnimationCurve hpByMinute = AnimationCurve.Linear(0, 1, 30, 3);
    public AnimationCurve speedByMinute = AnimationCurve.Linear(0, 1, 30, 1.4f);

    public void ApplyScaling(float minutes, ref int hp, ref float speed)
    {
        float hpMul = Mathf.Max(0.1f, hpByMinute.Evaluate(minutes));
        float spMul = Mathf.Max(0.1f, speedByMinute.Evaluate(minutes));
        hp = Mathf.RoundToInt(baseHP * hpMul);
        speed = moveSpeed * spMul;
    }
}
