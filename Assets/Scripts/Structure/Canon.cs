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

        Transform barrel = transform.GetChild(0);
        foreach(Transform child in barrel)
        {
            barrels.Add(child.gameObject);
        }
                
    }

    // Update is called once per frame
    void Update()
    {
        targets = GetClosestEnemies();

        if (targets.Count > 0)
        {
        GameObject mainTarget = targets[0]; 

        lookDirection = mainTarget.transform.position - transform.position;
        Vector3 flatDir = new Vector3(lookDirection.x, 0f, lookDirection.z);
        float yaw = (Mathf.Atan2(flatDir.x, flatDir.z) * Mathf.Rad2Deg) -180;
        transform.localRotation = Quaternion.Euler(0f, 0f, yaw);
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

        UpdateStats();

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

    public void UpdateStats()
    {
        atkSpeed = baseAtkSpeed * AtkSpeed;
        range = baseRange * Range;
        collider.radius = range;
        speed = baseSpeed * Speed;
        size = baseSize * Size;

    }

    public void UpdateAmmoVisuals()
    {
        foreach (VisualHolder holder in GetComponentsInChildren<VisualHolder>())
        {
            holder.SwitchVisual(bulletPreview);
        }

    }

    public void UpdateComponent(GameObject prefab, int index)
    {
        Transform old = transform.GetChild(index);
        Destroy(old.gameObject);
        GameObject newO = Instantiate(prefab, transform);
        newO.transform.SetSiblingIndex(index);
        Transform barrel = transform.GetChild(0);
        barrels.Clear();
        foreach(Transform child in barrel)
        {
            barrels.Add(child.gameObject);
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
                projectile.EffectStrength = EffectStrengthMultiplier;
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
}
