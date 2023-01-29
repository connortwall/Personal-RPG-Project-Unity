using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class ConsumableItem : Item
{
   [Header("Item Quantity")] 
   public int maxItemAmount;
   public int currentItemAmount;

   [Header("Item Model")] 
   public GameObject itemModel;
   
   [Header("Item Animations")] 
   public string consumeAnimation;
   
   // is freezing other actions?
   public bool isInteracting;

   public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerFXManager playerFXManager)
   {
      if (currentItemAmount > 0)
      {
         playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
         
      }
      // nom ore items
      else
      {
         playerAnimatorManager.PlayTargetAnimation("Shrug", true);
      }
   }
}
}
