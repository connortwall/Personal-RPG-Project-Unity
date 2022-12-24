using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

namespace CW
{
    
// class needs to be on same level as animator to be able to activate animation events for the character and fire animation events
public class PlayerAttacker : MonoBehaviour
{
    private PlayerAnimatorManager playerAnimatorManager;
    private PlayerManager playerManager;
    public PlayerInventory playerInventory;
    private InputHandler inputHandler;
    private WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    public void Awake()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (inputHandler.comboFlag)
        {
            playerAnimatorManager.anim.SetBool("canDoCombo", false);
            
            // play associated combo depending on previous attack
            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
            }
            else if (lastAttack == weapon.th_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_02, true);
                lastAttack = weapon.th_light_attack_02;
            }
            else if (lastAttack == weapon.th_light_attack_02)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.th_heavy_attack_01, true);
                lastAttack = weapon.th_heavy_attack_01;
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        // asign attacking weapon regardless
        weaponSlotManager.attackingWeapon = weapon;
        // two hand attack
        if (inputHandler.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_01, true);
            lastAttack = weapon.th_light_attack_01;
        }
        // one hand attack
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            lastAttack = weapon.OH_Light_Attack_1;
        }
       
    }
    
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        // two hand attack
        if (inputHandler.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(weapon.th_heavy_attack_01, true);
            lastAttack = weapon.th_heavy_attack_01;
        } 
        // one hand attack
        else
        {
            weaponSlotManager.attackingWeapon = weapon;
            playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }
    }


    #region Input Actions
    
    public void HandleRBAction()
    {
        // handle melee weapon attack
        if (playerInventory.rightWeapon.isMeleeWeapon)
        {
            PerformRBMeleeAction();
        }
        // handle spell action, handle miracle action, handle pyro action
        else if (playerInventory.rightWeapon.isSpellCaster 
                 || playerInventory.rightWeapon.isFaithCaster 
                 || playerInventory.rightWeapon.isPyroCaster)
        {
            PerformRBMagicAction(playerInventory.rightWeapon);
        }
        
    }
    #endregion

    #region Attack Actions

    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputHandler.comboFlag = true;
            inputHandler.comboFlag = false;
        }
        else
        {
            // unable to combo if player is interacting
            if (playerManager.isInteracting)
            {
                return;
            }
            if (playerManager.canDoCombo)
            {
                return;
            }
            playerAnimatorManager.anim.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventory.rightWeapon);
        }
    }

    private void PerformRBMagicAction(WeaponItem weapon)
    {
        // check for type of spell being cast
        if (weapon.isFaithCaster)
        {
            if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
            {
                // check for focus point
                // attempt to cast spell
            }
        }
    }
    

    #endregion
}
}
