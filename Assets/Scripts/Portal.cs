using UnityEngine;
using System.Collections.Generic;

public class Portal : MonoBehaviour
{

    [field: SerializeField] public List<MobCard> SpawnList {get; set;}
    [field: SerializeField] public float SpawnRate {get; set;}
    [field: SerializeField] public int Capacity {get; set;}
    [field: SerializeField] public int MaxCost {get; set;}

    [Header("Golden Hue")]
    [field: SerializeField] public float GoldenHueChance {get; set;}
    [SerializeField] private Material GoldenHue;
    [SerializeField] private GameObject GoldenSparks;
    public int currentCost {get; set; }
    private float spawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > SpawnRate)
        {
            spawnTimer = 0;
            Spawn();
        }
    }


    void Spawn()
    {
        for (int i = 0; i < Capacity; i++)
        {
            if (SpawnList[i] != null)
            {
                GameObject e = SpawnList[i].mobPrefab;
                GameObject g = Instantiate(e, transform.position, Quaternion.identity);
                Enemy mob = g.GetComponent<Enemy>();
                GoldenHueSpawn(mob);
            }
        }
    }
    
    public void UpdateStats()
    {
        currentCost = GetTotalCost();
    }


    public int GetTotalCost()
    {
        int cost = 0;
        for (int i = 0; i < SpawnList.Count; i++)
        {
            cost += GetCardCost(i);
        }
        return cost;
    }

    public int GetCardCost(int index)
    {
        if (SpawnList[index] != null) return SpawnList[index].cost;
        else return 0;
    }

    public void GoldenHueSpawn(Enemy mob) 
    {
        if (Random.Range(1,1000) > 1000 - GoldenHueChance)
        {
            mob.presetMaterial = GoldenHue;
            mob.coins *= 5;
            Instantiate(GoldenSparks, mob.spawnLoc);
        }
    }
}
