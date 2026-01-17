using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AoE : MonoBehaviour
{
    [SerializeField] private int times;
    [SerializeField] private float interval = 1f;
    private float tickTimer;
    [SerializeField] private List<EffectSO> effects;

    [Header("Stats")]

    [field: SerializeField] public Damage Damage   { get;  set; }
    [field: SerializeField] public float Lifetime {get; set;}
    [field:SerializeField] public float EffectStrengthMultiplier {get; set;}
    [field:SerializeField] public float EffectDurationMultiplier {get; set;}
    [field: SerializeField] public float Size {get; set;}
    [field: SerializeField] public float CritChance {get; set;}
    [field: SerializeField] public float CritDamage {get; set;}
    [SerializeField] private GameObject particlesOnTrigger;
    [SerializeField] private LayerMask enemyMask;
    private HashSet<Enemy> enemiesInside = new();
    private bool initialTrigger;


    void Start()
    {
        transform.localScale *= Size;

        StartCoroutine(DelayedFirstTrigger());
    }
    void Update()
    {
        if (initialTrigger) tickTimer += Time.deltaTime;

        if (tickTimer > interval)
        {
            Trigger();
            tickTimer = 0;
        }
    }

    void Trigger() 
    {
        foreach (Enemy enemy in enemiesInside)
        {
            enemy.TakeDamage(Damage);

            var ef = enemy.GetComponent<EffectHandler>();
            if (ef != null)
                foreach (var effect in effects)
                    ef.AddEffect(effect, EffectStrengthMultiplier, EffectDurationMultiplier, Size);
        }

        GameObject particles = Instantiate(particlesOnTrigger, transform.position, Quaternion.identity);
        particles.transform.localScale *= Size;

        times--;
        if (times <= 0) Destroy(gameObject);
    }

    IEnumerator DelayedFirstTrigger()
    {
        yield return new WaitForFixedUpdate();
        Trigger();
        initialTrigger = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
            enemiesInside.Add(enemy);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
            enemiesInside.Remove(enemy);
    }

    public void InheritProjectile(Projectile projectile)
    {
        Damage = projectile.Damage;
        Size = projectile.Size;
        EffectDurationMultiplier = projectile.EffectDurationMultiplier;
        EffectStrengthMultiplier = projectile.EffectStrengthMultiplier;
        CritChance = projectile.CritChance;
        CritDamage = projectile.CritDamage;
    }

}
