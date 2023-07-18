using System;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Animations;

namespace CW
{
    
// class needs to be on same level as animator to be able to activate animation events for the character and fire animation events
public class PlayerCombatManager : MonoBehaviour
{
    private InputHandler inputHandler;
    private CameraHandler cameraHandler;
    private PlayerManager playerManager;
    private PlayerAnimatorManager playerAnimatorManager;
    private PlayerEquipmentManager playerEquipmentManager;
    private PlayerStatsManager playerStatsManager;
    public PlayerInventoryManager playerInventoryManager;
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    private PlayerFXManager playerFXManager;

    [Header("Attack Animations")] 
    private string oh_light_attack_01 = "OH_LightAttack_01"; 
    private string oh_light_attack_02 = "OH_LightAttack_02"; 
    private string oh_heavy_attack_01 = "OH_HeavyAttack_01"; 
    private string oh_heavy_attack_02 = "OH_HeavyAttack_02";
    
    private string th_light_attack_01 = "TH_LightAttack_01"; 
    private string th_light_attack_02 = "TH_LightAttack_02"; 
    private string th_heavy_attack_01 = "TH_HeavyAttack_01"; 
    private string th_heavy_attack_02 = "TH_HeavyAttack_01";

    private string weapon_art = "Weapon_Art";
    
    public string lastAttack;
    // its on layer 12, search on layer 12
    private LayerMask backstabLayer = 1 << 12;
    // 13th
    private LayerMask riposteLayer = 1 << 13;

    public void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        cameraHandler = FindObjectOfType<CameraHandler>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerFXManager = GetComponent<PlayerFXManager>();
    }

    public void Update()
    {
        Debug.DrawRay(inputHandler.criticalAttackRaycatStartPoint.position, transform.TransformDirection(Vector3.forward) * 2, Color.cyan, 1f, false);
    }
    
    
    public void HandleRBAction()
    {
        playerAnimatorManager.EraseHandIKForWeapon();
        // handle melee weapon attack
        if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformRBMeleeAction();
        }
        // handle spell action, handle miracle action, handle pyro action
        else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster 
                 || playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster 
                 || playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster)
        {
            PerformRBMagicAction(playerInventoryManager.rightWeapon);
        }
        
    }
    
    public void HandleLBAction()
    {
        PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
        if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            // perform shield weapon art
            PerformLTWeaponArt(inputHandler.twoHandFlag);
        }
        else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
        {
            // perform light attack
        }
    }
    

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        //check if sufficient stamina, if not return
        if (playerStatsManager.currentStamina <= 0)
        {
            return;
        }
        
        if (inputHandler.comboFlag)
        {
            playerAnimatorManager.animator.SetBool("canDoCombo", false);
            
            // play associated combo depending on previous attack
            if (lastAttack == oh_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(oh_light_attack_02, true);
            }
            else if (lastAttack == th_light_attack_01)
            {
                playerAnimatorManager.PlayTargetAnimation(th_light_attack_02, true);
                lastAttack = th_light_attack_02;
            }
            else if (lastAttack == th_light_attack_02)
            {
                playerAnimatorManager.PlayTargetAnimation(th_heavy_attack_01, true);
                lastAttack = th_heavy_attack_01;
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        //check if sufficient stamina, if not return
        if (playerStatsManager.currentStamina <= 0)
        {
            return;
        }
        
        // assign attacking weapon regardless
        playerWeaponSlotManager.attackingWeapon = weapon;
        // two hand attack
        if (inputHandler.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(th_light_attack_01, true);
            lastAttack = th_light_attack_01;
        }
        // one hand attack
        else
        {
            playerAnimatorManager.PlayTargetAnimation(oh_light_attack_01, true);
            lastAttack = oh_light_attack_01;
        }
       
    }
    
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        //check if sufficient stamina, if not return
        if (playerStatsManager.currentStamina <= 0)
        {
            return;
        }
        
        // two hand attack
        if (inputHandler.twoHandFlag)
        {
            playerAnimatorManager.PlayTargetAnimation(th_heavy_attack_01, true);
            lastAttack = th_heavy_attack_01;
        } 
        // one hand attack
        else
        {
            playerWeaponSlotManager.attackingWeapon = weapon;
            playerAnimatorManager.PlayTargetAnimation(oh_heavy_attack_01, true);
            lastAttack = oh_heavy_attack_01;
        }
    }

    
    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputHandler.comboFlag = true;
            HandleWeaponCombo(playerInventoryManager.rightWeapon);
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
            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
            HandleLightAttack(playerInventoryManager.rightWeapon);
        }
        playerFXManager.PlayWeaponFX(false);
    }
    
    private void PerformLBBlockingAction()
    {
        if (playerManager.isInteracting)
        {
            return;
        }

        // cant start block if already blocking
        if (playerManager.isBlocking)
        {
            return;
        }
        playerAnimatorManager.PlayTargetAnimation("block", false, true);
        playerEquipmentManager.EnableBlockingCollider();
        playerManager.isBlocking = true;
    }
    
    private void PerformRBMagicAction(WeaponItem weapon)
    {
        // break if player is interacting, prevents spam casting of spell
        if (playerManager.isInteracting)
        {
            return;
        }
        // check for type of spell being cast
        if (weapon.weaponType == WeaponType.FaithCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
            {
                // check for focus point
                if (playerStatsManager.currentMagic >= playerInventoryManager.currentSpell.magicCost){
                    // attempt to cast spell
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                }
                // play an alternate out of magic animation
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrugging",true);
                }
            }
        }
        else if (weapon.weaponType == WeaponType.PyromancyCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
            {
                // check for focus point
                if (playerStatsManager.currentMagic >= playerInventoryManager.currentSpell.magicCost){
                    // attempt to cast spell
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                }
                // play an alternate out of magic animation
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrugging",true);
                }
            }
        }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting)
        {
            return;
        }
        // if two ahanding perfrom 
        if (isTwoHanding)
        {
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon_art, true);
        // else perfom left handed weapon animaiton
        }
        
      
    }

    
    // animation to be called as an animation event chose wichi frame of aniatio  to cast spell
    private void SuccessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
        playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }
    
    public void AttemptBackstabOrRiposte()
    {
        // shoot raycast out of player when holding control
        RaycastHit hit;
        
        Debug.Log("attempting critical");
        if (Physics.Raycast(inputHandler.criticalAttackRaycatStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, .5f, backstabLayer))
        {
            Debug.Log("attempting backstab");
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            //damage logic for critical
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;
            
            // if found a enemy with a character manager
            if (enemyCharacterManager != null)
            {
                // check for team ID so you cant backstab allies or self
                // pull player into a transform behind enemy so backstab animation is clean
                // TODO: can use lerp to make transition smoother
                playerManager.transform.position = enemyCharacterManager.backstabCollider.criticalDamagerStandPosition.position;
                // rotate player toards the transform
                Vector3 rotationDirection = playerManager.transform.eulerAngles;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                // having these variables separated allows for buffs and other modification later
                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier *
                                     rightWeapon.physicalDamage;
                // assign damage to enemy
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                
                // make enemy play animatiom
                playerAnimatorManager.PlayTargetAnimation("Backstab", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Backstabbed", true);
                
                // do damage

            }
        }
        
        
        else if (Physics.Raycast(inputHandler.criticalAttackRaycatStartPoint.position,
                     transform.TransformDirection(Vector3.forward), out hit, 2f, riposteLayer))
        {
            
            // check for team id
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                Debug.Log("attempting parry riposte");
                playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0; 
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;
            
                // having these variables separated allows for buffs and other modification later
                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier *
                                 rightWeapon.physicalDamage;
            // assign damage to enemy
            enemyCharacterManager.pendingCriticalDamage = criticalDamage;
            playerAnimatorManager.PlayTargetAnimation("riposte_stab", true);
            enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("riposted", true);
            }
        }

    }
}
}
