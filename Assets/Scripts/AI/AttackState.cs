using UnityEngine;


namespace CW
{
    
public class AttackState : State
{

    public CombatStanceState combatStanceState;
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
    {

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        float distanceFromTarget =
            Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        
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
}
}
