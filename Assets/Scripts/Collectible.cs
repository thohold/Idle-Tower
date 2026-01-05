using UnityEngine;
public enum CollectibleType
{
    Coin,
    Xp
}
public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType type;
    [SerializeField] private int amount;
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float acceleration = 5f;
    private Vector3 direction;
    private bool collected = false;
    private GameObject vacus;
    private Vacus vacusScript;
    private Rigidbody rb;
    private Collider collider;

    [Header("Particles")]
    [SerializeField] private GameObject consumeParticle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Random.onUnitSphere;
        speed = minSpeed;
        vacus = GameObject.FindGameObjectWithTag("Vacus");
        vacusScript = vacus.GetComponent<Vacus>();
        vacusScript.AddCollectible(this);
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        rb.AddForce(new Vector3(0,1,0));
    }

    // Update is called once per frame
    void Update()
    {
        if (collected) 
        {
            direction = (vacus.transform.position - transform.position).normalized;
            rb.useGravity = false;
            rb.linearVelocity = direction * speed;
        }
        if (speed < maxSpeed && collected) speed += acceleration * Time.deltaTime;
    }

    public void SetCollected()
    {
        collected = true;
        gameObject.layer = LayerMask.NameToLayer("Collectible(collected)");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vacus"))
        {
            vacusScript.Consume(this);
            Instantiate(consumeParticle, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public CollectibleType GetType()
    {
        return type;
    }

    public int GetAmount()
    {
        return amount;
    }
}
