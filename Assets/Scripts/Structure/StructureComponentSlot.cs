using UnityEngine;

public enum StructurePartSlot
{
    Barrel,
    Base,
    Magazine,
    Totem
}
public class StructureComponentSlot : MonoBehaviour
{
    public StructurePartSlot slot;
    public Transform attachPoint;
    public GameObject currentInstance;
    public Material currentMaterial;

    public void Awake()
    {
        currentMaterial = currentInstance.GetComponent<Renderer>().materials[0];
    }
    public void Replace(GameObject replacement)
    {
        if (currentInstance) Destroy(currentInstance);

        currentInstance = Instantiate(replacement, attachPoint);
        currentInstance.transform.localPosition = Vector3.zero;
        currentInstance.transform.localScale = Vector3.one;

        ChangeMaterial(currentMaterial);
    }

    public void ChangeMaterial(Material material)
    {
        currentMaterial = material;
        var renderer = currentInstance.GetComponent<Renderer>();
        var mats = renderer.materials;
        for (int i = 0; i < mats.Length; i++) mats[i] = material;
        renderer.materials = mats;
        
    }
    

    
}
