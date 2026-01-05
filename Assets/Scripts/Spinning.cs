using UnityEngine;

public class Spinning : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;


    void Update()
    {
        transform.Rotate(rotation.x * Time.deltaTime, rotation.y * Time.deltaTime, rotation.z * Time.deltaTime);
    }
}
