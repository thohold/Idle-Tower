using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum AimType {
    ToEnemy,
    FromBarrel
}

public enum TargetType {
    Closest,
    Random
}
public class Canon : MonoBehaviour
{
    private Structure Structure;
    [SerializeField] private float baseRange;
    public float range {get; set;}
    [SerializeField] private List<GameObject> barrels;
    private Quaternion baseRotation;

    [SerializeField] private bool rotateYAxis;
    

    [SerializeField] private AimType aimType;

    [Header("Stats")]
    [field: SerializeField] public float AtkSpeed { get;  set; }
    [field: SerializeField] public float Range    { get;  set; }
    [field: SerializeField] public float Size     { get;  set; }
    [field: SerializeField] public int Damage   { get;  set; }
    [field: SerializeField] public float Speed   { get;  set; }
    [field: SerializeField] public float EffectStrengthMultiplier   { get;  set; }
    [field: SerializeField] public float EffectDurationMultiplier   { get;  set; }

    [Header("Barrel")]

    [field: SerializeField] public GameObject bullet {get; set;}
    [field: SerializeField] public GameObject bulletPreview {get; set;}

    private Projectile bulletConfig;
    private float reloadTimer;
    private bool shooting;
    [field: SerializeField] public List<EffectSO> Effects {get; set;}

    // BURST

    [field: SerializeField] public int magazineSize {get; set;}
    private int currentMagazine;
    [SerializeField] private float magazineInterval;
    private float magazineTimer;

    private Vector3 baseSize;
    private Vector3 size;
    [SerializeField] private float baseAtkSpeed;
    public float atkSpeed {get; set;}
    [SerializeField] private float baseSpeed;
    public float speed {get; set;}

    private SphereCollider collider;
    private GameObject currentTarget;
    private Vector3 lookDirection;
    private List<GameObject> targetsInRange = new List<GameObject>();
    private List<GameObject> targets = new List<GameObject>();
    private GameObject target;
    [SerializeField] private bool switchTargets;

    [field: SerializeField] public AudioClip fireSound {get; set;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Structure = transform.parent.gameObject.GetComponent<Structure>();
        bulletConfig = bullet.GetComponent<Projectile>();
        collider = GetComponent<SphereCollider>();
        UpdateAmmoVisuals();
        collider.radius = range;
        baseRotation = transform.localRotation;
        baseSize = bulletConfig.transform.localScale;
        Reset();    
    }

    // Update is called once per frame
    void Update()
    {
        targets = GetClosestEnemies();

        if (targets.Count > 0)
        {
            GameObject mainTarget = targets[0]; 

            RotateCanon(mainTarget);

        }

        if (currentMagazine <= 0) 
        {
            reloadTimer += Time.deltaTime;
            shooting = false;
        }
        if (shooting) magazineTimer += Time.deltaTime;

        if (reloadTimer > atkSpeed && targets.Count > 0)
        {
            reloadTimer = 0;
            Reload();
            shooting = true;
        }
        if (magazineTimer > magazineInterval && shooting && currentMagazine > 0)
        {
            magazineTimer = 0;
            currentMagazine--;
            Shoot();
        }


    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered!");
            targetsInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy left!");
            targetsInRange.Remove(other.gameObject);    
        }
    }


    List<GameObject> GetClosestEnemies()
    {
        return targetsInRange
            .Where(e => e != null)
            .OrderBy(e => (e.transform.position - transform.position).sqrMagnitude)
            .Take(barrels.Count)
            .ToList();
    }

    public void Reset()
    {
        atkSpeed = baseAtkSpeed * AtkSpeed;
        range = baseRange * Range;
        collider.radius = range;
        speed = baseSpeed * Speed;
        size = baseSize * Size;

        UpdateAmmoVisuals();
        UpdateBarrels();

    }

    public void UpdateBarrels()
    {
        barrels.Clear();
        if (Structure.components.TryGetValue(StructurePartSlot.Barrel, out var b))
            {
                Transform barrel = b.currentInstance.transform;
            foreach(Transform child in barrel)
                {
                    barrels.Add(child.gameObject);
                }
            }
    }
    

    public void UpdateAmmoVisuals()
    {
        foreach (VisualHolder holder in GetComponentsInChildren<VisualHolder>())
        {
            holder.SwitchVisual(bulletPreview);
        }

    }


    private void Shoot()
    {
          
            for (int i = 0; i < barrels.Count; i++)
            {
                GameObject barrel = barrels[i];
                GameObject target = null;
                if (targets.Count == 0) return;
                if (switchTargets)
                {
                    target = i < targets.Count ? targets[i] : targets[0];
                }
                else
                {
                    target = targets[0];
                }
                
                GameObject p = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
                SoundManager.Instance.PlayOneShot(fireSound, transform.position);
                Projectile projectile = p.GetComponent<Projectile>();
                projectile.transform.localScale = size;
                projectile.Speed = speed;
                projectile.EffectStrengthMultiplier = EffectStrengthMultiplier;
                projectile.EffectDurationMultiplier = EffectDurationMultiplier;
                projectile.Size = Size;
                var dmg = projectile.Damage;
                dmg.amount = Damage;
                projectile.Damage = dmg;

                switch (aimType)
                {
                    case AimType.ToEnemy:   
                        projectile.SetTarget(target);
                        break;
                    case AimType.FromBarrel:
                        projectile.SetDirection(p.transform.up); // Taking account of blenders rotation
                        break;
                }

                if (projectile.Type == ProjectileType.Lobbed) projectile.LaunchLobbed(); 
            }  
        }

    private void Reload()
    {
        currentMagazine = magazineSize;
    }

    private void RotateCanon(GameObject mainTarget)
    {
        lookDirection = mainTarget.transform.position - transform.position;

        // --- Yaw (Z axis) ---
        Vector3 flatDir = new Vector3(lookDirection.x, 0f, lookDirection.z);
        float yaw = (Mathf.Atan2(flatDir.x, flatDir.z) * Mathf.Rad2Deg) - 180f;

        // --- Pitch (X axis) ---
        float pitch = 0f;
        if (rotateYAxis) // rename this later to rotatePitch maybe
        {
            float horizontalDist = new Vector2(flatDir.x, flatDir.z).magnitude;
            pitch = Mathf.Atan2(lookDirection.y, horizontalDist) * Mathf.Rad2Deg;

            // Optional clamp so it doesn't go demon-mode
            // pitch = Mathf.Clamp(pitch, -45f, 45f);
        }

        // Compose: yaw first, then pitch
        Quaternion yawQ = Quaternion.AngleAxis(yaw, Vector3.forward); // Z
        Quaternion pitchQ = Quaternion.AngleAxis(pitch, Vector3.right); // X (minus is common)

        transform.localRotation = yawQ * pitchQ;

    }
}
