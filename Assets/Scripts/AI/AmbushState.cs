using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
   


public class AmbushState : State
{
   public bool isSleeping;
   public float detectionRadius = 2;
   public string sleepAnimation;
   public string wakeAnimation;

   public LayerMask detectionLayer;

   public PursueTargetState pursueTargetState;


   private void Awake()
   {
      detectionLayer = LayerMask.GetMask("Player");
   }

   public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimationManager enemyAnimationManager)
   {
      if (enemyManager.isInteracting)
      {
         return this;
      }
      
      if (isSleeping && !enemyManager.isInteracting)
      {
         enemyAnimationManager.PlayTargetAnimation(sleepAnimation, true);
      }

      #region Handle Target Detection

      Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);
      for (int i = 0; i < colliders.Length; i++)
      {
         // search colliders and find player manger variable
         CharacterStatsManager characterStatsManager = colliders[i].transform.GetComponent<CharacterStatsManager>();
         if (characterStatsManager != null)
         {
            Vector3 targetDirection = characterStatsManager.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            
            if(viewableAngle > enemyManager.minimumDetectionAngle 
               && viewableAngle < enemyManager.maximumDetectionAngle){}

            enemyManager.currentTarget = characterStatsManager;
            isSleeping = false;
            enemyAnimationManager.PlayTargetAnimation(wakeAnimation, true);
         }
      }

      #endregion

      #region Handle State Change

      if (enemyManager.currentTarget != null)
      {
         return pursueTargetState;
      }
      else
      {
         return this;
      }

      #endregion
   }
}
}