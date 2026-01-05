using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Damage")]
public class DamageCard : UpgradeCard
{
    [SerializeField] private int addition = 1;

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.canon != null) s.canon.Damage += addition;
    }
}