using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coins;
    public int xp;
    public List<Structure> structures = new List<Structure>();
    [field: SerializeField] public List<UpgradeCard> unlockedUpgradeCards {get; set;}
    [field: SerializeField] public List<UpgradeCard> unlockedStructureCards {get; set;}
    [field: SerializeField] public List<MobCard> unlockedMobCards {get; set;}



    public Structure currentSelectedStructure;
    public Enemy currentSelectedEntity;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        GameObject[] found = GameObject.FindGameObjectsWithTag("Structure");

        foreach (GameObject g in found)
        {
            structures.Add(g.GetComponent<Structure>());
        }
    }

    public void AddXp(int amount)
    {
        foreach (Structure s in structures)
        {
            s.Xp += amount;
        }
    }




}