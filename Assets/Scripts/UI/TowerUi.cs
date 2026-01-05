using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUi : MonoBehaviour
{
    private Structure structure;
    private Canon canon;

    [SerializeField] private GameObject structureTab;
    [SerializeField] private GameObject portalTab;
    [SerializeField] private GameObject shopTab;


    // INFO
    [SerializeField] Image cardPreview;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text descText;
    // XP
    [SerializeField] private GameObject xpTab;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text xpText;

    // UPGRADE BUTTON
    [SerializeField] private Button upgradeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgradeButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        structure = GameManager.Instance.currentSelectedStructure;
        if (structure == null)
        {
            structureTab.SetActive(false);
            portalTab.SetActive(false);
            xpTab.SetActive(false);
            shopTab.SetActive(true);
        }
        else
        {
            switch (structure.Type)
            {
                case StructureType.Deployed:
                    canon = structure.GetComponentInChildren<Canon>();
                    structureTab.SetActive(true);
                    portalTab.SetActive(false);
                    xpTab.SetActive(true);
                    shopTab.SetActive(false);
                    nameText.text = structure.Name;
                    descText.text = structure.Description;
                    levelText.text = structure.Level.ToString();
                    cardPreview.sprite = structure.CardSprite;
                    break;
                case StructureType.Portal:
                    xpTab.SetActive(true);
                    portalTab.SetActive(true);
                    structureTab.SetActive(false);
                    shopTab.SetActive(false);
                    break;
            }
            
            xpSlider.value = (float)((float)structure.Xp / (float)structure.NextXp);
            xpText.text = structure.Xp.ToString() + "/" + structure.NextXp;
                
            if (structure.Xp >= structure.NextXp)
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }
        }
    }
}
