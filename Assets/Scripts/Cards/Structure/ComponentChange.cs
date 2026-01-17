using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Empty Upgrade")]
public class EmptyUpgrade : UpgradeCard
{

    public override void ApplyUpgrade(Structure s, int level)
    {
    }
}