using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartReplacement
{
    public StructurePartSlot slot;
    public GameObject part;
    public Material mat;
}

public abstract class UpgradeCard : Card
{
    public List<PartReplacement> partReplacements;
    public Material baseMatReplacement;
    public string relation;
    public int maxLevel = 1;

    public void Apply(Structure structure, int level)
    {
        PartReplacement replacement = null;
        if (partReplacements.Count >= level) replacement = partReplacements[level - 1];
        if (replacement != null)
        {
            if (structure.components.TryGetValue(replacement.slot, out var slot))
            {
                if (replacement.part != null) slot.Replace(replacement.part);
                if (replacement.mat != null) slot.ChangeMaterial(replacement.mat);
            }
        }

        if (baseMatReplacement != null) structure.ChangeMaterial(baseMatReplacement);

        ApplyUpgrade(structure, level);
    }
    public abstract void ApplyUpgrade(Structure tower, int level);
}