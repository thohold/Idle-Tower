using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StructureShopUi : MonoBehaviour
{
    [SerializeField] private GameObject deckTab;

    [SerializeField] private GameObject structDeck;
    [SerializeField] private GameObject shotButton;

    [SerializeField] private GameObject cardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseDeckTab()
    {
        deckTab.SetActive(false);
        shotButton.SetActive(true);
    }

    public void OpenDeckTab()
    {
        deckTab.SetActive(true);
        shotButton.SetActive(false);
        FillDeck();
    }

    void FillDeck()
    {
        foreach (Transform child in structDeck.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (StructureCard card in GameManager.Instance.unlockedStructureCards)
        {
            GameObject o = Instantiate(cardPrefab, structDeck.transform);
            StructureCardUi cardHolder = o.GetComponent<StructureCardUi>();
            cardHolder.card = card;
        }
    }
}
