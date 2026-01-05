using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Multishot")]
public class MultiShotCard : UpgradeCard
{
    [SerializeField] private List<GameObject> barrelPrefabs;

    public override void ApplyUpgrade(Structure s, int level)
    {
        s.canon.UpdateComponent(barrelPrefabs[level-1], 0);
        s.canon.UpdateAmmoVisuals();
    }
}