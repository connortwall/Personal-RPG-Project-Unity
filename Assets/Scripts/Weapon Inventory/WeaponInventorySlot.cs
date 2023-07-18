using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{
   
public class WeaponInventorySlot : MonoBehaviour
{
   private PlayerInventoryManager playerInventoryManager;
   private PlayerWeaponSlotManager playerWeaponSlotManager;
   private UIManager uiManager;
   
   public Image icon;
   private WeaponItem item;

   private void Awake()
   {
      playerInventoryManager = FindObjectOfType<PlayerInventoryManager>();
      playerWeaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();
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
         playerInventoryManager.weaponsInventory.Add(playerInventoryManager.weaponsInRightHandSlots[0]);
         playerInventoryManager.weaponsInRightHandSlots[0] = item;
         playerInventoryManager.weaponsInventory.Remove(item);
      }
      else if (uiManager.rightHandSlot02Selected)
      {
         playerInventoryManager.weaponsInventory.Add(playerInventoryManager.weaponsInRightHandSlots[1]);
         playerInventoryManager.weaponsInRightHandSlots[1] = item;
         playerInventoryManager.weaponsInventory.Remove(item);
      }
      else if(uiManager.leftHandSlot01Selected)
      {
         playerInventoryManager.weaponsInventory.Add(playerInventoryManager.weaponsInLeftHandSlots[0]);
         playerInventoryManager.weaponsInLeftHandSlots[0] = item;
         playerInventoryManager.weaponsInventory.Remove(item);
      }
      else if(uiManager.leftHandSlot02Selected)
      {
         playerInventoryManager.weaponsInventory.Add(playerInventoryManager.weaponsInLeftHandSlots[1]);
         playerInventoryManager.weaponsInLeftHandSlots[1] = item;
         playerInventoryManager.weaponsInventory.Remove(item);
      }
      else
      {
         Debug.Log("No item slot selected");
         return;
         
      }
      
      // load right and left weapon
      playerInventoryManager.rightWeapon = playerInventoryManager.weaponsInRightHandSlots[playerInventoryManager.currentRightWeaponIndex];
      playerInventoryManager.leftWeapon = playerInventoryManager.weaponsInLeftHandSlots[playerInventoryManager.currentLeftWeaponIndex];
      
      // load weapons into weapon manager
      playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
      playerWeaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
      
      // update images in UI
      uiManager.equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventoryManager);
      uiManager.ResetAllSelectedSlots();
     
   }
   
}
}