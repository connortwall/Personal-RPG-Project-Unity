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
        if (enemyManager.isInteracting)
        {
            return this;
        }
        //check for attack range
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);
        
        // TODO: potantially circle player or walk around them
        
        // use when enemy is moving towards player
        HandleRotateTowardsTarget(enemyManager);
        
        // this fixes bug of character still walking after attacking
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
    
    private void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        
        // rotate manually
        if (enemyManager.isPerformingAction)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();
     
            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }
     
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        // rotate with pathfinding (navmesh agent)
        else
        {
            // TODO: review this to understand
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;
         
            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            enemyManager.enemyRigidbody.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        // want a hybrid system that uses navmesh and is brainless to be able to follow you easily on ground and off a cliff
        
    }
}
}
