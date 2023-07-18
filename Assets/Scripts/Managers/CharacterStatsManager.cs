using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

namespace CW
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterAnimatorManager characterAnimatorManager;
        [Header("Team I.D.")] 
        public int teamIDNumber = 0;
        
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int magicLevel = 10;
        public float maxMagic;
        public float currentMagic;

        // base exp award ed
        public int expAwardedOnDeath = 50;
        // start game with 0
        public int expCount = 0;

        [Header("Poise")] 
        public float totalPoiseDefense; // total poise during damage caluclation
        public float offensivePoiseBonus; // the poise you gain during an attack with a weapon
        public float armorPoiseBonus; // the poise you gain from wearing armor
        public float totalPoiseResetTime = 15;
        public float poiseResetTimer = 0;
        
        [Header(("Physical Armor Absorptions"))] 
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionTorso;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;
        public float physicalDamageAbsorptionFeet;
        
      
        [Header(("Fire Armor Absorptions"))] 
        public float fireDamageAbsorptionHead;
        public float fireDamageAbsorptionTorso;
        public float fireDamageAbsorptionLegs;
        public float fireDamageAbsorptionHands;
        public float fireDamageAbsorptionFeet;

        // lightnig
        // magic etc
        
        
        public bool isDead;

        protected virtual void Awake()
        {
            // use hand IK to line up wepons with animations if they don't line up well
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }
        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefense = armorPoiseBonus;
        }

        public virtual void TakeDamage(int physicalDamage, int fireDamage, bool playDefaultDeathAnimation, string damageAnimation = "Injured Stumble Idle")
        {
            if (isDead)
                return;

            //absorption are percentages of 100
            float totalPhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) * 
                (1 - physicalDamageAbsorptionTorso / 100) * 
                (1 - physicalDamageAbsorptionLegs / 100) * 
                (1 - physicalDamageAbsorptionHands / 100) *
                (1 - physicalDamageAbsorptionFeet / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));
            Debug.Log("Total Damage absorption is:" + totalPhysicalDamageAbsorption + "%");

            float totalFireDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) * 
                (1 - fireDamageAbsorptionTorso / 100) * 
                (1 - fireDamageAbsorptionLegs / 100) * 
                (1 - fireDamageAbsorptionHands / 100) *
                (1 - fireDamageAbsorptionFeet / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));
            
            float finalDamage = physicalDamage + fireDamage; //lightning etc
            Debug.Log("Total Damage Dealt is " + finalDamage);

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                
                isDead = true;
            }
        }

        public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if (isDead)
                return;
            
            characterAnimatorManager.EraseHandIKForWeapon();

            //absorption are percentages of 100
            float totalPhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) * 
                (1 - physicalDamageAbsorptionTorso / 100) * 
                (1 - physicalDamageAbsorptionLegs / 100) * 
                (1 - physicalDamageAbsorptionHands / 100) *
                (1 - physicalDamageAbsorptionFeet / 100);

            physicalDamage = Mathf.RoundToInt(physicalDamage - (physicalDamage * totalPhysicalDamageAbsorption));
            Debug.Log("Total Damage absorption is:" + totalPhysicalDamageAbsorption + "%");

            float totalFireDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) * 
                (1 - fireDamageAbsorptionTorso / 100) * 
                (1 - fireDamageAbsorptionLegs / 100) * 
                (1 - fireDamageAbsorptionHands / 100) *
                (1 - fireDamageAbsorptionFeet / 100);

            fireDamage = Mathf.RoundToInt(fireDamage - (fireDamage * totalFireDamageAbsorption));
            
            float finalDamage = physicalDamage + fireDamage; //lightning etc
            Debug.Log("Total Damage Dealt is " + finalDamage);

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void TakePoisonDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        public virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }
    }
}