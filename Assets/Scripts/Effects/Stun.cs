using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Stun")]
public class Stun : EffectSO
{
    public float stunChance;
    public GameObject stunParticlesPrefab;

    public override void OnEnter(EffectInstance instance)
    {
        if (Random.Range(1,100) > 100 - (100 * stunChance))
        {
        instance.owner.animator.enabled = false;
        Instantiate(stunParticlesPrefab, instance.owner.spawnLoc.position, stunParticlesPrefab.transform.rotation);
        instance.owner.stunned++;
        Debug.Log(instance.duration);
        }
    }
    public override void Tick(EffectInstance instance)
    {

    }

    public override void OnStack(EffectInstance instance)
    {
        
    }


    public override void OnExit(EffectInstance instance)
    {
        Debug.Log("WE OUT");
        instance.owner.animator.enabled = true;
        instance.owner.stunned--; 
    }

    public override void OnOwnerDeath(EffectInstance instance)
    {
        
    }
}
