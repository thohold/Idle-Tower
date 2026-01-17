using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Effect Strength")]
public class EffectStrengthCard : UpgradeCard
{
    [SerializeField] private float modifierAdd = 0.5f;

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.canon != null) s.canon.EffectStrengthMultiplier += modifierAdd;
    }
}