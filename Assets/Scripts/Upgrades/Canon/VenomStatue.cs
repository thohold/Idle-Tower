using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Venom Statue")]
public class VenomStatue : UpgradeCard
{
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private GameObject dartPrefab;

    public override void ApplyUpgrade(Structure s, int level)
    {
        s.canon.UpdateComponent(basePrefab, 1);

        Canon canon = s.GetComponentInChildren<Canon>();
        canon.bullet = dartPrefab;
    }
}