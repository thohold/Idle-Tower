using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerStatUi : MonoBehaviour
{
    private Structure structure;
    private Canon canon;

 
    // STATS
    [SerializeField] TMP_Text damageText;
    [SerializeField] TMP_Text atkSpeedText;
    [SerializeField] TMP_Text rangeText;
    [SerializeField] TMP_Text sizeText;
    [SerializeField] TMP_Text speedText;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        structure = GameManager.Instance.currentSelectedStructure;
        if (structure != null) canon = structure.canon;

        if (canon != null)
        {
            damageText.text = canon.Damage.ToString();
            atkSpeedText.text = canon.atkSpeed.ToString();
            rangeText.text = canon.range.ToString();
            speedText.text = canon.speed.ToString();
            sizeText.text = canon.Size.ToString();
        }






    }
}
