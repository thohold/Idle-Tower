using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spawn/Rate")]
public class SpawnRateCard : UpgradeCard
{
    [SerializeField] private float multiplier = 0.7f;

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.portal != null) s.portal.SpawnRate *= multiplier;
    }
}