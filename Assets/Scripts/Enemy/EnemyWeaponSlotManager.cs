using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
{

   public override void GrantWeaponAttackingPoiseBonus()
   {
      characterStatsManager.totalPoiseDefense = characterStatsManager.totalPoiseDefense + characterStatsManager.offensivePoiseBonus;
   }

   public override void ResetWeaponAttackingPoiseBonus()
   {
      characterStatsManager.totalPoiseDefense = characterStatsManager.armorPoiseBonus;
   }
   
   public void DrainStaminaLightAttack()
   {
      }

   public void DrainStaminaHeavyAttack()
   {
      }



}
}
