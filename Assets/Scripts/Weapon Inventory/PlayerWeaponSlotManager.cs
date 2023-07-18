using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        
        private InputHandler inputHandler;
        private PlayerInventoryManager playerInventoryManager;
        private PlayerManager playerManager;
        private PlayerStatsManager playerStatsManager;
        private PlayerFXManager playerFXManager;
        private PlayerAnimatorManager playerAnimatorManager;
        private CameraHandler cameraHandler;
        private QuickSlotsUI quickSlotsUI;

        protected override void Awake()
        {
            base.Awake();
            inputHandler = GetComponent<InputHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerManager = GetComponent<PlayerManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerFXManager = GetComponent<PlayerFXManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        }
        
        public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    // save current weapon in slot then load weapon item model to slot
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.leftHandIdleAnimation,false, true);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        //move current left weapon to the back of charcter or disable it, 0.2f for crossfade
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        // destroy model in hand
                        leftHandSlot.UnloadWeaponAndDestroy();
                        playerAnimatorManager.PlayTargetAnimation("Left_Arm_Idle_01",false, true);
                    }
                    else
                    {
                        // if there is a back weapon unload and destroy when switching back and forth
                        backSlot.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false,weaponItem);
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;

                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, unarmedWeapon);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.leftHandIdleAnimation,false, true);
                }
                else
                {
                    playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, unarmedWeapon);
                    playerAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }

        public void SuccessfullyThrowFireBomb()
        {
            Destroy(playerFXManager.instantiatedFXModel);
            BombConsumableItem fireBombItem = playerInventoryManager.currentConsumableItem as BombConsumableItem; 
            GameObject activeModelBomb = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            
            // throw bomb where camera is aiming up and down, and thrown using players facing direction
            activeModelBomb.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y,0);
            
            //detect bomb damage collider
            BombDamageCollider bombDamageCollider = activeModelBomb.GetComponentInChildren<BombDamageCollider>();

            bombDamageCollider.explosionDamage = fireBombItem.baseDamage;
            bombDamageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            // add force to rigid body to move through the air
            bombDamageCollider.bombRigidbody.AddForce(activeModelBomb.transform.forward * fireBombItem.forwardVelocity);
            bombDamageCollider.bombRigidbody.AddForce(activeModelBomb.transform.up * fireBombItem.upwardVelocity);
            bombDamageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
            LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
            // check for friendly fire
            
        }
        
        public void EnableCombo()
        {
            //animator.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            //animator.SetBool("canDoCombo", false);
        }
        
        public void DrainStaminaLightAttack()
        {
            playerStatsManager.TakeStaminaDamage(
                Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultipler));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStatsManager.TakeStaminaDamage(
                Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        
    }
}