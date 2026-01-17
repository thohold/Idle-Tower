using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MobCardUi : MonoBehaviour
{

    public MobCard mob;
    public Image cardPreview;
    public TMP_Text costText;
    public Button selectButton;
    public PortalUi portalUi;
    public Sprite emptyCardSprite;
    

    void Start()
    {
        if (mob != null)
        {
        cardPreview.sprite = mob.artwork;
        costText.text = mob.cost.ToString();
        }
    }
    public void Select()
    {
        portalUi.SelectCard(mob);
    }
}
