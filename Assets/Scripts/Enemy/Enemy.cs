using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpawnEntry {
    public GameObject prefab;
    public int count;
}

public class Enemy : MonoBehaviour
{

    [Header("Movement")]
    private Vector3 currentDirection;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float directionInterval;
    [SerializeField] private float directionTimer;
    [SerializeField] private float gravityMultiplier = 3f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance;
    [SerializeField] private Transform groundCheck;

    private bool isGrounded;

    [Header("Stats")]
    [SerializeField] private float health;
    [SerializeField] public float baseMovementSpeed;
    private float movementSpeed;
    [SerializeField] public float movementSpeedMultiplier = 1f;
    [field: SerializeField] public int coins {get; set;}
    [field: SerializeField] public int LootMultiplier {get; set;}
    [field: SerializeField] public int xp {get; set;}
    
    [field: SerializeField] public Transform spawnLoc {get; set;}
    public int stunned {get; set;}


    [Header("Particles")]
    [SerializeField] private GameObject deathParticles;
    [SerializeField] protected GameObject damageText;

    [field: SerializeField] public Renderer rend {get; set;}

    private Material[] mats;
    private Material[] originalMats;
    [field: SerializeField] public Material presetMaterial {get; set;}
    [SerializeField] private Material hurtMaterial;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Visual")]
    [SerializeField] public Animator animator;
    [SerializeField] private Transform visual;
    [SerializeField] private Vector3 visualForwardOffset = new Vector3(0, 180f, 0);

    [Header("Audio")]
    [field: SerializeField] public AudioClip deathSound {get; set;}

    [Header("Random Wander")]
    [SerializeField] private float reachDistance;
    public BoxCollider wanderArea;
    private Vector3 wanderTarget;

    [SerializeField] private float wanderTargetInterval = 5f;
    private float wanderTimer;



    private EffectHandler effectHandler;


    void Awake()
    {
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wanderArea = GameObject.FindGameObjectWithTag("Grid").GetComponent<BoxCollider>();
        mats = rend.materials;
        originalMats = rend.materials;
        if (presetMaterial != null) LoadPresetMat();
        effectHandler = GetComponent<EffectHandler>();
        PickNewWanderTarget();
    }

    public void HitFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Material[] mats = rend.materials;
        for (int i = 0; i < mats.Length; i++)
            mats[i] = hurtMaterial;

        rend.materials = mats;

        yield return new WaitForSeconds(flashDuration);

        rend.materials = originalMats;

    }
    
    void FixedUpdate()
    {

        if (stunned <= 0) rb.linearVelocity = currentDirection * movementSpeed;

        if (!isGrounded) rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        wanderTimer += Time.deltaTime;
        movementSpeed = baseMovementSpeed * movementSpeedMultiplier;
        if (health <= 0) Die();

        CheckGrounded();

        RotateVisual();     

        if (Vector3.Distance(transform.position, wanderTarget) < reachDistance || wanderTimer > wanderTargetInterval)
        {
            PickNewWanderTarget();
            wanderTimer = 0f;
        }

        Vector3 toTarget = (wanderTarget - transform.position);
        toTarget.y = 0f;

        currentDirection = toTarget.normalized;



    }

    void CheckGrounded()
    {
        Vector3 origin = groundCheck.position;

        isGrounded = Physics.Raycast(origin, Vector3.down, groundDistance, groundLayer);

        if (animator)
            animator.SetBool("isGrounded", isGrounded);

        Debug.DrawRay(origin, Vector3.down * groundDistance, isGrounded ? Color.green : Color.red);
    }

    Vector3 GetRandomPointInBox(BoxCollider box)
    {
        Vector3 center = box.bounds.center;
        Vector3 size = box.bounds.size;

        float x = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float z = Random.Range(center.z - size.z / 2f, center.z + size.z / 2f);

        return new Vector3(x, transform.position.y, z);
    }

    void PickNewWanderTarget()
    {
        wanderTarget = GetRandomPointInBox(wanderArea);
    }



    void Die()
    {
        Instantiate(deathParticles, spawnLoc.position, Quaternion.identity);
        DropLoot();
        Map.Instance.DropMapLoot(spawnLoc.position);
        effectHandler.OnOwnerDeath();
        SoundManager.Instance.PlayOneShot(deathSound, spawnLoc.position);
        Destroy(this.gameObject);
    }

    void RotateVisual()
    {
        Vector3 flatDirection = new Vector3(currentDirection.x, 0f, currentDirection.z);

        if (flatDirection.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(flatDirection) * Quaternion.Euler(visualForwardOffset);

        visual.rotation = targetRotation;
    }

    

    public void TakeDamage(Damage damage)
    {
        HitFlash();
        health -= damage.amount;
        
        var text = Instantiate(damageText, transform.position + Vector3.up, Quaternion.identity);
        DamageText dmgText = text.GetComponent<DamageText>();
        dmgText.Init(damage);

    }

    void DropLoot()
    {
        LootHandler.Instance.SpawnCoins(coins, spawnLoc.position);
        LootHandler.Instance.SpawnXp(xp, spawnLoc.position);
    }

    void LoadPresetMat()
    {
        for (int i = 0; i < mats.Length; i++)
            originalMats[i] = presetMaterial;

        rend.materials = originalMats;
    }

}
