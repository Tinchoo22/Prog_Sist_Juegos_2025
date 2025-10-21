using UnityEngine;
public class WeaponConfig : ScriptableObject
{

    public string id;
    public float fireRate = 1f;     
    public int damage = 1;
    public float projectileSpeed = 10f;
    public int projectilesPerShot = 1;   
    public float fireRange = 6f;

    public bool hasPiercing;
    [Min(0)] public int pierces = 0;

    public bool hasBoomerang;
    [Min(0.1f)] public float boomerangOutDistance = 5f;
    [Min(0.1f)] public float boomerangReturnSpeedMult = 1.2f;

    public bool hasHoming;
    [Min(10f)] public float homingTurnRateDegPerSec = 120f;
    [Min(1f)] public float homingSeekRadius = 6f;

    public bool useSpread;                     
    [Min(2)] public int spreadCount = 3;         
    [Min(0f)] public float spreadAngleDeg = 30f; 

    public bool useBurst;                       
    [Min(2)] public int burstShots = 3;        
    [Min(0.05f)] public float burstInterval = 0.08f;

    public bool useMultiShot;        
    [Min(2)] public int multiShotCount = 2;
}
