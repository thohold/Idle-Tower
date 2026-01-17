using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    private Enemy owner;
    public readonly List<EffectInstance> activeEffects = new();
    public bool dirty {get; set;}

    void Awake()
    {
        owner = GetComponent<Enemy>();
    }

    void Update()
    {
        float dt = Time.deltaTime;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];
            effect.Update(dt);

            if (effect.remainingDuration <= 0f)
            {
                effect.definition.OnExit(effect);
                activeEffects.RemoveAt(i);
                dirty = true;
            }
        }
    }

    public void AddEffect(EffectSO effectSO, float strength, float durationAdd, float size)
    {
        foreach (var effect in activeEffects)
        {
            if (effect.definition == effectSO)
            {
                effect.Refresh();
                effect.definition.OnStack(effect);
                dirty = true;
                return;
            }
        }

        activeEffects.Add(new EffectInstance(effectSO, owner, strength, durationAdd, size));
        dirty = true;
    }

    public void OnOwnerDeath()
    {
        foreach (var effect in activeEffects)
        {
            effect.OnOwnerDeath();
        }

        activeEffects.Clear();
    }
}
