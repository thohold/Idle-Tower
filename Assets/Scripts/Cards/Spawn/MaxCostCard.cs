using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spawn/Max Cost")]
public class MaxCostCard : UpgradeCard
{

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.portal != null) s.portal.MaxCost++;
    }
}