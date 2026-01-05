using UnityEngine;
using System.Collections.Generic;

public class Vacus : MonoBehaviour
{
    private int goldAmount;
    private List<Collectible> collectibles = new List<Collectible>();

    private float collectTimer;
    [SerializeField] private float collectInterval;
    [SerializeField] private GameObject harvestParticles;
    [SerializeField] private Transform particlePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        collectTimer += Time.deltaTime;
        if (collectTimer > collectInterval)
        {
            collectTimer = 0;
            Collect();
        }
    }

    void Collect()
    {
        Instantiate(harvestParticles, particlePoint.position, Quaternion.identity);
        foreach (Collectible c in collectibles)
        {
            c.SetCollected();
        }
    }

    public void AddCollectible(Collectible c)
    {
        collectibles.Add(c);
    }

    public void Consume(Collectible c)
    {
        switch (c.GetType())
        {
            case CollectibleType.Coin:
                GameManager.Instance.coins += c.GetAmount();
                collectibles.Remove(c);
                break;
            case CollectibleType.Xp:
                GameManager.Instance.AddXp(c.GetAmount());
                collectibles.Remove(c);
                break;
        }
    }
}
