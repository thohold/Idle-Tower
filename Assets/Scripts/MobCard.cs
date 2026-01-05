using UnityEngine;

[CreateAssetMenu(menuName = "Mobs/Mob Card")]
public class MobCard : ScriptableObject
{
    public string name;
    public Rarity rarity;
    public Sprite artwork;
    public GameObject mobPrefab;
    public int cost;
}
