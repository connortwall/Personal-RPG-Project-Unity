using System;
using UnityEngine;

namespace CW
{
    
public class IdleState : State
{
    public LayerMask detectionLayer;
    public PursueTargetState pursueTargetState;

    public void Awake()
    {
        pursueTargetState = GetComponentInChildren<PursueTargetState>();
        detectionLayer = LayerMask.GetMask("Player");
    }

    public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimationManager enemyAnimationManager)
    {
        if (enemyManager.isInteracting)
        {
            return this;
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        #region Handle Enemy Target Detection
        
        // look for potential target i.e. for object with character stats and check if its in view
        for (int i = 0; i < colliders.Length; i++)
        {
            // detect anything on detection that has character stats
            CharacterStatsManager characterStatsManager = colliders[i].transform.GetComponent<CharacterStatsManager>();
            if (characterStatsManager != null && characterStatsManager.teamIDNumber != enemyStatsManager.teamIDNumber){
                // check for team ID (enemy or teamate)
                Vector3 targetDirection = characterStatsManager.transform.position - transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                // if it can see the target
                if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                { 
                    enemyManager.currentTarget = characterStatsManager;
                    // TODO: bug when game is run, it is null
                    return pursueTargetState;
                }
            }
            
        }
        #endregion

        #region Handle Switching To Next State

        // if target is found switch to pursue target state 
        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        // if not target return to idle state   
        else
        {
            return this;
        }

        #endregion
       
    }
}
}