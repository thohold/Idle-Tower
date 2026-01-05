using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Reload Speed")]
public class AtkSpeedCard : UpgradeCard
{
    [SerializeField] private float multiplier = 0.8f;

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.canon != null) s.canon.AtkSpeed *= multiplier;
    }
}
