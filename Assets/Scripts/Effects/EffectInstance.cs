using UnityEngine;

public class EffectInstance
{
    public EffectSO definition;
    public Enemy owner;

    public float duration;
    public float remainingDuration;
    public int stacks;

    private float tickTimer;

    public float strengthMultiplier = 1;
    public float durationMultiplier = 1;
    public float size;

    public EffectInstance(EffectSO definition, Enemy owner, float strength, float durationMultiplier, float size)
    {
        this.definition = definition;
        this.owner = owner;
        this.strengthMultiplier = strength;
        
        this.durationMultiplier = durationMultiplier;
        duration = definition.duration;
        remainingDuration = duration;
        stacks = 1;

        definition.OnEnter(this);
    }

    public void Update(float dt)
    {
        remainingDuration -= dt;
        tickTimer += dt;

        if (tickTimer > definition.tickInterval)
        {
            tickTimer = 0;
            definition.Tick(this);
        }
    }

    public void Refresh()
    {
        if (definition.refreshDurationOnReapply)
            remainingDuration = duration;

        if (definition.stackOnReapply && stacks < definition.maxStacks)
            stacks++;
    }

    public void OnOwnerDeath()
    {
        definition.OnOwnerDeath(this);
    }
}
