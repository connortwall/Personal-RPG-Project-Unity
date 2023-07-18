using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
   
public class CharacterManager : MonoBehaviour
{
   private CharacterAnimatorManager characterAnimatorManager;
   private CharacterWeaponSlotManager characterWeaponSlotManager;
   [Header("Lock On Transform")]
   public Transform lockOnTransform;
   
   [Header("Combat Collider")]
   public CriticalDamageCollider backstabCollider;
   public CriticalDamageCollider riposteCollider;

   [Header("Interaction")]
   public bool isInteracting;
   
   [Header("Combat Flags")] 
   public bool canBeRiposted;
   public bool canBeParried;
   public bool canDoCombo;
   public bool isParrying;
   public bool isBlocking;
   public bool isInvulnerable;
   public bool isUsingRightHand;
   public bool isUsingLeftHand;
   public bool isTwoHandingWeapon;

   [Header("Movement Flags")] 
   public bool isRotatingWithRootMotion;
   public bool canRotate;
   public bool isSprinting;
   public bool isGrounded;
   public bool isInAir;

   [Header("Spells")] 
   public bool isFiringSpell;
   
   // damage will be inflicted during an animation event
   // used in backstab or riposte animations
   public int pendingCriticalDamage;

   protected virtual void Awake()
   {
      characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
      characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
   }
   protected virtual void FixedUpdate()
   {
      // reset two handing status if need be
      characterAnimatorManager.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHandingWeapon);
   }
}
}
