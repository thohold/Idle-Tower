using UnityEngine;
public enum Element {
    Physical,
    Fire,
    Water,
    Earth,
    Poison,
    Holy,
    Ice,
    Arcane
}

[System.Serializable]
public struct Damage
{
    public int amount;
    public Element element;
    public bool critical;

    public Damage(int amount, Element element, bool critical)
    {
        this.amount = amount;
        this.element = element;
        this.critical = critical;
    }
}
