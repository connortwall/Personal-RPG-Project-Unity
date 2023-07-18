using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class RotateTowardsTargetState : State
{
 public CombatStanceState combatStanceState;
 
 public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimationManager enemyAnimationManager)
 {
  enemyAnimationManager.animator.SetFloat("Vertical",0);
  enemyAnimationManager.animator.SetFloat("Horizontal",0);

  // find difference in positions
  Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
  // will return value bewteen -180 and 180
  float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

  if (enemyManager.isInteracting)
  {
   return this; // when interacting, still interact, pausing here unitl the completion of action
  }
  
  //first angle
  if (viewableAngle >= 100 && viewableAngle <= 180 && !enemyManager.isInteracting)
  {
   enemyAnimationManager.PlayTargetAnimationWithRootRotation("sword and shield 180 turn", true);
   return this;
  }
  
  else if (viewableAngle <= -100 && viewableAngle <= -180 && !enemyManager.isInteracting)
  {
   enemyAnimationManager.PlayTargetAnimationWithRootRotation("sword and shield 180 turn", true);
   return combatStanceState;
  }
  else if (viewableAngle <= -45 && viewableAngle <= -100 && !enemyManager.isInteracting)
  {
   enemyAnimationManager.PlayTargetAnimationWithRootRotation("sword and shield right turn", true);
   return combatStanceState;
  }
  else if (viewableAngle >= 45 && viewableAngle <= 100 && !enemyManager.isInteracting)
  {
   enemyAnimationManager.PlayTargetAnimationWithRootRotation("sword and shield left turn", true);
   return combatStanceState;
  }

  return combatStanceState;
 }
}
}
