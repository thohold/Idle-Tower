using UnityEngine;
using TMPro;

public class GoldCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    void Start() 
    {
    }
    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.Instance.coins.ToString();
    }
}
