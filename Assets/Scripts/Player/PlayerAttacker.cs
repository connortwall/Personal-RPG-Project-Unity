using System;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Animations;

namespace CW
{
    
// class needs to be on same level as animator to be able to activate animation events for the character and fire animation events
public class PlayerAttacker : MonoBehaviour
{
    private PlayerAnimatorManager playerAnimatorManager;
    private PlayerManager playerManager;
    private PlayerStats playerStats;
    public PlayerInventory playerInventory;
    private InputHandler inputHandler;
    private WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    // its on layer 12, search on layer 12
    private LayerMask backstabLayer = 1 << 12;
    // 13th
    private LayerMask riposteLayer = 1 << 13;

    public void Awake()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        inputHandler = GetComponentInParent<InputHandler>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void Update()
    {
        Debug.DrawRay(inputHandler.criticalAttackRaycatStartPoint.position, transform.TransformDirection(Vector3.forward) * 2, Color.cyan, 1f, false);
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        //check if sufficient stamina, if not return
        if (playerStats.currentStamina <= 0)
        {
            return;
        }
        
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
        //check if sufficient stamina, if not return
        if (playerStats.currentStamina <= 0)
        {
            return;
        }
        
        // assign attacking weapon regardless
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
        //check if sufficient stamina, if not return
        if (playerStats.currentStamina <= 0)
        {
            return;
        }
        
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
    

    public void HandleLBAction()
    {
        PerformLBBlockingAction();
    }

    public void HandleLTAction()
    {
        if (playerInventory.leftWeapon.isShieldWeapon)
        {
            // perform shield weapon art
            PerformLTWeaponArt(inputHandler.twoHandFlag);
        }
        else if (playerInventory.leftWeapon.isMeleeWeapon)
        {
            // perform light attack
        }
    }
    #endregion

    #region Attack Actions

    private void PerformRBMeleeAction()
    {
        if (playerManager.canDoCombo)
        {
            inputHandler.comboFlag = true;
            HandleWeaponCombo(playerInventory.rightWeapon);
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
        // break if player is interacting, prevents spam casting of spell
        if (playerManager.isInteracting)
        {
            return;
        }
        // check for type of spell being cast
        if (weapon.isFaithCaster)
        {
            if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
            {
                // check for focus point
                if (playerStats.currentMagic >= playerInventory.currentSpell.magicCost){
                    // attempt to cast spell
                    playerInventory.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStats);
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
            playerAnimatorManager.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
        // else perfom left handed weapon animaiton
        }
        
      
    }

    // having a successfully cast spell here allows for
    // animation to be called as an animation event, same level as model, now can chose wichi frame of aniatio  to cast spell
    private void SuccessfullyCastSpell()
    {
        playerInventory.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStats);
    }
    
    #endregion
    
    #region Defense Actions
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
        playerManager.isBlocking = true;
    }
    #endregion

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
            DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;
            
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
                int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier *
                                     rightWeapon.currentWeaponDamage;
                // assign damage to enemy
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                
                // make enemy play animatiom
                playerAnimatorManager.PlayTargetAnimation("Backstab", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Backstabbed", true);
                
                // do damage

            }
        }
        
        
        else if (Physics.Raycast(inputHandler.criticalAttackRaycatStartPoint.position,
                     transform.TransformDirection(Vector3.forward), out hit, 2f, riposteLayer))
        {
            
            // check for team id
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

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
                int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier *
                                 rightWeapon.currentWeaponDamage;
            // assign damage to enemy
            enemyCharacterManager.pendingCriticalDamage = criticalDamage;
            playerAnimatorManager.PlayTargetAnimation("riposte_stab", true);
            enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("riposted", true);
            }
        }

    }
}
}
