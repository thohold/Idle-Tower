using UnityEngine;
using System.Collections.Generic;

public enum StructureType {
    Deployed,
    Vacus,
    Portal
}
public class Structure : MonoBehaviour
{
    
    [Header("Info")]
    [field: SerializeField] public string Name   { get;  set; }
    [field: SerializeField] public string Description   { get;  set; }
    [field: SerializeField] public Sprite CardSprite   { get;  set; }
    [field: SerializeField] public StructureType Type   { get;  set; }


    [Header("Level")]
    [field: SerializeField] public int Xp   { get;  set; }
    [field: SerializeField] public int NextXp   { get;  set; }
    [field: SerializeField] public float NextXpAddition   { get;  set; }
    [field: SerializeField] public int Level   { get;  set; }

    [Header("Upgrades")]
    [field: SerializeField] public List<UpgradeCard> allowedCards {get; set;}
    private Dictionary<UpgradeCard, int> cardLevels = new Dictionary<UpgradeCard, int>();

    [Header("Structure Components")]
    [field: SerializeField] public Canon canon { get;  set; }
    [field: SerializeField] public Portal portal { get; set;}

    

    public bool HasMaxLevel(UpgradeCard card)
    {
        return cardLevels.ContainsKey(card) && cardLevels[card] >= card.maxLevel;
    }

    public int GetCardLevel(UpgradeCard card)
    {
        if (cardLevels.TryGetValue(card, out int level))
        {
            return level;
        }
        else
        {
            return 0;
        }
    }

    public void ApplyCard(UpgradeCard card)
    {
        if (!cardLevels.ContainsKey(card))
            cardLevels[card] = 1;
        else
            cardLevels[card]++;
            Level++;
        if (HasMaxLevel(card)) allowedCards.Remove(card);
        card.ApplyUpgrade(this, cardLevels[card]);
    }

}
