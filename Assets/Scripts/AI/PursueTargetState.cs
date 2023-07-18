using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class PursueTargetState : State
{
    public CombatStanceState combatStanceState;
    public RotateTowardsTargetState rotateTowardsTargetState;
    
    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimationManager enemyAnimationManager)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);
        HandleRotateTowardsTarget(enemyManager);

        
        if (enemyManager.isInteracting)
        {
            return this;
        }
        // chase the target
        // if within attack range return combat stance stae
        // if target is out of range return this state and continue to chase target
        
        // use when enemy is moving towards player
        HandleRotateTowardsTarget(enemyManager);
        
        
        // dont move is in process of attacking or just attacked
        if (enemyManager.isPerformingAction)
        {
            enemyAnimationManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            return this;
        }
        
        // move towards player
        if (distanceFromTarget > enemyManager.maximumAggroRadius)
        {
            enemyAnimationManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        }

        if (distanceFromTarget <= enemyManager.maximumAggroRadius)
        {
            return combatStanceState;
        }
        else
        {
            return this;
        }
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
