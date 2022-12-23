using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
    {
        //check for attack range
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);
        
        // TODO: potantially circle player or walk around them
        
        // this fixes bug of chrqcter still walking abfter attacking
        if (enemyManager.isPerformingAction)
        {
            enemyAnimationManager.anim.SetFloat("Vertical",0, 0.1f, Time.deltaTime);
        }
        
        // if in attack range return attack state
        if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
        {
            return attackState;
        }
        // if the player runs out of range, return pursue target state
        else if (distanceFromTarget > enemyManager.maximumAttackRange)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        // if we are in a cool down after attackign, retirn this state anc continue attacking player
    }
}
}
