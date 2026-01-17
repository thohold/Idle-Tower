using UnityEngine;
using System.Collections.Generic;


public abstract class EffectSO : ScriptableObject
{
    [Header("Effect Settings")]
    public float duration;
    public int maxStacks = 1;
    public bool refreshDurationOnReapply = true;
    public bool stackOnReapply = false;
    public float tickInterval = 1f;

    public List<ElementResistance> resistanceModifiers;

    public abstract void OnEnter(EffectInstance effect);

    public abstract void Tick(EffectInstance effect);

    public abstract void OnStack(EffectInstance effect);

    public abstract void OnExit(EffectInstance effect);

    public abstract void OnOwnerDeath(EffectInstance effect);


}
