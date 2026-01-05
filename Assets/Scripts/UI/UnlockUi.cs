using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CardType {
    Upgrade,
    Structure
}
public class UnlockUi : MonoBehaviour
{
    private CardType currentUnlock;
    [SerializeField] private Image glow;
    [SerializeField] private CardUI cardPreview;

    private UpgradeCard currentUpgradeCard;



    public void EnterUnlockWindow(UpgradeCard card)
    {
        Time.timeScale = 0;
        currentUpgradeCard = card;
        cardPreview.cardImage.sprite = card.artwork;
        cardPreview.cardName.text = card.cardName;
        cardPreview.cardDesc.text = card.description;
        cardPreview.cardRelation.text = card.relation;
        cardPreview.cardLevel.text = card.maxLevel.ToString();
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
    }

    public void Confirm()
    {
        switch (currentUnlock)
        {
            case CardType.Upgrade:
                GameManager.Instance.unlockedUpgradeCards.Add(currentUpgradeCard);
                break;
        }

        currentUpgradeCard = null;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
