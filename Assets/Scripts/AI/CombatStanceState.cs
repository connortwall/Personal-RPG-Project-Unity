using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    public class CombatStanceState : State
    {
        public EnemyAttackAction[] enemyAttacks;
        public AttackState attackState;
        public PursueTargetState pursueTargetState;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;
        
        
    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimationManager enemyAnimationManager)
    {
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position,
            enemyManager.transform.position);
        enemyAnimationManager.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
        enemyAnimationManager.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
        attackState.hasPerformedAttack = false;

        if (enemyManager.isInteracting)
        {
            // reset animator values
            enemyAnimationManager.animator.SetFloat("Vertical", 0);
            enemyAnimationManager.animator.SetFloat("Horizontal", 0);

            return this;
        }
        if (distanceFromTarget > enemyManager.maximumAggroRadius)
        {
            return pursueTargetState;
        }

        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            DecideCirclingAction(enemyAnimationManager);
        }
        
        //check for attack range; use when enemy is moving towards player
        HandleRotateTowardsTarget(enemyManager);

        // if in attack range return attack state
        if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
        {
            // when leaving state
            randomDestinationSet = false;
            return attackState;
        }
        // if the player runs out of range, return pursue target state
        else
        {
            GetNewAttack(enemyManager);
        }

        return this;
        // if we are in a cool down after attackign, retirn this state anc continue attacking player
    }
    
    protected void HandleRotateTowardsTarget(EnemyManager enemyManager)
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

    protected void DecideCirclingAction(EnemyAnimationManager enemyAnimationManager)
    {
        // circle with only forward vertical movement
        // circle with running
        // circle with walking only
        WalkAroundTarget(enemyAnimationManager);
    }
    
    protected void WalkAroundTarget(EnemyAnimationManager enemyAnimationManager)
    {
        verticalMovementValue = 0.5f;

        horizontalMovementValue = Random.Range(-1, 1);
        if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
        {
            horizontalMovementValue = 0.5f;
        }
        else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
        {
            horizontalMovementValue = -0.5f;
        }
    }
    
    protected virtual void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            float distanceFromTarget =
                Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
            int maxScore = 0;
            
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                    
                }
            }
            
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;
            
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];
                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                        {
                            return;
                        }
                        temporaryScore += enemyAttackAction.attackScore;
                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                    
                }
            }
        }
}
}
