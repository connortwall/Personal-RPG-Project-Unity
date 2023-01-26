using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

namespace CW
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int magicLevel = 10;
        public float maxMagic;
        public float currentMagic;

        // start game with 0
        public int expCount = 0;
        
        public bool isDead;

        // review virtual void
        public virtual void TakeDamage(int damage, bool playDefaultDeathAnimation, string damageAnimation = "Injured Stumble Idle")
        {
            
        }

    }
}