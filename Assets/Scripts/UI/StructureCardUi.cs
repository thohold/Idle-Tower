using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StructureCardUi : MonoBehaviour
{
    [field: SerializeField] public StructureCard card {get; set;}
    [SerializeField] private Image cardPreview;
    [SerializeField] private Button cardButton;
    [SerializeField] private TMP_Text priceText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardPreview.sprite = card.artwork;
        priceText.text = card.price.ToString();
        
    }

    void Update()
    {
        if (GameManager.Instance.coins < card.price)
        {
            cardButton.interactable = false;
            priceText.color = Color.red;
        }
        else
        {
            cardButton.interactable = true;
            priceText.color = Color.yellow;
        }
    }

    public void SelectCard()
    {
        FreeCamera.Instance.EnterPlaceMode(card.preview, card.prefab, card.price);
    }
}
