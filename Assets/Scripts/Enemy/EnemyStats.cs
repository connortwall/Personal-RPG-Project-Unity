using System;
using System.Collections;
using System.Collections.Generic;
using CW;
using UnityEngine;

namespace CW
{
    public class EnemyStats : CharacterStats
{

    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    
    public void TakeDamage(int damage, bool playAnimation)
    {
        // don't take damage if dead
        if (isDead)
        {
            return;
        }
        // reduce enemy health by damage
        currentHealth = currentHealth - damage;

        if (playAnimation)
        {
            animator.Play("Injured Stumble Idle");
        }

        if (currentHealth <= 0)
        {
                currentHealth = 0;
                if (playAnimation)
                {
                    animator.Play("Falling Back Death");
                }
                
                // remember to update animator's isDead bool (in associated character managers)
                isDead = true;
               
        }
    }
    
}
}
