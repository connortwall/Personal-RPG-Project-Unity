using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CW
{
    public class EnemyBossManager : MonoBehaviour
    {
        public TMP_Text bossName;
        
        private UIBossHealthBar bossHealthBar;
        private EnemyStatsManager enemyStatsManager;

        private EnemyAnimationManager enemyAnimationManager;

        private BossCombatStanceState bossCombatStanceState;
        // handle switching phase
        // handle switching attack phase
        [Header("Second Phase FX")] 
        public GameObject particleFX;
        
        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyAnimationManager = GetComponent<EnemyAnimationManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStatsManager.maxHealth);
            
        }

        public void UpdateBossHealthBar(int currentHealth, int maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
            if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
            {
                bossCombatStanceState.hasPhaseShifted = true;
                ShiftToSecondPhase();
            }
        }

        public void ShiftToSecondPhase()
        {
            enemyAnimationManager.animator.SetBool("isInvulnerable",true);
            enemyAnimationManager.animator.SetBool("isPhaseShifting",true);
            // play an animation w event that trigger partcle FX
            enemyAnimationManager.PlayTargetAnimation("Sword_and_Shield_Power_Up",true);
            //switch attack actions
            bossCombatStanceState.hasPhaseShifted = true;
        }
    }
}
