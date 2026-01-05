using UnityEngine;

public class LootHandler : MonoBehaviour
{
    public static LootHandler Instance;

    [Header("Coins")]
    [SerializeField] private GameObject coin1;
    [SerializeField] private GameObject coin10;
    [SerializeField] private GameObject coin100;

    [Header("Xp")]
    [SerializeField] private GameObject xp1;
    [SerializeField] private GameObject xp10;
    [SerializeField] private GameObject xp100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnCoins(int value, Vector3 position)
    {
        int gold = value / 100;
        value %= 100;

        int silver = value / 10;
        value %= 10;

        int copper = value;

        Spawn(coin100, gold, position);
        Spawn(coin10, silver, position);
        Spawn(coin1, copper, position);
    }

        public void SpawnXp(int value, Vector3 position)
    {
        int xp100s = value / 100;
        value %= 100;

        int xp10s = value / 10;
        value %= 10;

        int xp1s = value;

        Spawn(xp100, xp100s, position);
        Spawn(xp10, xp10s, position);
        Spawn(xp1, xp1s, position);
    }

    void Spawn(GameObject prefab, int count, Vector3 pos)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 0.5f;
            offset.y = Mathf.Abs(offset.y);

            Instantiate(prefab, pos + offset, prefab.transform.rotation);
        }
    }

}
