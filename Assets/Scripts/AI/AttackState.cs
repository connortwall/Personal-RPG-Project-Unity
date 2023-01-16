using UnityEngine;


namespace CW
{
    
public class AttackState : State
{

    public CombatStanceState combatStanceState;
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;

    private bool willDoComboOnNextAttack = false;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
    {
        if (enemyManager.isInteracting && enemyManager.canDoCombo == false)
        {
            return this;
        }
        else if (enemyManager.isInteracting && enemyManager.canDoCombo)
        {
            willDoComboOnNextAttack = false;
            enemyAnimationManager.PlayTargetAnimation(currentAttack.actionAnimation,true);
        }

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        float distanceFromTarget =
            Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        // use when enemy is moving towards player
        HandleRotateTowardsTarget(enemyManager);
        
        if (enemyManager.isPerformingAction)
        {
            return combatStanceState;
        }
   
        // there is a current attack
        if (currentAttack != null)
        {
            // if too close to the enemy to perform current attack, get a new attack
            if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
            {
                return this;
            }
            // if we are close eneough to attack, then let us proceed
            else if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
            {
                //if our enemy is within our attack viewable anlge we attack
                if (viewableAngle <= currentAttack.maximumAttackAngle
                    && viewableAngle >= currentAttack.minimumAttackAngle)
                {
                    if (enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPerformingAction)
                    {
                        enemyAnimationManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                        enemyAnimationManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimationManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                        enemyManager.isPerformingAction = true;

                        RollForComboChance(enemyManager);

                        // can combo
                        if (currentAttack.canCombo && willDoComboOnNextAttack)
                        {
                            currentAttack = currentAttack.comboAction;
                            return this;
                        }
                        // cannot combo
                        else
                        {
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                            currentAttack = null;
                            return combatStanceState;
                        }
                        
                        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                        currentAttack = null;
                        return combatStanceState;

                    }
                }
                
            }
     
        }
        else
        {
            GetNewAttack(enemyManager);
        }

        return combatStanceState;

        // select one of the attacks based on attack scores
        // if the selected attack is not able to be used (bc of bad angle/distance) select new attack
        // if attack is viable, stop movement, attack target
        // set recovery timer to the attacks recovery time
        // return to combat stance
    }
    
    private void GetNewAttack(EnemyManager enemyManager)
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
                        if (currentAttack != null)
                        {
                            return;
                        }
                        temporaryScore += enemyAttackAction.attackScore;
                        if (temporaryScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                    
                }
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

    private void RollForComboChance(EnemyManager enemyManager)
    {
        // create a random generator
        float comboChance = Random.Range(0, 100);
        
        if (enemyManager.allowAIToPerformCombos && comboChance <= enemyManager.comboLikelihood)
        {
            willDoComboOnNextAttack = true;
        }
    }
}
}
