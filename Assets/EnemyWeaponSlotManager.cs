using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class EnemyWeaponSlotManager : MonoBehaviour
{
   // for now manually assign weapons to hands
   public WeaponItem rightHandWeapon;
   public WeaponItem leftHandWeapon;
   
   private WeaponHolderSlot rightHandSlot;
   private WeaponHolderSlot leftHandSlot;

   private DamageCollider leftHandDamageCollider;
   private DamageCollider rightHandDamageCollider;

   private void Awake()
   {
      WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
      // assign weapon slots
      foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
      {
         if (weaponSlot.isLeftHandSlot)
         {
            leftHandSlot = weaponSlot;
         }
         else if (weaponSlot.isRightHandSlot)
         {
            rightHandSlot = weaponSlot;
         }
      }
   }

   private void Start()
   {
      LoadWeaponsOnBothHands();
   }

   private void LoadWeaponsOnBothHands()
   {
      if (rightHandWeapon != null)
      {
         LoadWeaponOnSlot(rightHandWeapon, false);
      }

      if (leftHandWeapon != null)
      {
         LoadWeaponOnSlot(leftHandWeapon, true);
      }
   }

   public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
   {
      if (isLeft)
      {
         leftHandSlot.currentWeapon = weapon;
         leftHandSlot.LoadWeaponModel(weapon);
         // load weapon damage collider
         LoadWeaponsDamageCollider(true);
      }
      else
      {
         rightHandSlot.currentWeapon = weapon;
         rightHandSlot.LoadWeaponModel(weapon);
         LoadWeaponsDamageCollider(false);

         // load weapon damage collider
      }
   }

   public void LoadWeaponsDamageCollider(bool isLeft)
   {
      // is left hand
      if (isLeft)
      {
         // find damage collider and assign it
         leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
      }
      // is right hand
      else
      {
         // find damage collider and assign it
         rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();

      }
      
   }

   public void OpenDamageCollider()
   {
      rightHandDamageCollider.EnableDamageCollider();
   }

   public void CloseDamageCollider()
   {
      rightHandDamageCollider.DisableDamageCollider();
   }
   
   #region Handle Weapon Stamina Drainage

   public void DrainStaminaLightAttack()
   {
   }
        
   public void DrainStaminaHeavyAttack()
   {
   }

   #endregion


}
}
