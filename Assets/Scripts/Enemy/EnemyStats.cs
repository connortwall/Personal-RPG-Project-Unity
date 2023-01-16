using System;
using System.Collections;
using System.Collections.Generic;
using CW;
using UnityEngine;

namespace CW
{
    public class EnemyStats : CharacterStats
{  
    private EnemyAnimationManager enemyAnimationManager;

    public UIEnemyHealthBar enemyHealthBar;
    // base exp award ed
    public int expAwardedOnDeath = 50;
    
    private void Awake()
    {
        enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage, bool playAnimation, string damageAnimation = "Injured Stumble Idle")
    {
        // don't take damage if dead
        if (isDead)
        {
            return;
        }

        // reduce enemy health by damage
        currentHealth = currentHealth - damage;
        enemyHealthBar.SetHealth(currentHealth);
        
        enemyAnimationManager.PlayTargetAnimation(damageAnimation, true);
        
        if (currentHealth <= 0)
        {
            HandleDeath(playAnimation);
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
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.AddExp(expAwardedOnDeath);
        }
    }
}
}
