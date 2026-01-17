using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class CardUI
{
    public Image artwork;
    public Button button;
    public TMP_Text cardLevel;

}

public class StructureUpgradeUI : MonoBehaviour
{
    private Structure structure;

    private bool upgrading;
    private UpgradeCard[] deck;
    [SerializeField] private GameObject cardTab;

    [SerializeField] private List<CardUI> cardPreviews;

    [SerializeField] private Sprite emptyCardSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgrading = false;
        cardTab.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void UpgradeEnter()
    {
        structure = GameManager.Instance.currentSelectedStructure;
        List<UpgradeCard> availableCards = GameManager.Instance.unlockedUpgradeCards.Intersect(structure.allowedCards).ToList();
        if (availableCards.Count == 0) return;
        upgrading = true;
        this.structure = structure;
        Time.timeScale = 0;
        cardTab.SetActive(true);
        NewDeck(availableCards);
    }

    public void UpgradeExit(int number)
    {
        structure.Xp -= structure.NextXp;
        structure.NextXp = (int)(structure.NextXp + structure.NextXpAddition);
        structure.ApplyCard(deck[number]);
        upgrading = false;
        this.structure = null;   
        Time.timeScale = 1;
        cardTab.SetActive(false);
    }

    void NewDeck(List<UpgradeCard> possibleCards)
    {
        deck = new UpgradeCard[3];
        int drawCount = Mathf.Min(3, possibleCards.Count);

        for (int i = 0; i < drawCount; i++)
        {
            int index = Random.Range(0, possibleCards.Count);
            UpgradeCard card = possibleCards[index];
            possibleCards.RemoveAt(index);

            deck[i] = card;
            UpdateUI(cardPreviews[i], card);
        }

        for (int i = drawCount; i < cardPreviews.Count; i++)
        {
            deck[i] = null;
            UpdateUI(cardPreviews[i], null);
        }
        
    }


    void UpdateUI(CardUI cardPreview, UpgradeCard card)
    {
        if (card != null)
        {
        cardPreview.artwork.sprite = card.artwork;
        cardPreview.cardLevel.text = (structure.GetCardLevel(card) + 1).ToString();
        cardPreview.button.interactable = true;
        }
        else
        {
        cardPreview.artwork.sprite = emptyCardSprite;
        cardPreview.cardLevel.text = "";
        cardPreview.button.interactable = false;
        }
    }
}
