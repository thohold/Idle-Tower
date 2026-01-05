using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Burst")]
public class BurstFireCard : UpgradeCard
{
    [SerializeField] private List<GameObject> magazinePrefabs;
    [SerializeField] private int amount;

    public override void ApplyUpgrade(Structure s, int level)
    {
        s.canon.UpdateComponent(magazinePrefabs[level-1], 2);
        s.canon.magazineSize += amount;
        s.canon.UpdateAmmoVisuals();
    }
}