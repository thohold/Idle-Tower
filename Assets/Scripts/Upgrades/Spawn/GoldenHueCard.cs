using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spawn/Golden Hue")]
public class GoldenHueCard : UpgradeCard
{
    [SerializeField] private float addition = 25; // 25 = 2.5%

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.portal != null) s.portal.GoldenHueChance += addition;
    }
}