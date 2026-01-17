using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Curse")]
public class Curse : EffectSO
{
    public int damage;
    public GameObject tickEffect;

    public override void OnEnter(EffectInstance instance)
    {
    }
    public override void Tick(EffectInstance instance)
    {
        int finalDamage = Mathf.RoundToInt(damage * instance.strengthMultiplier) * instance.stacks;
        instance.owner.TakeDamage(new Damage(finalDamage, Element.Arcane, false));
        Instantiate(tickEffect, instance.owner.spawnLoc.position, Quaternion.identity);
    }

    public override void OnStack(EffectInstance instance)
    {
        instance.resistanceModifiers[Element.Arcane] = -0.1f * instance.stacks;
    }


    public override void OnExit(EffectInstance instance)
    {
    }

    public override void OnOwnerDeath(EffectInstance instance)
    {
        
    }
}
