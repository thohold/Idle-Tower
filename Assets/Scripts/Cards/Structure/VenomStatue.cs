using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Venom Statue")]
public class VenomStatue : UpgradeCard
{
    [SerializeField] private GameObject dartPrefab;

    public override void ApplyUpgrade(Structure s, int level)
    {
        Canon canon = s.GetComponentInChildren<Canon>();
        canon.bullet = dartPrefab;
    }
}