using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{
   
public class WeaponInventorySlot : MonoBehaviour
{
   private PlayerInventory playerInventory;
   private WeaponSlotManager weaponSlotManager;
   private UIManager uiManager;
   
   public Image icon;
   private WeaponItem item;

   private void Awake()
   {
      playerInventory = FindObjectOfType<PlayerInventory>();
      weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
      uiManager = FindObjectOfType<UIManager>();
   }

   public void AddItem(WeaponItem newItem)
   {
      item = newItem;
      icon.sprite = item.itemIcon;
      
      gameObject.SetActive(true);
      icon.enabled = true;
   }

   public void ClearInventorySLot()
   {
      item = null;
      icon.sprite = null;
      icon.enabled = false;
      gameObject.SetActive(false);
   }

   public void EquipThisItem()
   {
      // add current item to inventory
      // equip new item
      // remove this item from inventory
      if (uiManager.rightHandSlot01Selected)
      {
         playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[0]);
         playerInventory.weaponsInRightHandSlots[0] = item;
         playerInventory.weaponsInventory.Remove(item);
      }
      else if (uiManager.rightHandSlot02Selected)
      {
         playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[1]);
         playerInventory.weaponsInRightHandSlots[1] = item;
         playerInventory.weaponsInventory.Remove(item);
      }
      else if(uiManager.leftHandSlot01Selected)
      {
         playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[0]);
         playerInventory.weaponsInLeftHandSlots[0] = item;
         playerInventory.weaponsInventory.Remove(item);
      }
      else if(uiManager.leftHandSlot02Selected)
      {
         playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[1]);
         playerInventory.weaponsInLeftHandSlots[1] = item;
         playerInventory.weaponsInventory.Remove(item);
      }
      else
      {
         return;
         Debug.Log("No item slot selected");
      }
      
      // load right and left weapon
      playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
      playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];
      
      // load weapons into weapon manager
      weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
      weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
      
      // update images in UI
      uiManager.equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
      uiManager.ResetAllSelectedSlots();
     
   }
   
}
}