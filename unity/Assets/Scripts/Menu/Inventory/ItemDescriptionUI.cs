﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

[ExecuteInEditMode]
public class ItemDescriptionUI : MonoBehaviour {

    [SerializeField] private Text generalStatsText;
    [SerializeField] private Text abilityNames;
    [SerializeField] private Text abilityStatDescriptors;
    [SerializeField] private Text abilityStatValues;
    private InventoryMenu parentMenu;
    private ItemData itemData;
    [SerializeField] private UnitWeaponCombiner combiner;

    private void OnItemDataChange()
    {
        if (itemData == null)
        {
            generalStatsText.text = "";
            abilityNames.enabled = false;
            abilityStatDescriptors.enabled = false;
            abilityStatValues.enabled = false;
            return;
        }

        if (itemData.IsType(ItemType.Weapon))
        {
            WeaponData weaponData = itemData.WeaponData;
            UnitData unitData = ParentMenu.UnitData;
            combiner = new UnitWeaponCombiner(unitData, weaponData);

            abilityNames.enabled = true;
            abilityStatDescriptors.enabled = true;
            abilityStatValues.enabled = true;

            if (true)
            {
                generalStatsText.text = string.Format(
                    "<size=+24>{0:F1}</size> damage / second\n" +
                    "<size=+4><#{1}><u>{2} - {3}</u></color></size> damage {4}\n" +
                    "<size=+4><#{5}><u>{6:F2}</u></color></size> attacks / second {7}\n" +
                    "<size=+4><#{8}><u>{9:F2}m</u></color></size> reach {10}",
                    combiner.DamagePerSecond,
                    ColorUtility.ToHtmlStringRGB(StatValueTextColor(weaponData, combiner.DamageBonusData)),
                    combiner.MinDamage,
                    combiner.MaxDamage,
                    DamageBonusInfoString(weaponData, combiner.DamageBonusData),
                    ColorUtility.ToHtmlStringRGB(StatValueTextColor(weaponData, combiner.SpeedBonusData)),
                    combiner.AttacksPerSecond,
                    SpeedBonusInfoString(weaponData, combiner.SpeedBonusData),
                    ColorUtility.ToHtmlStringRGB(StatValueTextColor(weaponData, combiner.ReachBonusData)),
                    combiner.WeaponReach,
                    ReachBonusInfoString(weaponData, combiner.ReachBonusData)
                    );

                abilityStatValues.text = string.Format(
                    "{0}\n{1}\n\n{2}\n{3}\n\n{4}\n{5}",
                    weaponData.StrRequired,
                    AbilityBonusValueString(weaponData.StrDamageBonus, weaponData.StrSpeedBonus, weaponData.StrReachBonus),
                    weaponData.DexRequired,
                    AbilityBonusValueString(weaponData.DexDamageBonus, weaponData.DexSpeedBonus, weaponData.DexReachBonus),
                    weaponData.IntRequired,
                    AbilityBonusValueString(weaponData.IntDamageBonus, weaponData.IntSpeedBonus, weaponData.IntReachBonus)
                    );
            }
            else
            {
                generalStatsText.text = "Locked";
            }
        }
    }

    private Color StatValueTextColor(float baseValue, float strBonus, float dexBonus, float intBonus)
    {
        float total = baseValue + strBonus + dexBonus + intBonus;
        float baseComponent = baseValue / total;
        float colorComponent = 1 - baseComponent;
        float maxBonus = Mathf.Max(strBonus, dexBonus, intBonus);
        if (maxBonus == 0)
        {
            return Color.white;
        }
        else
        {
            return new Color(
                baseComponent + colorComponent * (strBonus / maxBonus),
                baseComponent + colorComponent * (dexBonus / maxBonus),
                baseComponent + colorComponent * (intBonus / maxBonus)
                );
        }
    }

    private Color StatValueTextColor(WeaponData weaponData, UnitWeaponCombiner.AbilityStatBonus bonus)
    {
        float baseValue = 0;
        if(bonus is UnitWeaponCombiner.DamageBonus)
        {
            baseValue = weaponData.AverageDamage;
        }
        else if(bonus is UnitWeaponCombiner.SpeedBonus)
        {
            baseValue = weaponData.TotalAttackTime;
        }
        else if(bonus is UnitWeaponCombiner.ReachBonus)
        {
            baseValue = weaponData.Reach;
        }
        return StatValueTextColor(baseValue, bonus.StrComponent, bonus.DexComponent, bonus.IntComponent);
    }

    private string DamageBonusInfoString(WeaponData weaponData, UnitWeaponCombiner.DamageBonus bonus)
    {
        string result = "";
        if(bonus.Value > 0)
        {
            result += string.Format("({0} - {1} ", weaponData.MinDamage, weaponData.MaxDamage);
            if(bonus.StrComponent > 0)
            {
                result += string.Format("<#FF4444>+ {0:F0}</color>", bonus.StrComponent);
            }
            result += ")";
        }
        return result;        
    }

    private string SpeedBonusInfoString(WeaponData weaponData, UnitWeaponCombiner.SpeedBonus bonus)
    {
        string result = "";
        if (bonus.Value > 0)
        {
            result += string.Format("({0:F2} ", weaponData.AttacksPerSecond);
            if (bonus.DexComponent > 0)
            {
                result += string.Format("<#44FF44>+ {0:F2}</color>", bonus.DexComponent);
            }
            result += ")";
        }
        return result;
    }

    private string ReachBonusInfoString(WeaponData weaponData, UnitWeaponCombiner.ReachBonus bonus)
    {
        string result = "";
        if (bonus.Value > 0)
        {
            result += string.Format("({0:F2} ", weaponData.Reach);
            if (bonus.StrComponent > 0)
            {
                result += string.Format("<#FF4444>+ {0:F2}</color>", bonus.StrComponent);
            }
            if (bonus.DexComponent > 0)
            {
                result += string.Format("<#44FF44>+ {0:F2}</color>", bonus.DexComponent);
            }
            if (bonus.IntComponent > 0)
            {
                result += string.Format("<#4444FF>+ {0:F2}</color>", bonus.IntComponent);
            }
            result += ")";
        }
        return result;
    }

    private string AbilityBonusValueString(float damageBonus, float speedBonus, float reachBonus)
    {
        float[] bonuses = { damageBonus, speedBonus, reachBonus };
        if(Mathf.Max(bonuses) == 0)
        {
            return " - ";
        }

        var result = new System.Text.StringBuilder();
        bool someTextBefore = false;
        for(int i = 0; i<bonuses.Length; i++)
        {
            if (bonuses[i] > 0)
            {
                if (someTextBefore)
                {
                    result.Append(", ");
                }
                result.Append("+");
                switch (i)
                {
                    case 0:
                        result.AppendFormat("{0:F1} damage", bonuses[i]);
                        break;
                    case 1:
                        result.AppendFormat("{0:P} speed", bonuses[i]);
                        break;
                    case 2:
                        result.AppendFormat("{0:F2} reach", bonuses[i]);
                        break;
                }
                someTextBefore = true;
            }
        }
        return result.ToString();
    }

    public ItemData ItemData
    {
        set
        {
            if (value != itemData)
            {
                itemData = value;
                OnItemDataChange();
            }
            else
            {
                return;
            }
        }
    }

    private InventoryMenu ParentMenu
    {
        get
        {
            if (parentMenu == null)
            {
                parentMenu = GetComponentInParent<InventoryMenu>();
            }
            Debug.Assert(parentMenu != null);
            return parentMenu;
        }
    }
}
