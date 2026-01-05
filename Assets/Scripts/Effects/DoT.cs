using UnityEngine;

[CreateAssetMenu(menuName = "Effects/DoT")]
public class DoT : EffectSO
{
    public int damage;
    public Element element;
    public GameObject tickEffect;

    public override void OnEnter(EffectInstance instance)
    {

    }
    public override void Tick(EffectInstance instance)
    {
        int finalDamage = Mathf.RoundToInt(damage * instance.strengthMultiplier) * instance.stacks;
   
        instance.owner.TakeDamage(new Damage(finalDamage, element, false));
        Instantiate(tickEffect, instance.owner.spawnLoc.position, Quaternion.identity);
    }


    public override void OnExit(EffectInstance instance)
    {
        
    }

    public override void OnOwnerDeath(EffectInstance instance)
    {
        
    }
}
