﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemData", menuName = "Data/Item/Data")]
public class ItemData : UniqueScriptableObject {

    [SerializeField] private string displayName;

    [SerializeField] private List<Skill> skills;
    [SerializeField] private List<Perk> perks;
    [SerializeField] private EquipmentSlot slot;
    [SerializeField] private ItemGraphics graphics;

    public List<Perk> Perks
    {
        get { return perks; }
    }

    public EquipmentSlot Slot
    {
        get { return slot; }
    }

    public ItemGraphics Graphics
    {
        get { return graphics; }
    }

}
