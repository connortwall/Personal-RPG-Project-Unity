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
        public MagicBar magicBar;
        public float staminaRegenerationAmount = 20;
        private float staminaRegenerationTimer = 0;
        
        private  PlayerAnimatorManager playerAnimatorManager;
    
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        magicBar = FindObjectOfType<MagicBar>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currentStamina);

        maxMagic = SetMaxMagicFromMagicLevel();
        currentMagic = maxMagic;
        magicBar.SetMaxMagic(maxMagic);
        magicBar.SetCurrentMagic(currentMagic);
        
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

    private float SetMaxMagicFromMagicLevel()
    {
        maxMagic = magicLevel * 10;
        return maxMagic;
    }
    
    public override void TakeDamage(int damage, bool playAnimation, string damageAnimation = "Injured Stumble Idle")
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
        
        if (playAnimation)
        {
            playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);
        }
        
        if (currentHealth <= 0)
        {
                currentHealth = 0;
                if (playAnimation)
                {
                    playerAnimatorManager.PlayTargetAnimation("Falling Back Death", true);
                } 

                // remember to update animator's isDead bool (in associated character managers)
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

    public void HealPlayer(int healAmount)
    {
        // heal player for heal amount
        currentHealth = currentHealth + healAmount;
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        // update health bar ui
        healthBar.SetCurrentHealth(currentHealth);
    }

    public void DeductMagic(int magicPoints)
    {
        currentMagic = currentMagic - magicPoints;
        // ensure value is at least 0
        if (currentMagic < 0)
        {
            currentMagic = 0;
        }
        magicBar.SetCurrentMagic(currentMagic);
    }

    public void AddExp(int exp)
    {
        expCount = expCount + exp;
    }
}
}
