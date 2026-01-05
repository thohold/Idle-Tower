using UnityEngine;
[CreateAssetMenu(menuName = "Structure/card")]
public class StructureCard : ScriptableObject
{
    public Sprite artwork;
    public Rarity rarity;
    public int price;
    public GameObject prefab;
    public GameObject preview;


}
