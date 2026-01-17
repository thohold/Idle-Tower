using UnityEngine;
using System.Collections.Generic;

public enum ProjectileType {
    Normal,
    Lobbed,
    Boomerang,
    Homing
}

public enum HitReaction {
    Direct,
    Aoe
}


public class Projectile : MonoBehaviour
{
    [field: SerializeField] public ProjectileType Type   { get;  set; }
    [field: SerializeField] public HitReaction Reaction   { get;  set; }
    [field: SerializeField] public GameObject Aoe   { get;  set; }
    [field: SerializeField] public Damage Damage   { get;  set; }
    [field: SerializeField] public float Speed   { get;  set; }
    [field: SerializeField] public float EffectStrengthMultiplier   { get;  set; }
    [field: SerializeField] public float EffectDurationMultiplier   { get;  set; }
    [field: SerializeField] public float Lifetime {get; set;}
    [field: SerializeField] public float Size {get; set;}
    [field: SerializeField] public float CritChance {get; set;}
    [field: SerializeField] public float CritDamage {get; set;}

    [field: SerializeField] private List<EffectSO> Effects;

    [field: SerializeField] private AudioClip hitSound;

    [SerializeField] private Rigidbody rb;
    private Vector3 direction;
    private GameObject target;
    [SerializeField] private float lobArcMultiplier = 0.5f; 


    [Header("Particle")]
    [SerializeField] private GameObject impactParticles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (Type)
        {
            case ProjectileType.Normal:
                if (direction == Vector3.zero && target != null)
                    SetDirection((target.transform.position - transform.position).normalized);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveHandler();

        Lifetime -= Time.deltaTime;

        if (Lifetime <= 0) Destroy(this.gameObject);
    }

    public void SetDirection(Vector3 dir)
    {
        this.direction = dir;
    }

    public void MoveHandler()
    {
        switch (Type)
        {
            case ProjectileType.Normal:
                if (direction == null) direction = (target.transform.position - transform.position).normalized;;
                transform.position += direction * Speed * Time.deltaTime;
                break;
            case ProjectileType.Lobbed:
                transform.rotation = Quaternion.LookRotation(rb.linearVelocity);
                transform.Rotate(90f, 0f, 0f); 
                break;
            case ProjectileType.Homing:
                direction = (target.transform.position - transform.position).normalized;;
                transform.position += direction * Speed * Time.deltaTime;
                break;
        }
    }

    public static Vector3 CalculateLobbedVelocity(Vector3 origin, Vector3 target, float timeToTarget)
    {
        Vector3 toTarget = target + new Vector3 (0, 0.5f, 0) - origin;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);

        float yOffset = toTarget.y;
        float g = Mathf.Abs(Physics.gravity.y);

        float Vy = (yOffset + 0.5f * g * timeToTarget * timeToTarget) / timeToTarget;
        Vector3 Vxz = toTargetXZ / timeToTarget;

        return Vxz + Vector3.up * Vy;
    }

    public void LaunchLobbed()
    {
        if (target == null || rb == null)
        {
            Debug.LogWarning("Missing target or Rigidbody for lobbed projectile.");
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        float timeToTarget = Mathf.Clamp(distance / (Speed * lobArcMultiplier), 0.1f, 1.5f);

        Vector3 velocity = CalculateLobbedVelocity(transform.position, target.transform.position, timeToTarget);
        rb.linearVelocity = velocity;
    }



    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void ApplyEffects(EffectHandler effectHandler)
    {
        foreach (EffectSO effectSO in Effects)
        {
            effectHandler.AddEffect(effectSO, EffectStrengthMultiplier, EffectDurationMultiplier, Size);
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (Reaction == HitReaction.Direct)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                EffectHandler effectHandler = other.GetComponent<EffectHandler>();
                
                ApplyEffects(effectHandler);
                Damage damage = Damage;
                if (Random.Range(1, 100) > (100 - (100 * CritChance))) 
                {
                    damage.amount = (int)(damage.amount * CritDamage);
                    damage.critical = true;
                }
                enemy.TakeDamage(damage);
            }
            else if (Reaction == HitReaction.Aoe) 
            {
                GameObject aoeObject = Instantiate(Aoe, transform.position, transform.rotation);
                AoE aoe = aoeObject.GetComponent<AoE>();
                aoe.InheritProjectile(this);
            }
            if (impactParticles != null) Instantiate(impactParticles, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayOneShot(hitSound, transform.position);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (impactParticles != null) Instantiate(impactParticles, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayOneShot(hitSound, transform.position);
            if (Reaction == HitReaction.Aoe) 
            {

                GameObject aoeObject = Instantiate(Aoe, transform.position, transform.rotation);
                AoE aoe = aoeObject.GetComponent<AoE>();
                aoe.InheritProjectile(this);
            }
            Destroy(this.gameObject); 

        }
    }
}
