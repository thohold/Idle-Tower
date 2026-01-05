using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AoE : MonoBehaviour
{
    [SerializeField] private int times;
    [SerializeField] private float interval = 1f;
    private float tickTimer;
    [SerializeField] private Damage damage;
    [SerializeField] private List<EffectSO> effects;
    [field:SerializeField] public float effectStrengthMultiplier {get; set;}
    [field:SerializeField] public float effectDurationMultiplier {get; set;}
    public float size {get; set;}
    [SerializeField] private GameObject particlesOnTrigger;
    [SerializeField] private LayerMask enemyMask;
    private HashSet<Enemy> enemiesInside = new();
    private bool initialTrigger;


    void Start()
    {
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
            enemy.TakeDamage(damage);

            var ef = enemy.GetComponent<EffectHandler>();
            if (ef != null)
                foreach (var effect in effects)
                    ef.AddEffect(effect, effectStrengthMultiplier, effectDurationMultiplier, size);
        }

        Instantiate(particlesOnTrigger, transform.position, Quaternion.identity);

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

}
