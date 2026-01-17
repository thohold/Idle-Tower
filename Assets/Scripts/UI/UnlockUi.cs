using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UnlockUi : MonoBehaviour
{
    [SerializeField] private Image glow;
    [SerializeField] private CardUI cardPreview;

    private Card currentCard;



    public void EnterUnlockWindow(Card card)
    {
        Time.timeScale = 0;
        cardPreview.artwork.sprite = card.artwork;
        cardPreview.button.interactable = true;
        switch (card.rarity)
        {
            case Rarity.Common:
                glow.color = Color.white;
                break;
            case Rarity.Uncommon:
                glow.color = Color.green;
                break;
            case Rarity.Rare:
                glow.color = Color.blue;
                break;
            case Rarity.Epic:
                glow.color = Color.magenta;
                break;
            case Rarity.Legendary:
                glow.color = Color.yellow;
                break;
        }

        if (card is UpgradeCard uCard) GameManager.Instance.unlockedUpgradeCards.Add(uCard);
        if (card is MobCard mCard) GameManager.Instance.unlockedMobCards.Add(mCard);
        if (card is StructureCard sCard) GameManager.Instance.unlockedStructureCards.Add(sCard);
    }

    public void Confirm()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
