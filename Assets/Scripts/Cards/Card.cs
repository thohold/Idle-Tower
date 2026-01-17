using UnityEngine;

public enum Rarity 
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public abstract class Card : ScriptableObject
{
    public Sprite artwork;
    public Rarity rarity; 
}