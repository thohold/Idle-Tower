using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float floatSpeed = 1f;

    private float timer;

    public void Init(int amount, bool critical, Element element)
    {
        text.text = amount.ToString() + (critical ? "!" : "");
        text.color = GetColor(element);
        timer = lifetime;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer -= Time.deltaTime;

        if (timer <= 0f)
            Destroy(gameObject);
    }

    Color GetColor(Element element)
    {
        return element switch
        {
            Element.Physical => Color.white,
            Element.Earth => new Color(0.75f, 0.6f, 0.4f),
            Element.Poison => Color.green,
            Element.Fire   => new Color(1f, 0.4f, 0f),
            Element.Ice    => Color.cyan,
            Element.Arcane => new Color(1f, 0.407f, 0.74f),
            _                    => Color.white
        };
    }
}