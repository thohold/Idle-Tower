using UnityEngine;

public enum Rarity 
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public abstract class UpgradeCard : ScriptableObject
{
    public string cardName;
    public string description;
    public string relation;
    public Sprite artwork;
    public Rarity rarity; 

    public int maxLevel = 1;

    public abstract void ApplyUpgrade(Structure tower, int level);
}