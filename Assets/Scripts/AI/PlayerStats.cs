using System;
using System.Collections;
using System.Collections.Generic;
using CW;
using UnityEngine;

namespace CW
{
    public class PlayerStats : CharacterStats
{

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    
    private  PlayerAnimatorManager _playerAnimatorManager;
    
    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        _playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    
    private int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        // don't take damage if dead
        if (isDead)
        {
            return;
        }
        currentHealth = currentHealth - damage;
        healthBar.SetCurrentHealth(currentHealth);
        
        _playerAnimatorManager.PlayTargetAnimation("Injured Stumble Idle", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            _playerAnimatorManager.PlayTargetAnimation("Falling Back Death", true);
            // TODO: handle player death
            isDead = true;
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }
    
}
}
