using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Effect Duration")]
public class EffectDurationCard : UpgradeCard
{
    [SerializeField] private float modifierAdd = 0.2f;

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.canon != null) s.canon.EffectDurationMultiplier += modifierAdd;
    }
}