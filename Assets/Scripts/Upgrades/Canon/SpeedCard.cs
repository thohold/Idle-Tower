using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Speed")]
public class SpeedCard : UpgradeCard
{
    [SerializeField] private float addition = 1f;

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.canon != null) s.canon.Speed += addition;
    }
}