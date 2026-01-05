using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Venom")]
public class Venom : EffectSO
{
    public int damage;
    public Element element;
    public GameObject tickEffect;
    public GameObject deathAoe;

    public override void OnEnter(EffectInstance instance)
    {

    }
    public override void Tick(EffectInstance instance)
    {
        int finalDamage = Mathf.RoundToInt(damage * instance.strengthMultiplier) * instance.stacks;
   
        instance.owner.TakeDamage(new Damage(finalDamage, element, false));
        Instantiate(tickEffect, instance.owner.transform.position, Quaternion.identity);
    }


    public override void OnExit(EffectInstance instance)
    {
        
    }

    public override void OnOwnerDeath(EffectInstance instance)
    {
        GameObject aoe = Instantiate(deathAoe, instance.owner.spawnLoc.position, Quaternion.identity);;
        AoE aoeScript = aoe.GetComponent<AoE>();
        aoeScript.effectDurationMultiplier = instance.durationMultiplier;
        aoeScript.effectStrengthMultiplier = instance.strengthMultiplier;
    }
}
