using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Javelin Thrower/Iron Tip")]
public class ProjectileChange : UpgradeCard
{
    [SerializeField] private GameObject bulletPreviewPrefab;
    [SerializeField] private GameObject bulletPrefab;

    public override void ApplyUpgrade(Structure s, int level)
    {

        Canon canon = s.GetComponentInChildren<Canon>();
        canon.bullet = bulletPrefab;

        canon.bulletPreview = bulletPreviewPrefab;
        canon.UpdateAmmoVisuals();
        s.allowedCards.RemoveAll(upgrade => upgrade is ProjectileChange);
    }
}