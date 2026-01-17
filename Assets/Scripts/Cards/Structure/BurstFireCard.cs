using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Burst")]
public class BurstFireCard : UpgradeCard
{
    [SerializeField] private int amount;

    public override void ApplyUpgrade(Structure s, int level)
    {
        s.canon.magazineSize += amount;
    }
}