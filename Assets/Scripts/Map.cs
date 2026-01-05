using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    public static Map Instance;
    [SerializeField] private List<GameObject> commonDrops;
    [SerializeField] private List<GameObject> uncommonDrops;
    [SerializeField] private List<GameObject> rareDrops;
    [SerializeField] private List<GameObject> epicDrops;
    [SerializeField] private List<GameObject> legendaryDrops;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void DropMapLoot(Vector3 location)
    {
        int random = Random.Range(0, 1000);

        if (random > 998)
        {
            if (legendaryDrops.Count == 0) return; 
            GameObject drop = PickRandom(legendaryDrops);
            Instantiate(drop, location, Quaternion.identity);
            legendaryDrops.Remove(drop);
        }
        else if (random > 990)
        {
            if (epicDrops.Count == 0) return; 
            GameObject drop = PickRandom(epicDrops);
            Instantiate(drop, location, Quaternion.identity);
            epicDrops.Remove(drop);
        }
        else if (random > 975)
        {
            if (rareDrops.Count == 0) return; 
            GameObject drop = PickRandom(rareDrops);
            Instantiate(drop, location, Quaternion.identity);
            rareDrops.Remove(drop);
        }
        else if (random > 950)
        {
            if (uncommonDrops.Count == 0) return; 
            GameObject drop = PickRandom(uncommonDrops);
            Instantiate(drop, location, Quaternion.identity);
            uncommonDrops.Remove(drop);
        }
        else if (random > 900)
        {
            if (commonDrops.Count == 0) return; 
            GameObject drop = PickRandom(commonDrops);
            Instantiate(drop, location, Quaternion.identity);
            commonDrops.Remove(drop);
        }
        
    }


    GameObject PickRandom(List<GameObject> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }
}
