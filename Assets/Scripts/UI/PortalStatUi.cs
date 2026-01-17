using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PortalUi : MonoBehaviour
{
    private Portal portal;
    public List<Button> selectedMobButtons;
    public List<Image> selectedMobButtonImages;
    private int selectedSlot;
    public TMP_Text totalCostText;
    public TMP_Text spawnrateText;
    public GameObject mobCatalogue;
    public GameObject mobCardGrid;
    public GameObject mobCardPreset;

    public Sprite emptyCardSprite;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentSelectedStructure != null) portal = 
        GameManager.Instance.currentSelectedStructure.GetComponentInChildren<Portal>();

        if (GameManager.Instance.currentSelectedStructure != null) UpdateUI();
    }



    void UpdateUI()
    {
        spawnrateText.text = portal.SpawnRate.ToString("F2");
        totalCostText.text = portal.currentCost.ToString() + "/" + portal.MaxCost.ToString();

        for (int i = 0; i < selectedMobButtons.Count; i++)
        {
            if (i < portal.Capacity)
            {
                selectedMobButtons[i].interactable = true;
                if (portal.SpawnList[i] != null) selectedMobButtonImages[i].sprite = portal.SpawnList[i].artwork;
                else selectedMobButtonImages[i].sprite = emptyCardSprite;
            }
            else
            {
                selectedMobButtons[i].interactable = false;
            }
        }
    }

    public void SelectSlot(int index)
    {
        selectedSlot = index;
        mobCatalogue.SetActive(true);
        OpenCatalogue();
    }

    public void OpenCatalogue()
    {
        foreach (Transform child in mobCardGrid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (MobCard mob in GameManager.Instance.unlockedMobCards)
        {
            GameObject g = Instantiate(mobCardPreset, mobCardGrid.transform);
            MobCardUi mobCard = g.GetComponent<MobCardUi>();
            mobCard.mob = mob;
            mobCard.portalUi = this;
        }
    }

    public void CloseCatalogue()
    {
        mobCatalogue.SetActive(false);
    }

    public void SelectCard(MobCard card)
    {
        if (portal.MaxCost < (portal.GetTotalCost() + card.cost - portal.GetCardCost(selectedSlot))) return;

        portal.SpawnList[selectedSlot] = card;
        portal.UpdateStats();
        UpdateUI();
    }
}
