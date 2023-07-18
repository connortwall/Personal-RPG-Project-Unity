using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using CW;
using UnityEngine;

namespace CW
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        private PlayerManager playerManager;
        
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public MagicBar magicBar;
        
        public float staminaRegenerationAmount = 2;
        private float staminaRegenerationTimer = 0;
        
        private  PlayerAnimatorManager playerAnimatorManager;
    
        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            healthBar = GetComponentInChildren<HealthBar>();
            staminaBar = GetComponentInChildren<StaminaBar>();
            magicBar = GetComponentInChildren<MagicBar>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
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

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0) 
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if(poiseResetTimer <= 0 && !playerManager.isInteracting)
            {
                totalPoiseDefense = armorPoiseBonus;
            }
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
    
        public override void TakeDamage(int physicalDamage, int fireDamage, bool playDefaultDeathAnimation, string damageAnimation = "Injured Stumble Idle")
        {
            if (playerManager.isInvulnerable)
                return;

                // don't take damage if dead
            base.TakeDamage(physicalDamage, fireDamage, playDefaultDeathAnimation, damageAnimation = "Injured Stumble Idle");
            healthBar.SetCurrentHealth(currentHealth);
        
            if (playDefaultDeathAnimation)
            {
                playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);
            }
        
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                if (playDefaultDeathAnimation)
                {
                    playerAnimatorManager.PlayTargetAnimation("Falling Back Death", true);
                } 

                // remember to update animator's isDead bool (in associated character managers)
                isDead = true;
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (isDead)
            {
                return;
            }
            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                playerAnimatorManager.PlayTargetAnimation("Falling Back Death",true);
            }
        }
        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage);
            healthBar.SetCurrentHealth(currentHealth);
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