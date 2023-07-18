using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace CW
{
    
public class QuickSlotsUI : MonoBehaviour
{
    public Image currentSpellIcon;
    public Image currentConsumableIcon;
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;

    public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
    {
        // is right
        if (isLeft == false)
        {
            if (weapon.itemIcon != null)
            {
                // assign icon
                rightWeaponIcon.sprite = weapon.itemIcon;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                // if no icon
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
        }
        // is left
        else
        {
            if (weapon.itemIcon != null)
            {
                leftWeaponIcon.sprite = weapon.itemIcon;
                leftWeaponIcon.enabled = true;
            }
            else
            {
                // if no icon
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }
            
        }
    }

    public void UpdateCurrentSpellIcon(SpellItem spell)
    {
        if (spell.itemIcon != null)
        {
            currentSpellIcon.sprite = spell.itemIcon;
            currentSpellIcon.enabled = true;
        }
        else
        {
            currentSpellIcon.sprite = null;
            currentSpellIcon.enabled = false;
        }
    }
    
    public void UpdateCurrentConsumableIcon(ConsumableItem consumableItem)
    {
        if (consumableItem.itemIcon != null)
        {
            currentConsumableIcon.sprite = consumableItem.itemIcon;
            currentConsumableIcon.enabled = true;
        }
        else
        {
            currentConsumableIcon.sprite = null;
            currentConsumableIcon.enabled = false;
        }
    }
    
    
}
}
