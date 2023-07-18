using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class CharacterWeaponSlotManager : MonoBehaviour
{
    protected CharacterManager characterManager;
    protected CharacterStatsManager characterStatsManager;
    protected CharacterFXManager characterFXManager;
    protected CharacterInventoryManager characterInventoryManager;
    protected CharacterAnimatorManager characterAnimatorManager;
    
    [Header("Unarmed Weapon")]
    public WeaponItem unarmedWeapon;

    [Header("Weapon Slots")]
    // public to allow access by spell itself and add warm up FX
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;

    [Header("Damage Colliders")]
    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    [Header("Attacking Weapon")] 
    public WeaponItem attackingWeapon;

    [Header("Hand IK Targets")] 
    public RightHandIKTarget rightHandIKTarget;
    public LeftHandIKTarget leftHandIKTarget;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterFXManager = GetComponent<CharacterFXManager>();
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        LoadWeaponHolderSlots();
    }

    protected virtual void LoadWeaponHolderSlots()
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
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }
    
    public virtual void LoadBothWeaponsOnSlots()
    {
        LoadWeaponOnSlot(characterInventoryManager.rightWeapon, false);
        LoadWeaponOnSlot(characterInventoryManager.leftWeapon, true);
    }
    
     public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    // save current weapon in slot then load weapon item model to slot
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.leftHandIdleAnimation,false, true);
                }
                else
                {
                    if (characterManager.isTwoHandingWeapon)
                    {
                        //move current left weapon to the back of charcter or disable it, 0.2f for crossfade
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        // destroy model in hand
                        leftHandSlot.UnloadWeaponAndDestroy();
                        characterAnimatorManager.PlayTargetAnimation("Left_Arm_Idle_01",false, true);
                    }
                    else
                    {
                        // if there is a back weapon unload and destroy when switching back and forth
                        backSlot.UnloadWeaponAndDestroy();
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTargets(characterManager.isTwoHandingWeapon);
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;

                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadLeftWeaponDamageCollider();
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.leftHandIdleAnimation,false, true);
                }
                else
                {
                    characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadRightWeaponDamageCollider();
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }
     
     protected virtual void LoadLeftWeaponDamageCollider()
     {
         leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
         leftHandDamageCollider.physicalDamage = characterInventoryManager.leftWeapon.physicalDamage;
         leftHandDamageCollider.fireDamage = characterInventoryManager.leftWeapon.fireDamage;
         leftHandDamageCollider.poiseBreak = characterInventoryManager.leftWeapon.poiseBreak;
         leftHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;
         characterFXManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
     }

     protected virtual void LoadRightWeaponDamageCollider()
     {
         rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
         rightHandDamageCollider.physicalDamage = characterInventoryManager.rightWeapon.physicalDamage;
         rightHandDamageCollider.fireDamage = characterInventoryManager.rightWeapon.fireDamage;
         rightHandDamageCollider.poiseBreak = characterInventoryManager.rightWeapon.poiseBreak;
         rightHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;
         characterFXManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
     }

     public virtual void LoadTwoHandIKTargets(bool isTwoHandingWeapon)
     {
         // assigned at right hand slot bc only right hand weapon becomes two handed
         leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
         rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
         characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
     }
     
     
     // open damage colliders for animation events
     public virtual void OpenDamageCollider()
     {
         if (characterManager.isUsingRightHand)
         {
             rightHandDamageCollider.EnableDamageCollider();
         }
         else if (characterManager.isUsingLeftHand)
         {
             leftHandDamageCollider.EnableDamageCollider();
         }
     }

     // close damage colliders for animation events
     public virtual void CloseDamageCollider()
     {
         if (rightHandDamageCollider != null)
         {
             rightHandDamageCollider.DisableDamageCollider();
         }

         if (leftHandDamageCollider != null)
         {
             leftHandDamageCollider.DisableDamageCollider();
         }
     }
     
     public virtual void GrantWeaponAttackingPoiseBonus()
     {
         characterStatsManager.totalPoiseDefense =
             characterStatsManager.totalPoiseDefense + attackingWeapon.offensivePoiseBonus;
     }

     public virtual void ResetWeaponAttackingPoiseBonus()
     {
         characterStatsManager.totalPoiseDefense = characterStatsManager.armorPoiseBonus;
     }

    
}
}