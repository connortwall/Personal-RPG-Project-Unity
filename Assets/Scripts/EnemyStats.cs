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

    public void TakeDamage(int damage)
    {
        // don't take damage if dead
        if (isDead)
        {
            return;
        }
        currentHealth = currentHealth - damage;

        animator.Play("Injured Stumble Idle");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Falling Back Death");
            // TODO: handle player death
            isDead = true;
        }
    }
    
}
}
