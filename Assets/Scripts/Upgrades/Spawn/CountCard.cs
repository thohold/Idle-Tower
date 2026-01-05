using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Spawn/Count")]
public class CountCard : UpgradeCard
{
    [SerializeField] private int addition = 1;

    public override void ApplyUpgrade(Structure s, int level)
    {
        if (s.portal != null) 
        {
            s.portal.Capacity += addition;
            s.portal.SpawnList.Add(null);
        }

    }
}