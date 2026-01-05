using UnityEngine;
using System.Collections.Generic;

public class VisualHolder : MonoBehaviour
{
    [SerializeField] private List<Transform> visualSpots;

    public void SwitchVisual(GameObject prefab)
    {
        foreach (Transform pos in visualSpots)
        {
            foreach (Transform child in pos.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            Instantiate(prefab, pos);
        }
    }

}
