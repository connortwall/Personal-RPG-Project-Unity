using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
   [CreateAssetMenu(menuName = "Spells/Healing Spell")]
public class HealingSpell : SpellItem
{

   public int healAmount;

   public override void AttemptToCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
   {
      base.AttemptToCastSpell(playerAnimatorManager, playerStats);
      GameObject instantiateWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorManager.transform);
      playerAnimatorManager.PlayTargetAnimation(spellAnimation, true);
      Debug.Log("Attempting to cast spell");
   }

   public override void SuccessfullyCastSpell(PlayerAnimatorManager playerAnimatorManager, PlayerStats playerStats)
   {
      // also fires function in original spell item class
      base.SuccessfullyCastSpell(playerAnimatorManager,playerStats);
      GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorManager.transform);
      playerStats.HealPlayer(healAmount);
      Debug.Log("Spell cast successful");
   }
}

}
