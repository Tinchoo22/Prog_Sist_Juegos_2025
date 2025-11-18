using UnityEngine;

public class XPOrb : MonoBehaviour
{

    [SerializeField] private int amount = 1;

    [SerializeField] private float attractDistance = 3f;
    [SerializeField] private float speed = 8f;
     
    [SerializeField] private ParticleSystem idleParticles;     
    [SerializeField] private TrailRenderer trail;              
    [SerializeField] private ParticleSystem pickupBurstPrefab;

    [SerializeField] private bool useTint = false;
    [SerializeField] private Color tint = new Color(0.6f, 1f, 0.6f, 1f);

    private Transform player;
    private bool collected;

    public static void Drop(Vector2 pos, int amount)
    {
        var prefab = Resources.Load<GameObject>("XPOrb");
        var go = Object.Instantiate(prefab, pos, Quaternion.identity);
        var orb = go.GetComponent<XPOrb>();
        if (orb) orb.amount = amount;
    }

    private void OnEnable()
    {
        collected = false;
        TryFindPlayer();

        if (useTint) ApplyTint();

        if (idleParticles) idleParticles.Play(true);
        if (trail) trail.emitting = true;
    }

    private void OnDisable()
    {
        if (idleParticles) idleParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (trail) trail.emitting = false;
    }

    private void Update()
    {
        if (!player) TryFindPlayer();
        if (!player || collected) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attractDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;

        GameSession.Instance?.AddXP(amount);

        if (pickupBurstPrefab)
        {
            var burst = Instantiate(pickupBurstPrefab, transform.position, Quaternion.identity);
            if (useTint)
            {
                var main = burst.main;
                main.startColor = tint;
            }
            burst.Play(true);
    
        }

        GameFacade.I?.PlaySfx("xp_pickup");

        Destroy(gameObject); 
    }


    private void TryFindPlayer()
    {
        var p = GameObject.FindWithTag("Player");
        if (p) player = p.transform;
    }

    private void ApplyTint()
    {
      
        if (idleParticles)
        {
            var main = idleParticles.main;
            main.startColor = tint;
        }


        if (trail)
        {
            var grad = new Gradient();
            var c0 = tint; c0.a = 1f;
            var c1 = tint; c1.a = 0f;
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(c0, 0f), new GradientColorKey(c0, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
            );
            trail.colorGradient = grad;
        }
    }
}
