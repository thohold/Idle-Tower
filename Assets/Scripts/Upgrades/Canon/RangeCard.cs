using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Range")]
public class RangeCard : UpgradeCard
{
    [SerializeField] private float multiplier = 1.2f;

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.canon != null) s.canon.Range *= multiplier;
    }
}