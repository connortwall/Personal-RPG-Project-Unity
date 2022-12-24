using System;
using System.Collections;
using System.Collections.Generic;
using CW;
using UnityEngine;

namespace CW
{
    public class PlayerStats : CharacterStats
    {
        private PlayerManager playerManager;
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public float staminaRegenerationAmount = 20;
        private float staminaRegenerationTimer = 0;
        
        private  PlayerAnimatorManager playerAnimatorManager;
    
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
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
    
    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        if (playerManager.isInvulnerable)
        {
            return;
        }
        // don't take damage if dead
        if (isDead)
        {
            return;
        }
        currentHealth = currentHealth - damage;
        healthBar.SetCurrentHealth(currentHealth);
        
        playerAnimatorManager.PlayTargetAnimation("Injured Stumble Idle", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimatorManager.PlayTargetAnimation("Falling Back Death", true);
            // TODO: handle player death
            isDead = true;
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }

    // regenerates players stamina
    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            staminaRegenerationTimer = 0;
        }
        
        else
        {
            staminaRegenerationTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
    }
    
}
}
