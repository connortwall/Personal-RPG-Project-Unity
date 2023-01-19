using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    public class WeaponSlotManager : MonoBehaviour
    {
        private PlayerManager playerManager;
        private PlayerInventory playerInventory;
        
        // public to allow access by spell itself and add warm up FX
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        private WeaponHolderSlot backSlot;

        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        public WeaponItem attackingWeapon;

        private Animator animator;
        private QuickSlotsUI quickSlotsUI;
        private PlayerStats playerStats;
        private InputHandler inputHandler;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();
            
            
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
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                // save current weapon in slot
                leftHandSlot.currentWeapon = weaponItem;
                // load weapon item model to slot
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    //move current left weapon to the back of charcter or disable it, 0.2f for crossfade
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    // destroy model in hand
                    leftHandSlot.UnloadWeaponAndDestroy();
                    animator.CrossFade(weaponItem.two_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Both Arms Empty", 0.2f);
                    // if there is a back weapon unload and destroy when switching back and forth
                    backSlot.UnloadWeaponAndDestroy();
                    
                    #region Handle Right Weapon Idle Animations

                    if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right Arm Empty", 0.2f);
                    }

                    #endregion
                }

                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
           

        }

        #region Handle Weapon's Damage Collider
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            // Weapons will get damage from base weapon damage
            leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
        }
        
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;

        }
        
        // open damage colliders for animation events
        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }
        
        // close damage colliders for animation events
        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamageCollider.DisableDamageCollider();
        }
        
        #endregion

        #region Handle Weapon Stamina Drainage

        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultipler));
        }
        
        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

        #endregion
        
        public void EnableCombo()
        {
            //anim.SetBool("canDoCombo", true);
        }
        
        public void DisableCombo()
        {
            //anim.SetBool("canDoCombo", false);
        }
        
        
    }
}