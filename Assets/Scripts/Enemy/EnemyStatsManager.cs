using System;
using System.Collections;
using System.Collections.Generic;
using CW;
using UnityEngine;

namespace CW
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        private EnemyManager enemyManager;
    private EnemyAnimationManager enemyAnimationManager;
    private EnemyBossManager enemyBossManager;
    public UIEnemyHealthBar enemyHealthBar;
    
    public bool isBoss;
    
    protected override void Awake()
    {
        base.Awake();
        enemyManager = GetComponent<EnemyManager>();
        enemyAnimationManager = GetComponent<EnemyAnimationManager>();
        enemyBossManager = GetComponent<EnemyBossManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isBoss)
        {
            enemyHealthBar.SetMaxHealth(maxHealth);
        }
    }

    public override void HandlePoiseResetTimer()
    {
         if (poiseResetTimer > 0) 
         {
         poiseResetTimer = poiseResetTimer - Time.deltaTime;
                    }
                    else if(poiseResetTimer <= 0 && !enemyManager.isInteracting)
                    {
                        totalPoiseDefense = armorPoiseBonus;
                    }
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
    {
        base.TakeDamageNoAnimation(physicalDamage, fireDamage);
        
        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
    }

    public override void TakePoisonDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        base.TakePoisonDamage(damage);
        
        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            enemyAnimationManager.PlayTargetAnimation("Falling Back Death",true);
        }
    }

    public void BreakGuard()
    {
        enemyAnimationManager.PlayTargetAnimation("parried", true);
    }
    public override void TakeDamage(int physicalDamage, int fireDamage, bool playDefaultDeathAnimation, string damageAnimation = "Injured Stumble Idle")
    {
      
            base.TakeDamage(physicalDamage, fireDamage, playDefaultDeathAnimation, damageAnimation = "Injured Stumble Idle");
        

        if (!isBoss)
        {
            // reduce enemy health by damage
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if(isBoss && enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
        }
        enemyAnimationManager.PlayTargetAnimation(damageAnimation, true);
        
        if (currentHealth <= 0)
        {
            HandleDeath(playDefaultDeathAnimation);
        }

}

    private void HandleDeath(bool playAnimation)
    {
        currentHealth = 0;
        if (playAnimation)
        {
            enemyAnimationManager.PlayTargetAnimation("Falling Back Death", true);
        }
                
        // remember to update animator's isDead bool (in associated character managers)
        isDead = true;
        
        //scan for every player in the scene, award them souls
        // could search for player who damages enemy
        PlayerStatsManager playerStatsManager = FindObjectOfType<PlayerStatsManager>();

        if (playerStatsManager != null)
        {
            playerStatsManager.AddExp(expAwardedOnDeath);
        }
    }
}
}
